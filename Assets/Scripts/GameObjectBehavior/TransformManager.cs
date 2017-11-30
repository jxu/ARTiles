using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    private bool lockObject;
    private Transform myTransform;

    // Use this for initialization
    void Start()
    {
        lockObject = false;
    }

    private void Awake()
    {
        myTransform = transform;
    }

    public string GetName()
    {
        return this.name;
    }

    public void Rotate(float vspeed)
    {
        if (!lockObject)
            myTransform.Rotate(0.0f, vspeed, 0.0f);
    }

    public void Move(float vspeed, float hspeed)
    {
        if (!lockObject)
            myTransform.Translate(hspeed, 0.0f, vspeed);
    }


    public void ToggleLock()
    {
        lockObject = !lockObject;
        Debug.Log("Object is now locked? " + lockObject.ToString());
    }

    public bool LockStatus()
    {
        return lockObject;
    }
}
