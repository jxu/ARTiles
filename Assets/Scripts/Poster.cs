using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poster : MonoBehaviour 
{
	public int spriteIndex = -1;
	public float realSizeWidth;
	public float realSizeHeight;


	void Start () 
	{
		ScaleReal();
	}

	public void ScaleReal()
	{
		// Scale sprite from arbitrary pixel size to Unity units (real world meters)
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		Sprite sp = renderer.sprite;
		float pixelsPerUnit = sp.pixelsPerUnit;

		float spUnitsWidth = sp.rect.width / pixelsPerUnit;
		float spUnitsHeight = sp.rect.height / pixelsPerUnit;

		//print ("Sprite width height (units): " + spUnitsWidth + " " + spUnitsHeight);

		// Compensate for parent's scale
		Vector3 parentScale = transform.parent.localScale;
		float transformWidth = (realSizeWidth / spUnitsWidth) / parentScale.x;
		float transformHeight = (realSizeHeight / spUnitsHeight) / parentScale.y;


		transform.localScale = new Vector3 (transformWidth, transformHeight, 
			transform.localScale.z);
	}
}
