using System.Collections;
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
    public ButtonControl bc;

    public GameObject go_selectedUnit;

    public GameObject go_buildPanel;
    public GameObject go_barracksPanel;

    public GameObject go_selectButton;

    Transform UnitCamera;

    public Sprite selectImage;
    public Sprite cancelImage;
    public Sprite uprankImage;
    public Sprite buildImage;
    public Sprite moveImage;
    public Sprite selectMoreImage;
    public Sprite createUnitImage;
    public Sprite createWorkerImage;

    public GameObject go_ConfirmButton;
    public GameObject go_ConfirmWorker;
    // Use this for initialization
    void Start()
    {
        go_actionButton_Length = go_actionButton.GetComponent<RectTransform>().rect.height;
        CreateButtons();
        UnitCamera = GameObject.FindGameObjectWithTag("UnitCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (go_selectedUnit != null)
        {
            if (go_selectedUnit.tag == "PlayerUnit")
            {
                PlayerUnitInfo plrUnit = go_selectedUnit.GetComponent<PlayerUnitInfo>();
                //PlayerUnitBehaviour plrUnit = go_selectedUnit.GetComponent<PlayerUnitBehaviour>();
                //go_unitInfo.GetComponentInChildren<Text>().text = "HP:" + plrUnit.f_HealthPoint + "\nSPD:" + plrUnit.f_speed + "\nRANGE:" + plrUnit.f_range;
                go_unitInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + plrUnit.GetUnitAttackDmg();
                go_unitInfo.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + plrUnit.GetUnitHealth();
                go_unitInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = plrUnit.transform.name;
                UnitCamera.position = go_selectedUnit.transform.position + (go_selectedUnit.transform.forward * 0.04f);
                UnitCamera.LookAt(go_selectedUnit.transform);
                UnitCamera.position += new Vector3(0, 0.03f, 0);
            }
            else if (go_selectedUnit.tag == "SelectableBuilding")
            {
                BuildingInfo building = go_selectedUnit.GetComponent<BuildingInfo>();
                //go_unitInfo.GetComponentInChildren<Text>().text = building.GetUnitsInfo();
                if (building.transform.GetComponent<TowerBehaviour>() != null)
                {
                    go_unitInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + building.transform.GetComponent<TowerBehaviour>().f_damage;
                }
                else
                {
                    go_unitInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "-";
                }
                go_unitInfo.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "" + building.f_health;
                go_unitInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = building.GetUnitsInfo();
                Vector3 cameraPoint = new Vector3(0,0,0);
                foreach (Transform child in go_selectedUnit.transform)
                {
                    if (child.name == "CameraPoint")
                    {
                        cameraPoint = child.position;
                    }
                }                
                UnitCamera.position = cameraPoint;
                UnitCamera.LookAt(go_selectedUnit.transform);
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
                || button.gameObject.name == "UpgradeBarracksButton" || button.gameObject.name == "WorkerActionButton")
            {
                Destroy(button.gameObject);
            }
        }
        

        if (go_selectedUnit != null)
        {
            if (go_selectedUnit.tag == "PlayerUnit")
            {
                for (int i = 1; i < 4; ++i)
                {
                    if ((i == 0 && bc.GetListOfUnit().Count <= 1) || i == 1 || (i == 2 && bc.GetListOfUnit().Count <= 1 && go_selectedUnit.GetComponent<PlayerUnitInfo>().GetUnitType() == PlayerUnitInfo.PlayerUnitType.PUN_WORKER) || i == 3)
                    {
                        GameObject goButton = (GameObject)Instantiate(go_actionButton);
                        goButton.name = "ActionButton";
                        goButton.transform.SetParent(go_actionPanel.transform, false);
                        goButton.transform.localScale = new Vector3(1, 1, 1);
                        goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                        goButton.GetComponent<ChooseCommand>().go_ConfirmUpgrade = go_ConfirmButton;
                        goButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                        goButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                        goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length * (i), 0);
                        switch (i)
                        {
                            case 0: // No longer needed
                                goButton.GetComponentInChildren<Text>().text = "CLOSE";
                                goButton.GetComponent<Image>().sprite = cancelImage;
                                break;
                            case 1:
                                goButton.GetComponentInChildren<Text>().text = "MOVE";
                                goButton.GetComponent<Image>().sprite = moveImage;
                                break;
                            case 2:
                                if (go_selectedUnit.GetComponent<PlayerUnitInfo>().GetUnitType() == PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                                {
                                    goButton.GetComponentInChildren<Text>().text = "BUILD";
                                    goButton.GetComponent<Image>().sprite = buildImage;
                                }
                                else // not needed anymore
                                {
                                    goButton.GetComponentInChildren<Text>().text = "UPRANK";
                                    goButton.GetComponent<Image>().sprite = uprankImage;
                                }
                                break;
                            case 3:
                                goButton.GetComponentInChildren<Text>().text = "SELECTMORE";
                                goButton.GetComponent<Image>().sprite = selectMoreImage;
                                goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x - go_actionButton.GetComponent<RectTransform>().rect.height, go_selectButton.transform.localPosition.y, 0);
                                break;
                        }
                    }
                }                
            }
            else if (go_selectedUnit.tag == "SelectableBuilding" && go_selectedUnit.transform.parent != GameObject.FindGameObjectWithTag("EnemyBuildingList").transform)
            {
                GameObject goButton = (GameObject)Instantiate(go_actionButton);
                goButton.name = "ActionButton";
                goButton.transform.SetParent(go_actionPanel.transform, false);
                goButton.transform.localScale = new Vector3(1, 1, 1);
                goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                goButton.GetComponent<ChooseCommand>().go_ConfirmUpgrade = go_ConfirmButton;
                goButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                goButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y, 0);
                goButton.GetComponentInChildren<Text>().text = "CLOSE";
                goButton.GetComponent<Image>().sprite = cancelImage;
                if (go_selectedUnit.name == "Barracks")
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                        secondButton.name = "ActionButton";
                        secondButton.transform.SetParent(go_actionPanel.transform, false);
                        secondButton.transform.localScale = new Vector3(1, 1, 1);
                        secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                        secondButton.GetComponent<ChooseCommand>().go_BarracksPanel = go_barracksPanel;
                        secondButton.GetComponent<ChooseCommand>().go_ConfirmUpgrade = go_ConfirmButton;
                        go_barracksPanel.GetComponent<BarracksPanelInfo>().go_SelectedBarracks = go_selectedUnit;
                        secondButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                        secondButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);

                        //goButton.transform.localPosition = Vector3.zero;
                        secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length, 0);
                        secondButton.GetComponentInChildren<Text>().text = "CREATE";
                        secondButton.GetComponent<Image>().sprite = createUnitImage;
                        if (j == 1)
                        {
                            secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length * 2, 0);
                            secondButton.name = "UpgradeBarracksButton";
                            secondButton.GetComponentInChildren<Text>().text = "UPGRADEBARRACKS";
                            //secondButton.GetComponentInChildren<Text>().text = "" + go_selectedUnit.GetComponent<BarracksBehaviour>().i_levelUpCost;
                            //secondButton.GetComponentInChildren<Text>().color = new Color(0, 0, 0, 255);
                            secondButton.GetComponent<Image>().sprite = uprankImage;
                        }
                    }

                }
                else if (go_selectedUnit.GetComponent<TownHallBehaviour>() != null)
                {
                    for (int i = 0;i < 2;++i)
                    {
                        if ((i == 1 && GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().i_playerLevel < 3) || i == 0)
                        {
                            if (i == 0)
                            {
                                GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                                secondButton.name = "WorkerActionButton";
                                secondButton.transform.SetParent(go_actionPanel.transform, false);
                                secondButton.transform.localScale = new Vector3(1, 1, 1);
                                secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                                secondButton.GetComponent<ChooseCommand>().go_ConfirmUpgrade = go_ConfirmButton;
                                secondButton.GetComponent<ChooseCommand>().go_ConfirmWorker = go_ConfirmWorker;
                                secondButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                                secondButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);

                                secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length, 0);
                                secondButton.GetComponentInChildren<Text>().text = " ";
                                secondButton.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 255);
                                secondButton.GetComponent<Image>().sprite = createWorkerImage;
                            }
                            else
                            {
                                GameObject secondButton = (GameObject)Instantiate(go_actionButton);
                                secondButton.name = "UpgradeActionButton";
                                secondButton.transform.SetParent(go_actionPanel.transform, false);
                                secondButton.transform.localScale = new Vector3(1, 1, 1);
                                secondButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
                                secondButton.GetComponent<ChooseCommand>().go_ConfirmUpgrade = go_ConfirmButton;
                                secondButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                                secondButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);

                                secondButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length * 2, 0);
                                //secondButton.GetComponentInChildren<Text>().text = "" + GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().f_upgradeCost;
                                //secondButton.GetComponentInChildren<Text>().color = new Color(0, 0, 0, 255);
                                secondButton.GetComponent<Image>().sprite = uprankImage;
                            }
                        }
                    }
                }
            }
        }     
    }

    public void CreateBuildButton()
    {
        foreach (Transform button in gameObject.transform)
        {
            if (button.gameObject.name == "ActionButton" || button.gameObject.name == "UpgradeActionButton"
                || button.gameObject.name == "UpgradeBarracksButton" || button.gameObject.name == "WorkerActionButton")
            {
                Destroy(button.gameObject);
            }
        }

        GameObject goButton = (GameObject)Instantiate(go_actionButton);
        goButton.name = "ActionButton";
        goButton.transform.SetParent(go_actionPanel.transform, false);
        goButton.transform.localScale = new Vector3(1, 1, 1);
        goButton.GetComponent<ChooseCommand>().go_BuildingPanel = go_buildPanel;
        goButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        goButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        goButton.transform.localPosition = new Vector3(go_selectButton.transform.localPosition.x, go_selectButton.transform.localPosition.y + go_actionButton_Length, 0);
        goButton.GetComponentInChildren<Text>().text = "PLACE";
        goButton.GetComponent<Image>().sprite = buildImage;
    }
}
