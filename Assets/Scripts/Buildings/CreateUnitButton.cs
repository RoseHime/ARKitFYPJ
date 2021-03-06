﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CreateUnitButton : MonoBehaviour {

    public GameObject go_unitPrefab;
    PlayerInfo playerInfo;

    private ButtonControl bc;
    private UnitManager um;

    // Use this for initialization
    void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();
        um = GameObject.FindGameObjectWithTag("UnitManager").GetComponent<UnitManager>();

        transform.GetChild(0).GetComponent<Text>().text = go_unitPrefab.name;
        //transform.GetChild(1).GetComponent<Text>().text = "Costs:";
        transform.GetChild(1).GetComponent<Text>().text = "";
        if (go_unitPrefab.GetComponent<PlayerUnitInfo>().i_woodCost > 0)
        {
            transform.GetChild(1).GetComponent<Text>().text += "Wood:" + go_unitPrefab.GetComponent<PlayerUnitInfo>().i_woodCost + "\n";
        }
        if (go_unitPrefab.GetComponent<PlayerUnitInfo>().i_stoneCost > 0)
        {
            transform.GetChild(1).GetComponent<Text>().text += "Stone:" + go_unitPrefab.GetComponent<PlayerUnitInfo>().i_stoneCost + "\n";
        }
        if (go_unitPrefab.GetComponent<PlayerUnitInfo>().i_magicStoneCost > 0)
        {
            transform.GetChild(1).GetComponent<Text>().text += "Bone:" + go_unitPrefab.GetComponent<PlayerUnitInfo>().i_magicStoneCost + "\n";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (playerInfo.i_stone >= go_unitPrefab.GetComponent<PlayerUnitInfo>().i_stoneCost && playerInfo.i_wood >= go_unitPrefab.GetComponent<PlayerUnitInfo>().i_woodCost 
            && playerInfo.i_magicStone >= go_unitPrefab.GetComponent<PlayerUnitInfo>().i_magicStoneCost)
        {
            if (go_unitPrefab != null)
            {
                CreateEntities createEntitiy = GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>();
                createEntitiy.go_playerPrefab = go_unitPrefab;
                GameObject unit = createEntitiy.BuildPlayerUnit(transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.position);
                //unit.transform.position = Vector3.MoveTowards(unit.transform.position, transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position, unit.GetComponent<PlayerUnitBehaviour>().GetSpeed() * Time.deltaTime);
                unit.GetComponent<NavMeshAgent>().Warp(unit.transform.position);
                unit.GetComponent<NavMeshAgent>().SetDestination(transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position);
                //if ((transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position - unit.GetComponent<Transform>().position).sqrMagnitude < 0.2f *0.2f)
                //{
                //    um.listOfUnit.Add(unit.transform);
                //}
                //transform.parent.parent.parent.parent.gameObject.SetActive(false);
                //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchInput>().b_Cancelled = true;
                //GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().SetBackToSelect();
            }
        }
    }
}
