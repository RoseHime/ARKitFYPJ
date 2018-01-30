using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerBehaviour : MonoBehaviour {

    public GameObject go_enemyPrefab;
    public GameObject go_secondPrefab;
    public GameObject go_thirdPrefab;

    public float f_levelTimer = 300; // 5 minutes
    public float f_spawnRate = 10;
    private float f_cooldown = 0;
    private float f_levelCooldown = 0;

    public int i_level = 1;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (i_level < 3)
        {
            f_levelCooldown += Time.deltaTime;
            if (f_levelCooldown > f_levelTimer)
            {
                f_levelCooldown = 0;
                i_level++;

                if (i_level == 2)
                {
                    go_enemyPrefab = go_secondPrefab;
                }
                else if (i_level == 3)
                {
                    go_enemyPrefab = go_thirdPrefab;
                }
            }
        }


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
        GameObject tempEnemy = Instantiate(go_enemyPrefab, GameObject.FindGameObjectWithTag("EnemyList").transform);
        tempEnemy.transform.position = gameObject.transform.position;
        tempEnemy.GetComponent<NavMeshAgent>().Warp(gameObject.transform.position);
        //Debug.Log("Barracks:" + gameObject.transform.position);
        //Debug.Log("Enemy:" + gameObject.transform.position);
        //tempEnemy.transform.position = transform.GetChild(0).position;
        tempEnemy.transform.localScale = go_enemyPrefab.transform.localScale;
        tempEnemy.name = go_enemyPrefab.name;

        tempEnemy.GetComponent<EnemyBehaviour>().destination = transform.GetChild(0).position;
        tempEnemy.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;
        tempEnemy.GetComponent<EnemyBehaviour>().isDefending = true;
        return tempEnemy;
    }
}
