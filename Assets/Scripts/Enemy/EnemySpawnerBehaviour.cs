﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour {

    public GameObject go_enemyPrefab;

    public float f_spawnRate = 10;
    private float f_cooldown = 0;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (f_cooldown <= 1 / f_spawnRate)
        {
            f_cooldown += Time.deltaTime;
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("EnemyList").transform.childCount < GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<MasterAI>().i_UnitCapacity)
            {
                GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<MasterAI>().defendingUnits.Add(SpawnUnit());
                f_cooldown = 0;
            }

        }
    }

    public GameObject SpawnUnit()
    {
        GameObject tempEnemy = Instantiate(go_enemyPrefab);
        tempEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyList").transform);
        tempEnemy.transform.position = gameObject.transform.position;
        //tempEnemy.transform.position = transform.GetChild(0).position;
        tempEnemy.transform.localScale = go_enemyPrefab.transform.localScale;
        tempEnemy.name = go_enemyPrefab.name;

        tempEnemy.GetComponent<EnemyBehaviour>().destination = transform.GetChild(0).position;
        tempEnemy.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;
        tempEnemy.GetComponent<EnemyBehaviour>().isDefending = true;
        return tempEnemy;
    }
}
