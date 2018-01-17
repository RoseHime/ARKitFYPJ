﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateActionButton : MonoBehaviour
{

    public GameObject go_unitInfo;
    public GameObject go_actionButton;
    public GameObject go_actionPanel;
    private float go_actionButton_Length;
    Vector3 tempPos;

    public GameObject go_selectedUnit;

    public GameObject go_buildPanel;
    public GameObject go_barracksPanel;

    public GameObject go_selectButton;
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
                //PlayerUnitBehaviour plrUnit = go_selectedUnit.GetComponent<PlayerUnitBehaviour>();
                go_unitInfo.GetComponentInChildren<Text>().text = "HP:" + plrUnit.f_HealthPoint + "\nSPD:" + plrUnit.f_speed + "\nRANGE:" + plrUnit.f_range;
            }
            else if (go_selectedUnit.tag == "SelectableBuilding")
            {
                BuildingInfo building = go_selectedUnit.GetComponent<BuildingInfo>();
                go_unitInfo.GetComponentInChildren<Text>().text = building.GetUnitsInfo();
            }
        }

        //if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchInput>().enabled)
        //{
        //    if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchInput>().go_SelectedUnit == null)
        //    {
        //        gameObject.SetActive(false);
        //    }
        //}
        //else if (GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().go_SelectUnit() == null)
        //{
        //    GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().Setfalse(false);
        //    gameObject.SetActive(false);
        //}
    }

    public void CreateButtons()
    {
        foreach (Transform button in gameObject.transform)
        {
            if (button.gameObject.name == "ActionButton" || button.gameObject.name == "UpgradeActionButton"
                || button.gameObject.name == "UpgradeBarracksButton")
            {
                Destroy(button.gameObject);
            }
        }
        go_actionButton_Length = go_actionButton.GetComponent<RectTransform>().rect.height;

        if (go_selectedUnit.tag == "PlayerUnit")
        {
            for (int i = 0; i < go_selectedUnit.GetComponent<PlayerUnitBehaviour>().getAmountOfButton(); ++i)
            {
                GameObject goButton = (GameObject)Instantiate(go_actionButton);
                goButton.name = "ActionButton";
                goButton.transform.SetParent(go_actionPanel.transform, false);
                goButton.transform.localScale = new Vector3(1, 1, 1);
                goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                goButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                goButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length * (i), 0);
                switch (i)
                {
                    case 0:
                        goButton.GetComponentInChildren<Text>().text = "CLOSE";
                        break;
                    case 1:
                        goButton.GetComponentInChildren<Text>().text = "MOVE";
                        break;
                    case 2:
                        goButton.GetComponentInChildren<Text>().text = "UPRANK";
                        break;
                    case 3:
                        goButton.GetComponentInChildren<Text>().text = "BUILD";
                        break;
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
            goButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            goButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y, 0);
            goButton.GetComponentInChildren<Text>().text = "CLOSE";

            if (go_selectedUnit.name == "Barracks")
            {
                for (int j = 0;j < 2;++j)
                {
                    GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                    secondButton.name = "ActionButton";
                    secondButton.transform.SetParent(go_actionPanel.transform, false);
                    secondButton.transform.localScale = new Vector3(1, 1, 1);
                    secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                    secondButton.GetComponent<ChooseCommand>().go_BarracksPanel = go_barracksPanel;
                    go_barracksPanel.GetComponent<BarracksPanelInfo>().go_SelectedBarracks = go_selectedUnit;
                    secondButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                    secondButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);

                    //goButton.transform.localPosition = Vector3.zero;
                    secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length, 0);
                    secondButton.GetComponentInChildren<Text>().text = "CREATE";
                    if (j == 1)
                    {
                        secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length * 2, 0);
                        secondButton.name = "UpgradeBarracksButton";
                        secondButton.GetComponentInChildren<Text>().text = "UPGRADE (" + go_selectedUnit.GetComponent<BarracksBehaviour>().i_levelUpCost + ")";
                    }
                }

            }
            else if (go_selectedUnit.GetComponent<TownHallBehaviour>() != null && GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().i_playerLevel < 3)
            {
                GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                secondButton.name = "UpgradeActionButton";
                secondButton.transform.SetParent(go_actionPanel.transform, false);
                secondButton.transform.localScale = new Vector3(1, 1, 1);
                secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                secondButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                secondButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);

                //goButton.transform.localPosition = Vector3.zero;
                secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length, 0);
                secondButton.GetComponentInChildren<Text>().text = "UPGRADE (" + GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().f_upgradeCost + ")";
            }
        }
    }

    public void DestroyButton()
    {
       
    }
}
