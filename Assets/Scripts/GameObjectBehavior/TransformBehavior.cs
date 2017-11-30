using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBehavior : MonoBehaviour {
    private bool lockObject;
    private float nextFire = 0.5f;
    private float myTime = 0.0f;
    private RendererManager selectedObject = null;

    public float fireDelta = 0.5f;
    public float transformSpeedMod = 0.05f;
    public float rotationaSpeedMod = 10.0f;

    // Use this for initialization
    void Start () {
        lockObject = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        var rendererComponents = GetComponentsInChildren<RendererManager>(true);

        bool rotating = false;
        bool objectSpecified = (selectedObject != null);
        float h = Input.GetAxis("Horizontal") * transformSpeedMod;
        float v = Input.GetAxis("Vertical") * transformSpeedMod;

        /* Checks to see if Fire1 input has been made, which toggles the lock
         * on the transform of the object. There is a delta of .5 seconds to
         * to prevent the button from being toggled repeatedly */
        if (objectSpecified && Input.GetButton("Fire1") && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            toggleLock();
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        /* Checks to see if Fire2 input is currently being made
         * which allows for rotational movement using arrow keys */
        if (objectSpecified && Input.GetButton("Fire2"))
        {
            rotating = true;
            transform.Rotate(0.0f, v*rotationaSpeedMod, 0.0f);
            Debug.Log("Pressing fire2!");
        }
        /* Checks to see if Fire3, which for right now allows the selection
         * of colors */
        if (objectSpecified && Input.GetButton("Fire3") && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            //Color current = RendererManager.GetColor();
            Color current = Color.white;
            if (current == Color.white)
                current = Color.black;
            else
                current = Color.white;
            foreach (var r in rendererComponents)
            {
                r.SetColor(current);
            }
        }

        if (Input.GetButton("SelectorForward") && myTime > nextFire)
        {
            if (objectSpecified && selectedObject.isActiveAndEnabled)
            {
                //move to the next available object in the target manager
            }
        }
        

        myTime = myTime + Time.deltaTime;
        if (!lockStatus() && !rotating) 
        {
            transform.Translate(h, 0.0f, v);
        }
	}

    void rotate(float vspeed)
    {
        transform.Rotate(0.0f, vspeed * rotationaSpeedMod, 0.0f);
        Debug.Log("Pressing fire2!");
    }
    
    void toggleLock()
    {
        lockObject = !lockObject;
        Debug.Log("Object is now locked? " + lockObject.ToString());
    }

    bool lockStatus()
    {
        return lockObject;
    }
}
