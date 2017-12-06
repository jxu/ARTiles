/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    private VuMarkManager mVuMarkManager;
    protected TrackableBehaviour mTrackableBehaviour;
    private Dictionary<string, string> furnitureDictionary = 
        new Dictionary<string, string>();

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    void Update()
    {
        //UpdateRenderers();
        int targets = GetTargetsSize();
        Debug.Log("Number of targets currently: #" + targets);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.Trackable.ID + " found");
            OnTrackingFound(); 
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    public VuMarkTarget GetVuMarkObj()
    {
        /* By design, grabs the first VuMarkObj available */
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
 
        foreach (var bhvr in mVuMarkManager.GetActiveBehaviours())
        {
            return bhvr.VuMarkTarget;
        }
        return null;
    }

    public IEnumerable<VuMarkTarget> GetVuMarkTargets()
    {
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        return mVuMarkManager.GetActiveVuMarks();
    }

    public string FurnitureLookup(VuMarkTarget vuMarkObj)
    {
        if (vuMarkObj == null)
        {
            Debug.Log("Warning: VuMark ID Null in furniture lookup");
        }
        string vuMarkID = GetVuMarkId(vuMarkObj).Trim();
        Debug.Log("VuMark ID is " + vuMarkID);
        string vuMarkName = null;
        if (furnitureDictionary.TryGetValue(vuMarkID, out vuMarkName))
            return vuMarkName;
        return "";
    }


    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    public string GetVuMarkId(VuMarkTarget vumark)
    {
        switch (vumark.InstanceId.DataType)
        {
            case InstanceIdType.BYTES:
                return vumark.InstanceId.HexStringValue;
            case InstanceIdType.STRING:
                return vumark.InstanceId.StringValue;
            case InstanceIdType.NUMERIC:
                return vumark.InstanceId.NumericValue.ToString();
        }
        return string.Empty;
    }

    private void UpdateRenderers()
    {
        var nRendererComponents = GetComponentsInChildren<RendererManager>(true);
        IEnumerable<VuMarkTarget> targets = GetVuMarkTargets();
        int count = 0;
        foreach (var target in targets)
        {
            count += 1;
            string name = FurnitureLookup(target);
            foreach(var component in nRendererComponents)
            {
                //if (component.Rendering())
                  //  continue;
                string componentName = component.getName();
                bool loadVariable = name.CompareTo(componentName) == 0;
                if (loadVariable)
                    Debug.LogFormat("Loading #{0}: {1} ", count, name);
                component.SetState(loadVariable);
            }
        }
    }

    protected virtual void OnTrackingFound()
    {
        //Debug.Log("Some object found...");
        var nRendererComponents = GetComponentsInChildren<RendererManager>(true);
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        populateDictionary();
        IEnumerable<VuMarkTarget> allTargets = GetVuMarkTargets();

        /* Enable rendering in GameObjects with multiple children 
         * (if name matches): */

        foreach (var component in nRendererComponents)
        {
            string componentName = component.getName();
            foreach (var target in allTargets)
            {
                string vuMarkName = FurnitureLookup(target);
                component.SetState(vuMarkName.CompareTo(componentName) == 0);
            }
        }
        
        // Ensures colliders are disabled
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Unsure if Canvas needs to be child of object or should be attached 
        // to the camera itself
        foreach (var component in canvasComponents)
            component.enabled = false;

    }

    private int GetTargetsSize()
    {
        int targets = 0;
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        foreach (var bhvr in mVuMarkManager.GetActiveBehaviours())
        {
            //Debug.Log("Sorting through VuMark Manager");
            targets += 1;
        }
        return targets;
    }

    private void populateDictionary()
    {
        furnitureDictionary = new Dictionary<string, string>
        {
            {"1", "Chair"},
            {"999", "Chair"},
            {"2", "Table"},
            {"3", "Vase"}
        };
    }

    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }




    #endregion // PRIVATE_METHODS
}