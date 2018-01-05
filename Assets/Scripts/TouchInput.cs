using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour {

    

    public LayerMask touchInputMask;    
    private RaycastHit hit;
    Vector3 v3_rayPointTarget;

    public bool b_TargetChose;
    public bool b_SomethingIsSelected;
    public bool b_Cancelled;
    public bool b_StopRun;

    //For Unit Selection
    //private GameObject go_PlayerUnit;
    //private GameObject go_PlayerBuilding;

    public GameObject go_SelectedUnit;

    public bool b_BuildTower;


    public Text debugText;

    private void Start()
    {
        b_TargetChose = false;
        b_BuildTower = false;
        b_SomethingIsSelected = false;
        b_Cancelled = false;
        b_StopRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_Cancelled) // When Action Button STOP is pressed.
        {
            b_SomethingIsSelected = false;
            if (go_SelectedUnit != null)
            {
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)           //If the selected is a unit
                    {
                        go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
                    }
                }
                //else if (go_PlayerBuilding.GetComponent<BuildingInfo>().b_Selected)
                //{
                //    go_PlayerBuilding.GetComponent<BuildingInfo>().b_Selected = false;
                //}
            }
            b_Cancelled = false;
        }

        //Touch Input
        if (Input.touches.Length > 0)
        {
            //debugText.text = "Touch Registered";
            if (!b_SomethingIsSelected)
            {
                Ray ray;
                ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
                debugText.text = "Ray Created";
                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    debugText.text = recipient.name + "\n" + hit.point;
                    if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
                    {
                        go_SelectedUnit = recipient;
                        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                        go_SelectedUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                        b_SomethingIsSelected = true;
                    }
                }
            }
            if (b_TargetChose && b_SomethingIsSelected)
            {
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                        PickTargetPoint();
                }
            }
        }
#if UNITY_EDITOR
        //Mouse Input
        if (Input.GetMouseButton(0))  // if there is a left click on mouse
        {
            //debugText.text = "Click Registered";         
            if (!b_SomethingIsSelected)
            {
                Ray ray;
                ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                debugText.text = "Ray Created";
                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    debugText.text = recipient.name + "\n" + hit.point;
                    if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
                    {
                        go_SelectedUnit = recipient;
                        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                        go_SelectedUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                        b_SomethingIsSelected = true;
                    }
                }
            }
            if (b_TargetChose && b_SomethingIsSelected)
            {
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                        PickTargetPoint();
                }
            }

        }
#endif
    }

    public void PickTargetPoint()
    {
        if (Input.touches.Length > 0) // Get the new touch
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
            debugText.text = "Touch Registered in PickTarget";
            if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
            {
                v3_rayPointTarget = hit.point;
                debugText.text = "Target:" + v3_rayPointTarget;
                GameObject go_ObjectHit = hit.transform.gameObject;
                Debug.Log(hit.point);
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                    {
                        if (go_ObjectHit.name == "StoneMine")
                        {
                            Debug.Log("Select Stone Mine");
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(v3_rayPointTarget, go_ObjectHit.name);
                        }
                        else if (go_ObjectHit.name == "Tree")
                        {
                            Debug.Log("Select Tree");
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(v3_rayPointTarget, go_ObjectHit.name);
                        }
                        else
                        {
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(v3_rayPointTarget);
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_buildBuilding = b_BuildTower;
                            b_BuildTower = false;
                        }
                    }
                }
                

                b_TargetChose = false;
            }
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButton(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            debugText.text = "Touch Registered in PickTarget";
            if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
            {
                v3_rayPointTarget = hit.point;
                debugText.text = "Target:" + v3_rayPointTarget;
                GameObject go_ObjectHit = hit.transform.gameObject;
                Debug.Log(hit.point);
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                    {
                        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getType() == PlayerUnitBehaviour.PlayerUnitType.PUN_WORKER)
                        {
                            if (go_ObjectHit.name == "StoneMine")
                            {
                                Debug.Log("Select Stone Mine");
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(go_ObjectHit.transform.position, go_ObjectHit.name);
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_toHarvestStone = true;
                            }
                            else if (go_ObjectHit.tag == "Tree")
                            {
                                Debug.Log("Select Tree");
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetBuildingTargetPos(v3_rayPointTarget, go_ObjectHit.name);
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_toHarvestTree = true;

                            }
                            else
                            {
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(v3_rayPointTarget);
                                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_buildBuilding = b_BuildTower;
                                b_BuildTower = false;
                            }
                        }
                        else if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getType() == PlayerUnitBehaviour.PlayerUnitType.PUN_MELEE ||
                                 go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getType() == PlayerUnitBehaviour.PlayerUnitType.PUN_RANGE ||
                                 go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getType() == PlayerUnitBehaviour.PlayerUnitType.PUN_TANK)

                        {
                            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(v3_rayPointTarget);
                            Debug.Log("Move");
                        }
                    }
                }
                b_TargetChose = false;
            }
        }
#endif
    }

    public Vector3 rayHitTarget()
    {
        return v3_rayPointTarget;
    }

    void CheckIfUnitIsSelected()
    {
        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
        {
            b_StopRun = true;
        }
    }
}
