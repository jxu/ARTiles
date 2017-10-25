using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public GameObject[] allImageTargets;
	public int imageTargetsCount;
	private int selectedTargetIndex = 0;
	private string indexText;

	void Start () {

	}

	void Update () 
	{
		initAllImageTargets();
		handleInput();

	}

	void initAllImageTargets()
	{
		if (allImageTargets.Length == 0)
		{
			allImageTargets = GameObject.FindGameObjectsWithTag ("ImageTarget");
			imageTargetsCount = allImageTargets.Length;

			foreach (GameObject it in allImageTargets) 
			{
				Debug.Log("Found " + it);
			}
		}
	}

	void handleInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedTargetIndex > 0)
			{
				--selectedTargetIndex;
			}

			if (Input.GetKeyDown(KeyCode.RightArrow) && selectedTargetIndex < imageTargetsCount-1)
			{
				++selectedTargetIndex;
			}

			// TODO: inefficient, fix later
			indexText = "Index: " + selectedTargetIndex.ToString();

		}

	}

	// Immediate Mode GUI, for debugging (see GUI Scripting Guide)
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 20), indexText);
	}
}
