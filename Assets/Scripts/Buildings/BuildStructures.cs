using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStructures : MonoBehaviour {

    public GameObject enemyList;
    public GameObject go_TowerPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildBuilding(Vector3 position)
    {
        GameObject tempBuilding = Instantiate(go_TowerPrefab);
        tempBuilding.transform.position = position;
        tempBuilding.name = "Tower";
        tempBuilding.GetComponent<TowerBehaviour>().enemyList = enemyList;
    }
}
