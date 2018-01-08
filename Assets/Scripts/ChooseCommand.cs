using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Collections;

public class ChooseCommand : MonoBehaviour {

    //bool b_selectedCommand;
    public GameObject go_CommandButton;
    private GameObject go_CommandPanel;
    private ButtonControl bc;
    private RaycastHit hit;

    public GameObject go_BuildingPanel;
    public GameObject go_BarracksPanel;

    //Testing use
    //private string text;

    private void Start()
    {
        //b_selectedCommand = false;
        go_CommandPanel = go_CommandButton.transform.parent.gameObject;
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();
        //go_CrossHair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    public void OnClickCommand()
    {
        if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
        {
            Debug.Log("OnClickMOVE");
            //go_CommandPanel.SetActive(false);
            Ray ray = Camera.main.ScreenPointToRay(bc.getCrossHair().position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, bc.touchInputMask))
            {
                GameObject go_ObjectHit = hit.transform.gameObject;
                if (go_ObjectHit.name == "StoneMine")
                {
                    Debug.Log("Select Stone Mine");
                    bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(hit.point, go_ObjectHit.name);
                    bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().b_toHarvestStone = true;

                }
                else
                {
                    Debug.Log("Walk here");
                    bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().SetTargetPos(hit.point);

                }
            }
        }
    }
    //public void OffClickCommand()
    //{
    //    if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
    //    {
    //        ti.b_TargetChose = true;
    //        go_CommandPanel.SetActive(false);
    //    }
    //    else if (go_CommandButton.GetComponentInChildren<Text>().text == "BUILD")
    //    {
    //        go_BuildingPanel.SetActive(true);
    //        go_CommandPanel.SetActive(false);
    //    }
    //    else if (go_CommandButton.GetComponentInChildren<Text>().text == "CREATE")
    //    {
    //        go_BarracksPanel.SetActive(true);
    //        go_CommandPanel.SetActive(false);
    //    }
    //}

    private void Update()
    {
    }
}
