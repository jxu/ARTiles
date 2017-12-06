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
    private RendererManager rm;
    private TransformManager tb;
    public Text vuMarkText;
    public Text selectionText;
    public float fireDelta = 0.05f;
    public float transformSpeedMod = 10.0f;
    public float rotationaSpeedMod = 0.0f;

    // Use this for initialization
    void Start ()
    {
        handler = GameObject.Find("VuMark").GetComponent<DefaultTrackableEventHandler>();
        selection = handler.GetVuMarkObj();
        Debug.Log("Selection set to " + (selection == null));
        SetVuMarkText();
        tb = null;
        rm = null;
    }

    // Update is called once per frame
    void Update ()
    {
        handler = GameObject.Find("VuMark").GetComponent<DefaultTrackableEventHandler>();

        float h = Input.GetAxis("Horizontal") * transformSpeedMod;
        float v = Input.GetAxis("Vertical") * transformSpeedMod;

        if (selection == null)
        {
            VuMarkTarget firstTarget = handler.GetVuMarkObj();
            if (firstTarget == null)
            {
                /* Nothing visible, so nothing to do
                 * Resets selection, and updates the text */
                SetVuMarkText();
                return;
            }
            else
            {
                selection = firstTarget;
                rm = VuMarkRenderer(selection);
                tb = VuMarkTransformer(selection);
                SetVuMarkText();
            }
        }
        

        //Debug.Log("BEFORE TRANSF: SELECT IS " + handler.FurnitureLookup(selection));
        //TransformManager tb = VuMarkTransformer(selection);
        //RendererManager rm = VuMarkRenderer(selection);

        if (Input.GetButton("Jump") && myTime > nextFire)
        {
            Debug.Log("Jumped");
            nextFire = myTime + fireDelta;
            ChangeSelection();
            SetVuMarkText();
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        else if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            Debug.Log("Moving");
            nextFire = myTime + fireDelta;
            
            tb.Move(v, h);
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        else if (Input.GetButton("Fire2"))
        {
            Debug.Log("Rotating");
            tb.Rotate(v * rotationaSpeedMod);
        }
        else if (Input.GetButton("Fire3") && myTime > nextFire)
        {
            Debug.Log("Changing Color");
            nextFire = myTime + fireDelta;
            if (rm.GetColor() != Color.black)
                rm.SetColor(Color.black);
            else
                rm.SetColor(Color.white);
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
        else
        {
            Debug.Log("Trying to move " + handler.FurnitureLookup(selection));
            Debug.Log("Verify: Trying to move " + tb.GetName());
            tb.Move(v, h);
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
        IEnumerable<VuMarkTarget> allTargsEnum = handler.GetVuMarkTargets();
        List<VuMarkTarget> allTargs = allTargsEnum.ToList<VuMarkTarget>();
        if (allTargs.Count() > 1)
            selection = allTargs.Find(x => x!=selection);
        else
        {
            VuMarkTarget tryTarget = handler.GetVuMarkObj();
            if (tryTarget != null)
                selection = tryTarget;
            else
                selection = null;

        }
        tb = VuMarkTransformer(selection);
        rm = VuMarkRenderer(selection);
        SetVuMarkText();
    }

    public RendererManager VuMarkRenderer(VuMarkTarget vu)
    {
        if (vu == null)
            return null;
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
        if (vu == null)
            return null;
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
