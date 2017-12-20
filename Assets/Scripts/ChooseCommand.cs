using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Collections;

public class ChooseCommand : MonoBehaviour {

    //bool b_selectedCommand;
    public GameObject go_CommandButton;
    private GameObject go_CommandPanel;
    private GameObject go_MainCamera;
    private TouchInput ti;

    public GameObject go_BuildingPanel;

    //Testing use
    private string text;

    private void Start()
    {
        //b_selectedCommand = false;
        go_CommandPanel = go_CommandButton.transform.parent.gameObject;
        go_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ti = go_MainCamera.GetComponent<TouchInput>();
        text = go_CommandButton.GetComponentInChildren<Text>().text;
    }

    public void OnClickCommand()
    {

        if (go_CommandButton.GetComponentInChildren<Text>().text == "STOP")
        {
            go_CommandPanel.SetActive(false);
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
        {
            ti.b_TargetChose = true;
            ti.b_CheckFinger = true;
        }
        else if (go_CommandButton.GetComponentInChildren<Text>().text == "BUILD")
        {
            go_BuildingPanel.SetActive(true);
            go_CommandPanel.SetActive(false);
        }

    }

    public void OffClickCommand()
    {
        if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
        {
            ti.b_CheckFinger = false;
            go_CommandPanel.SetActive(false);
        }
    }

    private void Update()
    {
    }
}
