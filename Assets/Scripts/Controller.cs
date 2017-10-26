using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controller : MonoBehaviour {
	
	public Material spritesDefault;
	public Material grayscale;

	private int selectedTarget = 0;
	private string indexText;
	private Object[] allSprites;
	private GameObject[] allImageTargets;
	private int imageTargetsCount;

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
		allImageTargets = GameObject.FindGameObjectsWithTag ("ImageTarget");
		imageTargetsCount = allImageTargets.Length;

		foreach (GameObject it in allImageTargets) 
			Debug.Log("Found " + it);
	}

	// All sprites in Resources/Posters will be loaded 
	// in alphabetical order, for consistency
	void initAllSprites()
	{
		// Load sprite resources
		allSprites = Resources.LoadAll("Posters", typeof(Sprite));
		foreach(Object s in allSprites)
			Debug.Log("Found sprite " + s);

		allSprites = allSprites.OrderBy(s => s.ToString()).ToArray();
	}

	void handleInput()
	{
		Transform posterTransform = allImageTargets[selectedTarget].transform.GetChild(0);
		Poster poster = posterTransform.GetComponent<Poster>();


		// Cycle selection
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				selectedTarget = (selectedTarget - 1 + imageTargetsCount) % imageTargetsCount;

			if (Input.GetKeyDown(KeyCode.RightArrow))
				selectedTarget = (selectedTarget + 1) % imageTargetsCount;

			// TODO: inefficient, fix later
			indexText = "Target Index: " + selectedTarget.ToString();

			// TODO: Have initial selection highlighted
			for(int i = 0; i < imageTargetsCount; i++)
			{
				Transform thisPosterTransform = allImageTargets[i].transform.GetChild(0);
				Renderer renderer = thisPosterTransform.GetComponent<Renderer>();
				renderer.material = (i == selectedTarget) ? grayscale : spritesDefault;
			}
		}

		// Cycle poster choice
		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			int spriteCount = allSprites.Length;

			if (Input.GetKeyDown(KeyCode.UpArrow))
				poster.spriteIndex = (poster.spriteIndex + 1) % spriteCount;

			if (Input.GetKeyDown(KeyCode.DownArrow))
				poster.spriteIndex = (poster.spriteIndex - 1 + spriteCount) % spriteCount;

			// Replace sprite with sprite from index
			posterTransform.GetComponent<SpriteRenderer>().sprite = 
				(Sprite) allSprites[poster.spriteIndex];
			
			poster.ScaleReal();
		}

		// Change size of poster
		else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || 
			Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
		{
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

	// Immediate Mode GUI, for debugging (see GUI Scripting Guide)
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 20), indexText);
	}
}
