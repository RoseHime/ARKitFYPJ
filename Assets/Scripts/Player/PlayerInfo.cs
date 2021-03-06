﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    public int i_stone;
    public int i_wood;
    public int i_magicStone;

    public Text stoneText;
    public Text woodText;
    public Text magicStoneText;

    public int i_playerLevel = 1;
    public int i_MaxUnitCapacity = 10;

    public int f_upgradeCost = 10;
    public int i_maxLevel = 3;

    public float f_LODHighQuality = 1;
    public float f_LODMedQuality = 2;
    public float f_LODLowQuality = 3;

    public GameObject winLoseScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stoneText.text = "" + i_stone;
        woodText.text = "" + i_wood;
        magicStoneText.text = "" + i_magicStone;

        if (GameObject.FindGameObjectWithTag("EnemyBuildingList") != null && !winLoseScreen.activeSelf)
        {
            if (!CheckForEnemyBase())
            {
                winLoseScreen.SetActive(true);
                winLoseScreen.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (!CheckForPlayerBase())
            {
                winLoseScreen.SetActive(true);
                winLoseScreen.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public bool LevelUp()
    {
        if (i_magicStone >= f_upgradeCost)
        {
            i_magicStone -= f_upgradeCost;
            i_playerLevel++;
            f_upgradeCost *= 2;
            return true;
        }
        return false;
    }

    bool CheckForEnemyBase()
    {
        foreach (Transform building in GameObject.FindGameObjectWithTag("EnemyBuildingList").transform)
        {
            if (building.GetComponent<TownHallBehaviour>() != null)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckForPlayerBase()
    {
        foreach (Transform building in GameObject.FindGameObjectWithTag("BuildingList").transform)
        {
            if (building.GetComponent<TownHallBehaviour>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
