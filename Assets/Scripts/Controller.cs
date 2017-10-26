using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controller : MonoBehaviour {

	public static GameObject[] allImageTargets;
	public static int imageTargetsCount;

	public Material spritesDefault;
	public Material grayscale;

	private int selectedTarget = 0;
	private string indexText;
	private Object[] allSprites;


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
		allSprites = Resources.LoadAll("Posters", typeof(Sprite));
		foreach(Object s in allSprites)
			Debug.Log("Found sprite " + s);

		allSprites = allSprites.OrderBy(s => s.ToString()).ToArray();
	}

	void handleInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedTarget > 0)
				selectedTarget--;

			if (Input.GetKeyDown(KeyCode.RightArrow) && selectedTarget < imageTargetsCount-1)
				selectedTarget++;

			// TODO: inefficient, fix later
			indexText = "Target Index: " + selectedTarget.ToString();

			for(int i = 0; i < imageTargetsCount; i++)
			{
				Transform painting = allImageTargets[i].transform.GetChild(0);
				Renderer renderer = painting.GetComponent<Renderer>();
				renderer.material = (i == selectedTarget) ? grayscale : spritesDefault;
			}
		}

		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			Transform painting = allImageTargets[selectedTarget].transform.GetChild(0);
			Poster poster = painting.GetComponent<Poster>();
			int spriteCount = allSprites.Length;

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				poster.spriteIndex = (poster.spriteIndex + 1) % spriteCount;
			}

			// TODO: Down arrow

			// Replace sprite with sprite from index
			painting.GetComponent<SpriteRenderer>().sprite = (Sprite) allSprites[poster.spriteIndex];
			poster.ScaleReal();
		}

	}

	// Immediate Mode GUI, for debugging (see GUI Scripting Guide)
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 20), indexText);
	}
}
