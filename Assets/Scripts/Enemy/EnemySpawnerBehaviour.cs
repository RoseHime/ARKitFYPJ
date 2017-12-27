using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour {

    public GameObject go_enemyPrefab;

    public float f_spawnRate = 10;
    private float f_cooldown = 0;

    public int i_maxUnits = 1;


    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (f_cooldown <= f_spawnRate)
        {
            f_cooldown += Time.deltaTime;
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("EnemyList").transform.childCount < i_maxUnits)
            {
                SpawnUnit();
                f_cooldown = 0;
            }
        }
	}

    void SpawnUnit()
    {
        GameObject tempEnemy = Instantiate(go_enemyPrefab);
        tempEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyList").transform);
        tempEnemy.transform.position = gameObject.transform.position;
        tempEnemy.transform.localScale = go_enemyPrefab.transform.localScale;
        tempEnemy.name = go_enemyPrefab.name;
    }
}
