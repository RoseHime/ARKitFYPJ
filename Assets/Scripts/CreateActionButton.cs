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

    public GameObject go_selectedUnit;

    public GameObject go_buildPanel;
    public GameObject go_barracksPanel;
    // Use this for initialization
    void Start()
    {
        CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (go_selectedUnit != null)
        {
            if (go_selectedUnit.tag == "PlayerUnit")
            {
                PlayerUnitBehaviour plrUnit = go_selectedUnit.GetComponent<PlayerUnitBehaviour>();
                go_unitInfo.GetComponentInChildren<Text>().text = "HP:" + plrUnit.f_HealthPoint + "\nSPD:" + plrUnit.GetSpeed() + "\nRANGE:" + plrUnit.f_range;
            }
            else if (go_selectedUnit.tag == "SelectableBuilding")
            {
                BuildingInfo building = go_selectedUnit.GetComponent<BuildingInfo>();
                go_unitInfo.GetComponentInChildren<Text>().text = building.GetUnitsInfo();
            }
        }

        if (GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().go_SelectUnit() == null)
        {
            GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().Setfalse(false);
            gameObject.SetActive(false);
        }
    }

    public void CreateButtons()
    {
        foreach (Transform button in gameObject.transform)
        {
            if (button.gameObject.name == "ActionButton")
            {
                Destroy(button.gameObject);
            }
        }

        go_unitInfo_Length = go_unitInfo.GetComponent<RectTransform>().rect.width;
        go_actionButton_Length = go_actionButton.GetComponent<RectTransform>().rect.width;

        if (go_selectedUnit.tag == "PlayerUnit")
        {
            for (int i = 0; i < go_selectedUnit.GetComponent<PlayerUnitBehaviour>().getAmountOfButton(); ++i)
            {
                GameObject goButton = (GameObject)Instantiate(go_actionButton);
                goButton.name = "ActionButton";
                goButton.transform.SetParent(go_actionPanel.transform, false);
                goButton.transform.localScale = new Vector3(1, 1, 1);
                goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
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
                        case 5:
                            goButton.GetComponentInChildren<Text>().text = "BUILD";
                            break;



                    }
                }
            }
        }
        else if (go_selectedUnit.tag == "SelectableBuilding")
        {

            GameObject goButton = (GameObject)Instantiate(go_actionButton);
            goButton.name = "ActionButton";
            goButton.transform.SetParent(go_actionPanel.transform, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);
            goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;

            //goButton.transform.localPosition = Vector3.zero;
            goButton.transform.localPosition = new Vector3(go_unitInfo.transform.localPosition.x + (go_unitInfo_Length / 2) + (go_actionButton_Length / 2), go_unitInfo.transform.localPosition.y - 30, 0);
            goButton.GetComponentInChildren<Text>().text = "STOP";
            tempPos = goButton.transform.localPosition;

            if (go_selectedUnit.name == "Barracks")
            {
                GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                secondButton.name = "ActionButton";
                secondButton.transform.SetParent(go_actionPanel.transform, false);
                secondButton.transform.localScale = new Vector3(1, 1, 1);
                secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                secondButton.GetComponent<ChooseCommand>().go_BarracksPanel = go_barracksPanel;
                go_barracksPanel.GetComponent<BarracksPanelInfo>().go_SelectedBarracks = go_selectedUnit;

                //goButton.transform.localPosition = Vector3.zero;
                secondButton.transform.localPosition = new Vector3(tempPos.x + go_actionButton_Length * 1, go_unitInfo.transform.localPosition.y - 30, 0);
                secondButton.GetComponentInChildren<Text>().text = "CREATE";
                tempPos = secondButton.transform.localPosition;
            }
            else if (go_selectedUnit.GetComponent<TownHallBehaviour>() != null)
            {
                GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                secondButton.name = "UpgradeActionButton";
                secondButton.transform.SetParent(go_actionPanel.transform, false);
                secondButton.transform.localScale = new Vector3(1, 1, 1);
                secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;

                //goButton.transform.localPosition = Vector3.zero;
                secondButton.transform.localPosition = new Vector3(tempPos.x + go_actionButton_Length * 1, go_unitInfo.transform.localPosition.y - 30, 0);
                secondButton.GetComponentInChildren<Text>().text = "UPGRADE (" + GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().f_upgradeCost + ")";
                tempPos = secondButton.transform.localPosition;
            }
        }
    }

    public void DestroyButton()
    {
       
    }
}
