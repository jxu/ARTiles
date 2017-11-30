using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererManager : MonoBehaviour {
    /* This is initalized by dragging the children of the object onto
     * the public `renderers` variable in the GUI. I believe this is the proper way
     * of doing this in Unity */
    public Renderer[] renderers;

    public void SetState(bool aState)
    {
        foreach (var r in renderers)
            r.enabled = aState;
    }

    public void Disable()
    {
        SetState(false);
    }

    public void Enable()
    {
        SetState(true);
    }

    public bool Rendering()
    {
        return renderers[0].enabled;
    }

    public string getName()
    {
        return this.name;
    }

    public void SetColor(Color color)
    {
        foreach (var r in renderers)
        {
            r.material.SetColor("_Color", color);
        }
    }

    public Color GetColor()
    {
        return renderers[0].material.GetColor("_Color");
    }
}
