using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poster : MonoBehaviour 
{
	public float realSizeWidth;
	public float realSizeHeight;


	void Start () 
	{
		// Scale sprite from arbitrary pixel size to Unity units
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		Sprite sp = renderer.sprite;
		float pixelsPerUnit = sp.pixelsPerUnit;

		float spUnitsWidth = sp.rect.width / pixelsPerUnit;
		float spUnitsHeight = sp.rect.height / pixelsPerUnit;

		print ("Sprite width height (units): " + spUnitsWidth + " " + spUnitsHeight);

		transform.localScale = new Vector3 (realSizeWidth / spUnitsWidth, 
			                          		realSizeHeight / spUnitsHeight, 
											transform.localScale.z);

	}
}
