using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBuildingButton : MonoBehaviour {

    private GameObject go_MainCamera;
    private TouchInput ti;

    public GameObject buildingPrefab;

    private PlayerInfo playerInfo;

    void Start()
    {
        go_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ti = go_MainCamera.GetComponent<TouchInput>();
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();

        transform.GetChild(0).GetComponent<Text>().text = buildingPrefab.name + "\nCosts:";
        if (buildingPrefab.GetComponent<BuildingInfo>().i_woodCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nWood:" + buildingPrefab.GetComponent<BuildingInfo>().i_woodCost;
        }

        if (buildingPrefab.GetComponent<BuildingInfo>().i_stoneCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nStone:" + buildingPrefab.GetComponent<BuildingInfo>().i_stoneCost;
        }

        if (buildingPrefab.GetComponent<BuildingInfo>().i_magicStoneCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nMagicStone:" + buildingPrefab.GetComponent<BuildingInfo>().i_magicStoneCost;
        }
    }

    public void onClick()
    {
        if (playerInfo.i_stone >= buildingPrefab.GetComponent<BuildingInfo>().i_stoneCost && playerInfo.i_wood >= buildingPrefab.GetComponent<BuildingInfo>().i_woodCost 
            && playerInfo.i_magicStone >= buildingPrefab.GetComponent<BuildingInfo>().i_magicStoneCost)
        {
            GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>().go_TowerPrefab = buildingPrefab;
            //ti.b_BuildTower = true;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<CreateActionButton>().CreateBuildButton();
            transform.parent.parent.gameObject.SetActive(false);
        }

    }

    public void offClick()
    {
        ti.b_TargetChose = true;
    }
}
