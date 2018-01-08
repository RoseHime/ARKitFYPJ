using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {

    public LayerMask touchInputMask;
    private RaycastHit hit;
    Vector3 v3_rayPointTarget;
    private string s_text;
    private Button btn;

    public Text debugText;
    private GameObject go_SelectedUnit;
    private bool b_SomethingIsSelected;
    public RawImage i_Crosshair;

    // Use this for initialization
    void Start () {
        btn = this.GetComponent<Button>();
        s_text = btn.GetComponentInChildren<Text>().text;
        btn.onClick.AddListener(TapDown);
        b_SomethingIsSelected = false;
    }
	
	// Update is called once per frame
	void Update () {
        s_text = btn.GetComponentInChildren<Text>().text;
        Debug.Log(b_SomethingIsSelected);
    }
    
    public void TapDown()
    {
        //If nothing is selected
        if (!b_SomethingIsSelected && s_text == "Select")
        {
            //Debug.Log("You clicked");
            Ray ray;
            ray = Camera.main.ScreenPointToRay(i_Crosshair.transform.position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
            {
                GameObject recipient = hit.transform.gameObject;
                debugText.text = recipient.name + "\n" + hit.point;
                if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
                {
                    go_SelectedUnit = recipient;
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                    {
                        go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                    }
                    go_SelectedUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                    b_SomethingIsSelected = true;
                    btn.GetComponentInChildren<Text>().text = "Back";
                }
            }
            //btn.interactable = false;
            
        }

        //If something is selected
        else if (b_SomethingIsSelected && s_text == "Back")
        {
            if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
            {
                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getCommand().SetActive(false);
            }
            b_SomethingIsSelected = false;
            btn.GetComponentInChildren<Text>().text = "Select";
            //btn.interactable = false;
        }


        
    }

    public Transform getCrossHair()
    {
        return i_Crosshair.transform;
    }

    public Vector3 getTargetPoint(Vector3 pos)
    {
        return v3_rayPointTarget = pos;
    }

    public GameObject go_SelectUnit()
    {
        return go_SelectedUnit;
    }

    public bool Setfalse(bool b)
    {
        if (b == false)
            b_SomethingIsSelected = false;
        return true;
    }
}
