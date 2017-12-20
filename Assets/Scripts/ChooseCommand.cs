using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Collections;

public class ChooseCommand : MonoBehaviour {

    bool b_selectedCommand;
    public GameObject go_CommandButton;
    private GameObject go_CommandPanel;
    private GameObject go_MainCamera;
    private TouchInput ti;

    //Testing use
    private string text;

    private void Start()
    {
        b_selectedCommand = false;
        go_CommandPanel = go_CommandButton.transform.parent.gameObject;
        go_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ti = go_MainCamera.GetComponent<TouchInput>();
        text = go_CommandButton.GetComponentInChildren<Text>().text;
    }

    public void OnClickCommand()
    {
        if (b_selectedCommand)
        {
            if (go_CommandButton.GetComponentInChildren<Text>().text == "STOP")
            {
                go_CommandPanel.SetActive(false);
            }
            else if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
            {
                go_CommandPanel.SetActive(false);
            }
        }
    }

    public void OffClickCommand()
    {
        if (b_selectedCommand)
        {
            if (go_CommandButton.GetComponentInChildren<Text>().text == "MOVE")
            {
                ti.b_TargetChose = true;
            }
        }
    }

    private void Update()
    {
        if (go_CommandPanel.activeSelf == true)
        {
            b_selectedCommand = true;
        }
        if (go_CommandPanel.activeSelf == false)
        {
            b_selectedCommand = false;
        }
    }
}
