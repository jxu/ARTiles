using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.Linq;

public class ObjectTransformer : MonoBehaviour {
    private DefaultTrackableEventHandler handler;
    private VuMarkTarget selection;
    private float nextFire = 0.5f;
    private float myTime = 0.0f;

    public Text vuMarkText;
    public Text selectionText;
    public float fireDelta = 0.05f;
    public float transformSpeedMod = 10000f;
    public float rotationaSpeedMod = 10000.0f;

    // Use this for initialization
    void Start ()
    {
        handler = GameObject.Find("VuMark").GetComponent<DefaultTrackableEventHandler>();
        selection = handler.GetVuMarkObj();
        Debug.Log("Selection set to " + (selection == null));
        SetVuMarkText();
    }

    // Update is called once per frame
    void Update ()
    {
        float h = Input.GetAxis("Horizontal") * transformSpeedMod;
        float v = Input.GetAxis("Vertical") * transformSpeedMod;


        VuMarkTarget firstTarget = handler.GetVuMarkObj();
        if (firstTarget == null)
        {
            /* Nothing visible, so nothing to do
             * Resets selection, and updates the text */
            selection = null;
            SetVuMarkText();
            return;
        }
        else if (selection == null)
        {
            selection = firstTarget;
            SetVuMarkText();
        }


        Debug.Log("BEFORE TRANSF: SELECT IS " + handler.FurnitureLookup(selection));
        TransformManager tb = VuMarkTransformer(selection);
        RendererManager rm = VuMarkRenderer(selection);

        if (Input.GetButton("Jump") && myTime > nextFire)
        {
            Debug.Log("Jumped");
            nextFire = myTime + fireDelta;
            ChangeSelection();
            SetVuMarkText();
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            Debug.Log("Moving");
            nextFire = myTime + fireDelta;
            tb.Move(v, h);
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        if (Input.GetButton("Fire2") && myTime > nextFire)
        {
            Debug.Log("Rotating");
            nextFire = myTime + fireDelta;
            tb.Rotate(v * rotationaSpeedMod);
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        if (Input.GetButton("Fire3") && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            if (rm.GetColor() != Color.black)
                rm.SetColor(Color.black);
            else
                rm.SetColor(Color.white);
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        myTime = myTime + Time.deltaTime;
    }

    void SetVuMarkText()
    {
        if (selection == null)
            vuMarkText.text = "None";
        else
            vuMarkText.text = "ID from Handler:" + handler.FurnitureLookup(selection);
    }

    void ChangeSelection()
    {
        if (selection == null)
        {
            selection = handler.GetVuMarkObj();
        }
        IEnumerable<VuMarkTarget> allTargsEnum = handler.GetVuMarkTargets();
        List<VuMarkTarget> allTargs = allTargsEnum.ToList<VuMarkTarget>();
        Debug.Log("Selection before = " + handler.FurnitureLookup(selection));
        if (allTargs.Count() > 1)
            selection = allTargs.Find(x => x!=selection);
        Debug.Log("Selection after = " + handler.FurnitureLookup(selection)); 

    }

    public RendererManager VuMarkRenderer(VuMarkTarget vu)
    {
        string vuMarkName = handler.FurnitureLookup(vu);
        foreach (var component in handler.GetComponentsInChildren<RendererManager>(true))
        {
            if (vuMarkName.CompareTo(component.getName()) == 0)
            {
                return component;
            }
        }
        return null;
    }

    public TransformManager VuMarkTransformer(VuMarkTarget vu)
    {
        string vuMarkName = handler.FurnitureLookup(vu);
        foreach (var component in handler.GetComponentsInChildren<TransformManager>(true))
        {
            if (vuMarkName.CompareTo(component.GetName()) == 0)
            {
                return component;
            }
        }
        return null;
    }


}
