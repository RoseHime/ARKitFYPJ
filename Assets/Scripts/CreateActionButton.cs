using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateActionButton : MonoBehaviour
{

    public GameObject go_unitInfo;
    public GameObject go_actionButton;
    public GameObject go_actionPanel;
    private float go_unitInfo_Length;
    private float go_actionButton_Length;
    Vector3 tempPos;
    // Use this for initialization
    void Start()
    {

        go_unitInfo_Length = go_unitInfo.GetComponent<RectTransform>().rect.width;
        go_actionButton_Length = go_actionButton.GetComponent<RectTransform>().rect.width;

        if (go_actionPanel.activeSelf == true)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject goButton = (GameObject)Instantiate(go_actionButton);
                goButton.transform.SetParent(go_actionPanel.transform, false);
                goButton.transform.localScale = new Vector3(1, 1, 1);
                if (i == 0)
                {
                    //goButton.transform.localPosition = Vector3.zero;
                    goButton.transform.localPosition = new Vector3(go_unitInfo.transform.localPosition.x + (go_unitInfo_Length / 2) + (go_actionButton_Length / 2), go_unitInfo.transform.localPosition.y - 30, 0);
                    goButton.GetComponentInChildren<Text>().text = "STOP";
                    tempPos = goButton.transform.localPosition;
                }
                else
                {
                    goButton.transform.localPosition = new Vector3(tempPos.x + go_actionButton_Length * i, go_unitInfo.transform.localPosition.y - 30, 0);
                    switch (i)
                    {
                        case 1:
                            goButton.GetComponentInChildren<Text>().text = "MOVE";
                            break;
                        case 2:
                            goButton.GetComponentInChildren<Text>().text = "ATK";
                            break;
                        case 3:
                            goButton.GetComponentInChildren<Text>().text = "DEF";
                            break;
                        case 4:
                            goButton.GetComponentInChildren<Text>().text = "UPRANK";
                            break;



                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
