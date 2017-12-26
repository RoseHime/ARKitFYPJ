using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

    

    public LayerMask touchInputMask;    
    private RaycastHit hit;
    Vector3 v3_rayPointTarget;

    public bool b_TargetChose;
    private bool b_SomethingIsSelected;
    public bool b_Cancelled;
    public bool b_StopRun;

    //For Unit Selection
    private GameObject go_PlayerUnit;
    private GameObject go_PlayerBuilding;

    public bool b_BuildTower;

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
            if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)           //If the selected is a unit
            {
                go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
            }
            //else if (go_PlayerBuilding.GetComponent<BuildingInfo>().b_Selected)
            //{
            //    go_PlayerBuilding.GetComponent<BuildingInfo>().b_Selected = false;
            //}
            b_Cancelled = false;
        }

        //Touch Input
        if (Input.touches.Length > 0)
        {
            if (!b_SomethingIsSelected)
            {
                Ray ray;
                ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    if (recipient.tag == "PlayerUnit")
                    {
                        go_PlayerUnit = recipient;
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                    }
                    else if (recipient.tag == "SelectableBuilding")
                    {
                        // I still havn't written anything here, probably will soon
                        //go_PlayerBuilding = recipient;
                        //go_PlayerBuilding.GetComponent<BuildingInfo>().b_Selected = true;
                    }
                    else
                    {

                    }
                    b_SomethingIsSelected = true;

                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            if (b_TargetChose && b_SomethingIsSelected)
            {
                if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                    PickTargetPoint();
            }
        }

        //Mouse Input
        if (Input.GetMouseButton(0))  // if there is a left click on mouse
        {
            if (!b_SomethingIsSelected)
            {
                Ray ray;
                ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    if (recipient.tag == "PlayerUnit")
                    {
                        go_PlayerUnit = recipient;
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                        go_PlayerUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    else if (recipient.tag == "SelectableBuilding")
                    {
                        // I still havn't written anything here, probably will soon
                        go_PlayerBuilding = recipient;
                        go_PlayerBuilding.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {

                    }
                    b_SomethingIsSelected = true;

                   //if (Input.GetMouseButtonDown(0))
                   //{
                   //    recipient.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                   //    Debug.Log("HERE");
                   //}
                   //if (Input.GetMouseButtonUp(0))
                   //{
                   //    recipient.SendMessage("OffClick", hit.point, SendMessageOptions.DontRequireReceiver);
                   //}
                }
            }
            if (b_TargetChose && b_SomethingIsSelected)
            {
                Debug.Log("HERE@");
                if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                    PickTargetPoint();
            }
        }
    }

    public void PickTargetPoint()
    {
        if (Input.touches.Length > 0) // Get the new touch
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit, 100.0f, touchInputMask))
            {
                v3_rayPointTarget = hit.point;
                GameObject go_ObjectHit = hit.transform.gameObject;
                Debug.Log(hit.point);
                if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                {
                    if (go_ObjectHit.name == "GoldMine")
                    {
                        Debug.Log("Select Gold Mine");
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_StartHarvest = true;
                    }
                    else
                    {
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(v3_rayPointTarget);
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_buildBuilding = b_BuildTower;
                        b_BuildTower = false;
                    }
                }

                b_TargetChose = false;
            }
        }

        else if (Input.GetMouseButton(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, touchInputMask))
            {
                v3_rayPointTarget = hit.point;
                GameObject go_ObjectHit = hit.transform.gameObject;
                Debug.Log(hit.point);
                if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                {
                    if (go_ObjectHit.name == "GoldMine")
                    {
                        Debug.Log("Select Gold Mine");
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_StartHarvest = true;
                    }
                    else
                    {
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(v3_rayPointTarget);
                        go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_buildBuilding = b_BuildTower;
                        b_BuildTower = false;
                    }
                }

                b_TargetChose = false;
            }
        }
    }

    public Vector3 rayHitTarget()
    {
        return v3_rayPointTarget;
    }

    void CheckIfUnitIsSelected()
    {
        if (go_PlayerUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
        {
            b_StopRun = true;
        }
    }
}
