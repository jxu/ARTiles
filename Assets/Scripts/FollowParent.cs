using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should not be needed if model is child of image target

public class FollowParent : MonoBehaviour {
	void Update () 
	{
		gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
	}
}
