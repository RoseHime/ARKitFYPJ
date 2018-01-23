using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEntities: MonoBehaviour {

    public GameObject enemyList;
    public GameObject go_TowerPrefab;

    public GameObject playerList;
    public GameObject go_playerPrefab;

    private PlayerInfo playerInfo;

    // Use this for initialization
    void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildBuilding(Vector3 position)
    {
        playerInfo.i_stone -= go_TowerPrefab.GetComponent<BuildingInfo>().i_stoneCost;
        playerInfo.i_wood -= go_TowerPrefab.GetComponent<BuildingInfo>().i_woodCost;
        playerInfo.i_magicStone -= go_TowerPrefab.GetComponent<BuildingInfo>().i_magicStoneCost;

        Vector3 originalScale = go_TowerPrefab.transform.localScale;
        GameObject tempBuilding = Instantiate(go_TowerPrefab);
        tempBuilding.transform.position = position;
        tempBuilding.name = go_TowerPrefab.name;
        tempBuilding.transform.parent = GameObject.FindGameObjectWithTag("BuildingList").transform;
        tempBuilding.transform.localScale = originalScale;
        Vector3 lookDirection = new Vector3(Camera.main.transform.position.x, tempBuilding.transform.position.y, Camera.main.transform.position.z);
        tempBuilding.transform.LookAt(lookDirection);
       
    }

    public void BuildPlayerUnit(Vector3 position)
    {
        playerInfo.i_stone -= go_playerPrefab.GetComponent<PlayerUnitBehaviour>().i_stoneCost;
        playerInfo.i_wood -= go_playerPrefab.GetComponent<PlayerUnitBehaviour>().i_woodCost;
        playerInfo.i_magicStone -= go_playerPrefab.GetComponent<PlayerUnitBehaviour>().i_magicStoneCost;

        Vector3 originalScale = go_playerPrefab.transform.localScale;
        GameObject temp = Instantiate(go_playerPrefab);
        temp.transform.position = position;
        temp.name = go_playerPrefab.name;
        temp.transform.parent = playerList.transform;
        temp.transform.localScale = originalScale;
    }
}
