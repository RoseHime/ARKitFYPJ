﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

    

    public LayerMask touchInputMask;    
    private RaycastHit hit;
    Vector3 v3_rayPointTarget;
    public bool b_TargetChose;
    public bool b_CheckFinger;

    //For Unit Selection
    private GameObject go_PlayerUnit;

    public bool b_BuildTower;
    public GameObject go_towerPrefab;

    private void Start()
    {
        b_TargetChose = false;
        b_BuildTower = false;
        b_CheckFinger = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!b_TargetChose && !b_CheckFinger)
        {
            if (Input.touchCount > 0)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;

                    //Check what the ray hit
                    if (recipient.tag == "PlayerUnit")
                    {
                        go_PlayerUnit = recipient;
                        go_PlayerUnit.GetComponent<PlayerUnitUpdate>().b_Selected = true;
                    }


                    Debug.Log(hit.point);

                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
        else if (b_TargetChose && !b_CheckFinger)
        {
            PickTargetPoint();
        }
    }

    public void PickTargetPoint()
    {
        if (Input.touchCount > 0)
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit, 100.0f, touchInputMask))
            {
                v3_rayPointTarget = hit.point;
                Debug.Log(hit.point);
                if (go_PlayerUnit.GetComponent<PlayerUnitUpdate>().b_Selected)
                {
                    go_PlayerUnit.GetComponent<PlayerUnitUpdate>().SetTargetPos(v3_rayPointTarget);
                    go_PlayerUnit.GetComponent<PlayerUnitUpdate>().b_Selected = false;
                }
                if (b_BuildTower)
                {
                    transform.GetComponent<BuildStructures>().BuildBuilding(go_towerPrefab, v3_rayPointTarget);
                    b_BuildTower = false;
                }
                b_TargetChose = false;
            }
        }
    }

    public Vector3 rayHitTarget()
    {
        return v3_rayPointTarget;
    }
}
