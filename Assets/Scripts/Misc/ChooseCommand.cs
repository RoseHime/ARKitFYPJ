﻿using System.Collections;
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

    private GameObject go_DebugPurpose;

    //Testing use
    //private string text;

    private void Start()
    {
        //b_selectedCommand = false;
        go_CommandPanel = go_CommandButton.transform.parent.gameObject;
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();
        //go_CrossHair = GameObject.FindGameObjectWithTag("Crosshair");

        //go_DebugPurpose = GameObject.FindGameObjectWithTag("DebugPurpose").transform.GetChild(0).gameObject;
    }

    public void OnClickCommand()
    {
        //TestInput input = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestInput>();
        if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
        {
            Debug.Log("OnClickMOVE");
            //go_CommandPanel.SetActive(false);
            Ray ray = Camera.main.ScreenPointToRay(bc.getCrossHair().position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, bc.touchInputMask))
            {
                GameObject go_ObjectHit = hit.transform.gameObject;
                //go_DebugPurpose.GetComponent<Text>().text = "point location: " + hit.point;
                if (bc.GetListOfUnit().Count < 2)
                {
                    if (go_ObjectHit.tag == "StoneMine")
                    {
                        Debug.Log("Select Stone Mine");
                        bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(hit.point, go_ObjectHit.name);
                        bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().b_toHarvestStone = true;
                    }
                    else if (go_ObjectHit.tag == "Tree")
                    {
                        Debug.Log("Select Tree");
                        bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(hit.point, go_ObjectHit.name);
                        bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().b_toHarvestTree = true;
                    }
                    else if (go_ObjectHit.GetComponentInParent<Transform>().tag == "EnemyList" || go_ObjectHit.GetComponentInParent<Transform>().tag == "EnemyBuildingList")
                    {
                        Debug.Log("Attack enemy");
                    }
                    else
                    {
                        Debug.Log("Walk here");
                        //TestInput input = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestInput>();
                        // input.b_MoveUnit = true;
                        bc.go_SelectUnit().GetComponent<PlayerUnitBehaviour>().SetTargetPos(hit.point);
                    }
                }
                else if (bc.GetListOfUnit().Count > 1)
                {
                    for(int i = 0;  i < bc.GetListOfUnit().Count; i++)
                    {
                        bc.GetListOfUnit()[i].GetComponent<PlayerUnitBehaviour>().SetTargetPos(hit.point);
                    }
                }
            }

            //input.b_MoveUnit = true;
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "BUILD")
        {
            bc.b_ToBuild = true;
            bc.getButton().GetComponentInChildren<Text>().text = "Select";
            go_BuildingPanel.SetActive(true);
            go_CommandPanel.SetActive(false);
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "CREATE")
        {
            go_BarracksPanel.GetComponent<BarracksPanelInfo>().go_SelectedBarracks = go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit;
            go_BarracksPanel.SetActive(true);
            go_CommandPanel.SetActive(false);
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "CLOSE") // No longer needed
        {
            //input.selectedUnit = null;
            //go_BarracksPanel.SetActive(true);
            //go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit = null;
            //go_CommandPanel.SetActive(false);

            //for (int i = 1; i <= bc.GetListOfUnit().Count; i++)
            //{
            //    bc.GetListOfUnit()[i].GetComponent<PlayerUnitBehaviour>().b_Selected = false;
            //    bc.GetListOfUnit().Remove(bc.GetListOfUnit()[i]);
            //}
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "SELECTMORE")
        {
           if (bc.GetRecipient().tag == "PlayerUnit")
            {
                int i;
                for(i = 0; i < bc.GetListOfUnit().Count; i++)
                {
                    if (bc.GetRecipient().gameObject != bc.GetListOfUnit()[i].gameObject)
                    {
                        if (!bc.GetRecipient().GetComponent<PlayerUnitBehaviour>().b_Selected)
                            bc.GetListOfUnit().Add(bc.GetRecipient().transform);
                        bc.GetRecipient().GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                    }
                }
            }
        }
        else if (go_CommandButton.name == "UpgradeActionButton")
        {
            if (GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().LevelUp())
            {
                go_CommandPanel.SetActive(false);
                bc.SetBackToSelect();
            }
        }
        else if (go_CommandButton.name == "UpgradeBarracksButton")
        {
            if (go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit.GetComponent<BarracksBehaviour>().LevelUp())
            {
                go_CommandPanel.SetActive(false);
                bc.SetBackToSelect();
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
