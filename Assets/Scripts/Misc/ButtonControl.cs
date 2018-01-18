using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonControl : MonoBehaviour {

    public LayerMask touchInputMask;
    private RaycastHit hit;
    private Ray ray;
    Vector3 v3_rayPointTarget;
    private string s_text;
    private Button btn;

    public Text debugText;
    private GameObject go_SelectedUnit;
    private bool b_SomethingIsSelected;
    public RawImage i_Crosshair;
    public GameObject go_TargetBox;
    private GameObject recipient;
    List<Transform> listOfUnit = new List<Transform>();

    public GameObject go_buildPanel;
    public GameObject go_barracksPanel;

    public bool b_ToBuild;
    public bool b_BuildTower;

    private Camera ARCamera;

    // Use this for initialization
    void Start () {
        btn = this.GetComponent<Button>();
        s_text = btn.GetComponentInChildren<Text>().text;
        btn.onClick.AddListener(TapDown);
        b_SomethingIsSelected = false;
        b_ToBuild = false;
        recipient = null;

        ARCamera = GameObject.FindGameObjectWithTag("PlaneDetection").GetComponent<UnityARCameraManager>().m_camera;
    }

    // Update is called once per frame
    void Update()
    {
        s_text = btn.GetComponentInChildren<Text>().text;
        //Debug.Log(b_SomethingIsSelected);
        //ray = Camera.main.ScreenPointToRay(i_Crosshair.transform.position);
        ray = ARCamera.ScreenPointToRay(i_Crosshair.transform.position);
        if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
        {
            recipient = hit.collider.transform.gameObject;
            go_TargetBox.transform.position = hit.point;
            //debugText.text = "Camera Pos:" + ARCamera.transform.position + "\n" +
            //                 "Camera dir:" + ARCamera.transform.forward + "\n" +
            //                 "Ray dir:" + ray.direction + "\n" +
            //                 "HIt pos:" + hit.point + "\n";
        }
    }
    
    public void TapDown()
    {
        if (!b_SomethingIsSelected && recipient != null && s_text == "Select")
        {
            if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
            {
                go_SelectedUnit = recipient.transform.gameObject;
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
                {
                    go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
                    //go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().CreateCommand();
                }
                go_SelectedUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
                b_SomethingIsSelected = true;
                btn.GetComponentInChildren<Text>().text = "Back";
                if (go_SelectedUnit.tag == "PlayerUnit")
                {
                    listOfUnit.Add(go_SelectedUnit.transform);
                }
            }
        }

        else if (b_SomethingIsSelected && recipient != null && s_text == "Back")
        {
            if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
            {
                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
                {
                    go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
                    go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getCommand().SetActive(false);
                }
            }
            else if (recipient.tag == "SelectableBuilding")
            {
                if (GameObject.FindGameObjectWithTag("Command"))
                    GameObject.FindGameObjectWithTag("Command").SetActive(false);
            }
            b_SomethingIsSelected = false;
            btn.GetComponentInChildren<Text>().text = "Select";

            //btn.interactable = false;
            go_barracksPanel.SetActive(false);
            go_buildPanel.SetActive(false);
            //go_barracksPanel.GetComponentInParent<GameObject>().SetActive(false);
        }

            ////If nothing is selected
            //if (!b_SomethingIsSelected && s_text == "Select" && !b_ToBuild)
            //{
            //    //Debug.Log("You clicked");
            //    Ray ray;
            //    ray = Camera.main.ScreenPointToRay(i_Crosshair.transform.position);
            //    if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
            //    {
            //        //go_TargetBox.transform.position = hit.point;
            //        //recipient = go_TargetBox.GetComponent<DetectCollision>().getGO();
            //        recipient = hit.transform.gameObject;

            //        //debugText.text = recipient.name + "\n" + hit.point;
            //        if (recipient != null)
            //        {
            //            if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
            //            {
            //                go_SelectedUnit = recipient.gameObject;
            //                if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
            //                {
            //                    go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = true;
            //                }
            //                go_SelectedUnit.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
            //                b_SomethingIsSelected = true;
            //                btn.GetComponentInChildren<Text>().text = "Back";
            //            }
            //        }
            //    }
            //    //btn.interactable = false;           
            //}

            ////If something is selected
            //else if (b_SomethingIsSelected && s_text == "Back" && !b_ToBuild)
            //{
            //    if (recipient != null)
            //    {
            //        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>() != null)
            //        {
            //            if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
            //            {
            //                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
            //                go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().getCommand().SetActive(false);
            //            }
            //        }
            //        else if (recipient.tag == "SelectableBuilding")
            //        {
            //            if (GameObject.FindGameObjectWithTag("Command"))
            //                GameObject.FindGameObjectWithTag("Command").SetActive(false);
            //        }
            //    }
            //    b_SomethingIsSelected = false;
            //    btn.GetComponentInChildren<Text>().text = "Select";

            //    //btn.interactable = false;
            //    go_barracksPanel.SetActive(false);
            //    go_buildPanel.SetActive(false);
            //    //go_barracksPanel.GetComponentInParent<GameObject>().SetActive(false);
            //}

            //else if (b_ToBuild && s_text == "Select")
            //{
            //    Ray ray;
            //    ray = Camera.main.ScreenPointToRay(i_Crosshair.transform.position);
            //    if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
            //    {
            //        if (go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected)
            //        {
            //            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(hit.point);
            //            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_buildBuilding = true;
            //            b_ToBuild = false;
            //            go_SelectedUnit.GetComponent<PlayerUnitBehaviour>().b_Selected = false;
            //            b_SomethingIsSelected = false;
            //        }
            //    }
            //}


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

    public void SetBackToSelect()
    {
        //b_SomethingIsSelected = false;
        btn.GetComponentInChildren<Text>().text = "Select";
    }

    public Button getButton()
    {
        return btn;
    }

    public GameObject GetRecipient()
    {
        return recipient;
    }

    public List<Transform> GetListOfUnit()
    {
        return listOfUnit;
    }

    //void raycastToNavmesh(Vector3 v3_pos)
    //{
    //    Ray ray;
    //    ray = Camera.main.ScreenPointToRay(i_Crosshair.transform.position);
    //    if (Physics.Raycast(ray.origin, ray.direction, touchInputMask))
    //    {
    //        NavMeshHit navmeshHit;
    //        if (NavMesh.SamplePosition(hit.point, out navmeshHit, 1, 1))
    //        {
    //            
    //        }
    //    }
    //}
}
