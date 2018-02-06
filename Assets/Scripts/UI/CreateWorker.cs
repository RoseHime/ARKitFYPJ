using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CreateWorker : MonoBehaviour {

    public GameObject building;
    public Text textObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (building != null)
        {
            TownHallBehaviour townHall = building.GetComponent<TownHallBehaviour>();
            if (townHall != null)
            {
                textObject.text = "Create Worker for " + townHall.workerPrefab.GetComponent<PlayerUnitInfo>().i_woodCost + " wood & " + townHall.workerPrefab.GetComponent<PlayerUnitInfo>().i_stoneCost
                    + "stone?";
            }
        }

        
	}

    public void onClick()
    {
        PlayerInfo playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        TownHallBehaviour townHall = building.GetComponent<TownHallBehaviour>();
        if (playerInfo.i_stone >= townHall.workerPrefab.GetComponent<PlayerUnitInfo>().i_stoneCost && playerInfo.i_wood >= townHall.workerPrefab.GetComponent<PlayerUnitInfo>().i_woodCost
            && playerInfo.i_magicStone >= townHall.workerPrefab.GetComponent<PlayerUnitInfo>().i_magicStoneCost)
        {
            CreateEntities createEntitiy = GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>();
            createEntitiy.go_playerPrefab = townHall.workerPrefab;
            GameObject unit = createEntitiy.BuildPlayerUnit(townHall.transform.position);
            unit.GetComponent<NavMeshAgent>().Warp(unit.transform.position);
            unit.GetComponent<NavMeshAgent>().SetDestination(townHall.transform.GetChild(0).position);
            unit.transform.LookAt(townHall.transform.GetChild(0).position);
        }
    }
}
