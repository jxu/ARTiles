using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour {


	void Update () 
	{
		gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
	}
}
