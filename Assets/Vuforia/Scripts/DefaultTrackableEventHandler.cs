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
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound(); 
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
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

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    VuMarkTarget GetVuMarkObj()
    {
        VuMarkTarget target = null;
        mVuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        foreach (var bhvr in mVuMarkManager.GetActiveBehaviours())
        {
            Debug.Log("Sorting through VuMark Manager");
            target = bhvr.VuMarkTarget;
        }
        return target;
    }

    string GetVuMarkId(VuMarkTarget vumark)
    {
        vumark = GetVuMarkObj();
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

    protected virtual void OnTrackingFound()
    {
        var nRendererComponents = GetComponentsInChildren<RendererManager>(true);
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        populateDictionary();
        VuMarkTarget vuMarkObj = GetVuMarkObj();
        if (vuMarkObj == null)
        {
            Debug.Log("WARNING vuMarkObj is NULL");
        }
        string vuMarkID = GetVuMarkId(vuMarkObj).Trim();
        if (vuMarkID == null) {
            Debug.Log("WARNING: vuMarkID is NULL");
        }
        string vuMarkName = null;
        if (furnitureDictionary.TryGetValue(vuMarkID, out vuMarkName))
        {
            Debug.Log("vuMarkID accepted, value = " + vuMarkID);
            Debug.Log("vuMarkName is " + vuMarkName);
        }
        else
        {
            Debug.Log("vuMarkID rejected, value = " + vuMarkID);
            return;
        }
        /* Enable rendering in GameObjects with multiple children 
         * (if name matches): */
        foreach (var component in nRendererComponents)
        {
            string componentName = component.getName();
            bool loadVariable = vuMarkName.CompareTo(componentName) == 0;
            Debug.Log("Component name found = " + componentName);
            Debug.Log("loadVariable value = " + loadVariable.ToString());
            /* If the component name is equal to the VuMarkID, it will be loaded */
            component.SetState(loadVariable);
        }
        /* Enables renderering in single Game Objects (if name matches) */
        //foreach (var component in rendererComponents)
        //{
        //    string componentName = component.name;
        //    bool loadVariable = vuMarkName.CompareTo(componentName) == 0;
        //    Debug.Log("SComponent name found = " + componentName);
        //    Debug.Log("SloadVariable value = " + loadVariable.ToString());

        //    component.enabled = loadVariable;
        //}

        // Ensures colliders are disabled
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Unsure if Canvas needs to be child of object or should be attached 
        // to the camera itself
        foreach (var component in canvasComponents)
            component.enabled = false;

    }

    private void populateDictionary()
    {
        furnitureDictionary = new Dictionary<string, string>
        {
            {"1", "Chair"},
            {"2", "Table"},
            {"3", "Cup"}
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