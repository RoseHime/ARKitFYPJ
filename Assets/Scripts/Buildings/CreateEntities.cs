﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEntities: MonoBehaviour {

    public GameObject enemyList;
    public GameObject go_TowerPrefab;

    public GameObject playerList;
    public GameObject go_playerPrefab;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildBuilding(Vector3 position)
    {
        Vector3 originalScale = go_TowerPrefab.transform.localScale;
        GameObject tempBuilding = Instantiate(go_TowerPrefab);
        tempBuilding.transform.position = position;
        tempBuilding.name = go_TowerPrefab.name;
        if (tempBuilding.GetComponent<TowerBehaviour>() != null)
            tempBuilding.GetComponent<TowerBehaviour>().enemyList = enemyList;
        tempBuilding.transform.parent = GameObject.FindGameObjectWithTag("BuildingList").transform;
        tempBuilding.transform.localScale = originalScale;
    }

    public void BuildPlayerUnit(Vector3 position)
    {
        Vector3 originalScale = go_playerPrefab.transform.localScale;
        GameObject temp = Instantiate(go_playerPrefab);
        temp.transform.position = position;
        temp.name = go_playerPrefab.name;
        temp.transform.parent = playerList.transform;
        temp.transform.localScale = originalScale;
    }
}
