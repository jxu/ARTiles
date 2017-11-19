// TODO: Better selection interface (bounding box?)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controller : MonoBehaviour {
	
	public Material spritesDefault;
	//public Material highlightMaterial;

	int selectedTarget = 0;
	string indexText;
	List<Object> allSprites;
	List<GameObject> allImageTargets;
	int imageTargetsCount;

	const float realHeightDelta = 0.1f;
	const float realWidthDelta = 0.1f;
	const float minPosterLength = 0.01f;  // Can't make poster smaller than this


	void Start () 
	{
		initAllSprites();
		initAllImageTargets();
	}

	void Update () 
	{
		handleInput();
	}
		
	// Warning: Image targets added out of order!
	void initAllImageTargets()
	{
		allImageTargets = GameObject.FindGameObjectsWithTag("ImageTarget").ToList();
		imageTargetsCount = allImageTargets.Count;

		foreach (GameObject it in allImageTargets) 
			Debug.Log("Found ImageTarget " + it);
	}

	void initAllSprites()
	{
		// All sprites in Resources/Posters will be loaded in alphabetical order, for consistency
		allSprites = Resources.LoadAll("Posters", typeof(Sprite)).ToList();
		foreach(Object s in allSprites)
			Debug.Log("Found sprite " + s);

		allSprites = allSprites.OrderBy(s => s.ToString()).ToList();
	}

	void handleInput()
	{
		GameObject obj = getTargetChild(selectedTarget);

		// Cycle selection with left/right
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				selectedTarget = (selectedTarget - 1 + imageTargetsCount) % imageTargetsCount;

			else if (Input.GetKeyDown(KeyCode.RightArrow))
				selectedTarget = (selectedTarget + 1) % imageTargetsCount;

			obj = getTargetChild(selectedTarget);

			// TODO: inefficient, fix later
			indexText = "Target Index: " + selectedTarget.ToString();

			// TODO: Have initial selection highlighted
			// Update highlight current selection in grayscale
			/*
			for(int i = 0; i < imageTargetsCount; i++)
			{
				Renderer renderer = obj.GetComponent<Renderer>();
				renderer.material = (i == selectedTarget) ? highlightMaterial : spritesDefault;
			}*/

		}

		// Cycle poster choice
		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			if (obj.CompareTag("Poster"))
			{
				Transform posterTransform = obj.transform;
				Poster poster = posterTransform.GetComponent<Poster>();
				
				int spriteCount = allSprites.Count;

				if (Input.GetKeyDown(KeyCode.UpArrow))
					poster.spriteIndex = (poster.spriteIndex + 1) % spriteCount;

				if (Input.GetKeyDown(KeyCode.DownArrow))
					poster.spriteIndex = (poster.spriteIndex - 1 + spriteCount) % spriteCount;

				// Replace sprite with sprite from index
				posterTransform.GetComponent<SpriteRenderer>().sprite = 
					(Sprite) allSprites[poster.spriteIndex];

				poster.ScaleReal();
			}
		}

		// Change size of poster with WASD
		else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || 
			Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
		{
			if (obj.CompareTag("Poster"))
			{
				Poster poster = obj.GetComponent<Poster>();

				if (Input.GetKeyDown(KeyCode.W))
					poster.realHeight += realHeightDelta;

				if (Input.GetKeyDown(KeyCode.S) && 
					poster.realHeight - realHeightDelta > minPosterLength)
					poster.realHeight -= realHeightDelta;

				if (Input.GetKeyDown(KeyCode.D))
					poster.realWidth += realWidthDelta;

				if (Input.GetKeyDown(KeyCode.A) && 
					poster.realWidth - realWidthDelta > minPosterLength)
					poster.realWidth -= realWidthDelta;

				poster.ScaleReal();
			}
		}

	}


	GameObject getTargetChild(int ind)
	{
		return allImageTargets[ind].transform.GetChild(0).gameObject;
	}
		

	// Immediate Mode GUI, for debugging (see GUI Scripting Guide)
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 20), indexText);
	}
}
