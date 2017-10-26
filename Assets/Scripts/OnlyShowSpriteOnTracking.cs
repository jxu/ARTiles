using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

// TODO: Support arbitrary paintings

public class OnlyShowSpriteOnTracking : MonoBehaviour, ITrackableEventHandler 
{
	private TrackableBehaviour mTrackableBehaviour;
	GameObject painting;

	void Start()
	{
		// Get child painting
		painting = transform.GetChild(0).gameObject;

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}
		

	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("Showing image target");
			painting.SetActive(true);

		}
		else
		{
			Debug.Log("Hiding image target");
			painting.SetActive(false);
		}
	}   
}
