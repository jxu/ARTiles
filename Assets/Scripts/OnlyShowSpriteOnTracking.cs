using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

// TODO: Support arbitrary paintings

public class OnlyShowSpriteOnTracking : MonoBehaviour, ITrackableEventHandler 
{
	private TrackableBehaviour mTrackableBehaviour;
	private GameObject painting;
	public Renderer paintingRenderer; 



	void Start()
	{
		// Must be set before TrackableEventHandler is registered
		painting = GameObject.FindWithTag("Painting"); // Associated painting
		paintingRenderer = painting.GetComponent<Renderer>();

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
			paintingRenderer.enabled = true;
		}
		else
		{
			Debug.Log("Hiding image target");
			paintingRenderer.enabled = false;
		}
	}   
}
