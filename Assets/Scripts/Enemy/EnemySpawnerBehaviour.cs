using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour {

    public GameObject go_enemyPrefab;

    public float f_spawnRate = 10;
    private float f_cooldown = 0;

    public int i_maxUnits = 1;

    public List<GameObject> localEnemyList;

    public bool isMoving = false;
    
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
            if (localEnemyList.Count < i_maxUnits)
            {
                SpawnUnit();
                f_cooldown = 0;
            }
        }

        foreach(GameObject thing in localEnemyList)
        {
            if (thing == null)
            {
                localEnemyList.Remove(thing);
            }
        }

        isMoving = CheckIfMoving();
	}

    void SpawnUnit()
    {
        GameObject tempEnemy = Instantiate(go_enemyPrefab);
        tempEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyList").transform);
        tempEnemy.transform.position = gameObject.transform.position;
        tempEnemy.transform.localScale = go_enemyPrefab.transform.localScale;
        tempEnemy.name = go_enemyPrefab.name;

        tempEnemy.GetComponent<EnemyBehaviour>().destination = transform.GetChild(0).position;
        tempEnemy.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;

        localEnemyList.Add(tempEnemy);
    }

    public void MoveUnits(Vector3 position)
    {
        isMoving = true;
        foreach (GameObject unit in localEnemyList)
        {
            unit.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;
            unit.GetComponent<EnemyBehaviour>().destination = position;
        }
    }

    bool CheckIfMoving()
    {
        bool check = false;
        foreach (GameObject unit in localEnemyList)
        {
            if (unit.GetComponent<EnemyBehaviour>().EUS != EnemyBehaviour.EnemyUnitState.EUS_IDLE)
            {
                check = true;
                break;
            }
        }

        return check;
    }
}
