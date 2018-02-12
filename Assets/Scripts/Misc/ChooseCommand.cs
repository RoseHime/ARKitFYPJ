using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Collections;

public class ChooseCommand : MonoBehaviour {

    //bool b_selectedCommand;
    public GameObject go_CommandButton;
    private GameObject go_CommandPanel;
    private ButtonControl bc;
    private RaycastHit hit;

    private bool b_OnHold;

    public GameObject go_BuildingPanel;
    public GameObject go_BarracksPanel;

    private GameObject go_DebugPurpose;
    public GameObject go_ConfirmUpgrade;
    public GameObject go_ConfirmWorker;

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
                    if (go_ObjectHit.tag == "StoneMine" || go_ObjectHit.tag == "Tree")
                    {
                        bc.go_SelectUnit().GetComponent<PlayerFSM>().GetBuildingTargetPos(go_ObjectHit.transform);
                    }
                    else if ((go_ObjectHit.tag == "Enemy" || go_ObjectHit.transform.parent.tag == "EnemyBuildingList") &&
                             bc.go_SelectUnit().GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                    {
                        Debug.Log("Attack enemy");
                        bc.go_SelectUnit().GetComponent<PlayerFSM>().GetEnemyTargetPos(go_ObjectHit);
                    }
                    else
                    {
                        Debug.Log("Walk here");
                        //TestInput input = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestInput>();
                        // input.b_MoveUnit = true;
                        bc.go_SelectUnit().GetComponent<PlayerFSM>().SetTargetPos(hit.point);
                    }
                }
                else if (bc.GetListOfUnit().Count > 1)
                {
                    for(int i = 0;  i < bc.GetListOfUnit().Count; i++)
                    {
                        if (bc.GetListOfUnit()[i].GetComponent<PlayerUnitInfo>().GetUnitType() == PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                        {
                            if (go_ObjectHit.tag == "StoneMine" || go_ObjectHit.tag == "Tree")
                            {
                                bc.GetListOfUnit()[i].GetComponent<PlayerFSM>().GetBuildingTargetPos(go_ObjectHit.transform);
                            }
                            else
                            {
                                bc.GetListOfUnit()[i].GetComponent<PlayerFSM>().SetTargetPos(hit.point);
                            }
                        }
                        else
                        {
                            if ((go_ObjectHit.tag == "Enemy" || go_ObjectHit.transform.parent.tag == "EnemyBuildingList"))
                            {
                                Debug.Log("Attack enemy");
                                bc.GetListOfUnit()[i].GetComponent<PlayerFSM>().GetEnemyTargetPos(go_ObjectHit);
                            }
                            else
                            {
                                bc.GetListOfUnit()[i].GetComponent<PlayerFSM>().SetTargetPos(hit.point);
                            }
                        }
                    }
                }
            }

            //input.b_MoveUnit = true;
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "BUILD")
        {
            //bc.b_ToBuild = true;
            //bc.getButton().GetComponentInChildren<Text>().text = "Select";
            go_BuildingPanel.SetActive(true);
            go_CommandPanel.SetActive(false);
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "PLACE")
        {
            Ray ray = Camera.main.ScreenPointToRay(bc.getCrossHair().position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, bc.touchInputMask))
            {
                if (hit.transform == GameObject.FindGameObjectWithTag("Terrain").transform)
                {
                    bc.go_SelectUnit().GetComponent<PlayerFSM>().SetTargetPos(hit.point);

                    bc.go_SelectUnit().GetComponent<PlayerFSM>().b_buildBuilding = true;

                    //go_CommandPanel.SetActive(false);
                    //bc.SetBackToSelect();
                    go_CommandPanel.GetComponent<CreateActionButton>().CreateButtons();
                }
            }

        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "CREATE")
        {
            go_BarracksPanel.GetComponent<BarracksPanelInfo>().go_SelectedBarracks = go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit;
            go_BarracksPanel.SetActive(true);
            //go_CommandPanel.SetActive(false);
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "CLOSE") // No longer needed
        {
            //input.selectedUnit = null;
            //go_BarracksPanel.SetActive(true);
            //go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit = null;
            //go_CommandPanel.SetActive(false);

            //for (int i = 1; i <= bc.GetListOfUnit().Count; i++)
            //{
            //    bc.GetListOfUnit().Remove(bc.GetListOfUnit()[i]);
            //}
        }
        else if (go_CommandButton.name == "UpgradeActionButton")
        {
            //go_CommandPanel.SetActive(false);
            go_ConfirmUpgrade.GetComponent<UpgradeBuilding>().building = go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit;
            go_ConfirmUpgrade.SetActive(true);           
        }
        else if (go_CommandButton.name == "UpgradeBarracksButton")
        {
            //go_CommandPanel.SetActive(false);
            go_ConfirmUpgrade.GetComponent<UpgradeBuilding>().building = go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit;
            go_ConfirmUpgrade.SetActive(true);           
        }
        else if (go_CommandButton.name == "WorkerActionButton")
        {
            go_ConfirmWorker.GetComponent<CreateWorker>().building = go_CommandPanel.GetComponent<CreateActionButton>().go_selectedUnit;
            go_ConfirmWorker.SetActive(true);
        }
    }

    public void OnHoldCommand()
    {
        if (go_CommandButton.GetComponentInChildren<Text>().text == "SELECTMORE")
        {
            b_OnHold = true;
        }
    }

    public void OffHoldCommand()
    {
        if (go_CommandButton.GetComponentInChildren<Text>().text == "SELECTMORE")
        {
            b_OnHold = false;
            go_CommandPanel.GetComponent<CreateActionButton>().CreateButtons();
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
        if (b_OnHold)
        {
            if (bc.GetRecipient().tag == "PlayerUnit")
            {
                int i;
                for (i = 0; i < bc.GetListOfUnit().Count; i++)
                {
                    if (bc.GetRecipient().gameObject != bc.GetListOfUnit()[i].gameObject)
                    {
                        if (!bc.GetRecipient().GetComponent<PlayerFSM>().b_Selected)
                        {
                            //bc.GetRecipient().GetComponentInChildren<Transform>().Find("Plane").gameObject.SetActive(true);
                            bc.GetRecipient().GetComponent<PlayerFSM>().b_Selected = true;
                            bc.GetListOfUnit().Add(bc.GetRecipient().transform);
                        }
                    }
                }

            }
        }
    }
}
