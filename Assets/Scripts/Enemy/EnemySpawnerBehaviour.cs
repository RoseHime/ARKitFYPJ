using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour {

    public GameObject go_enemyPrefab;

    public float f_spawnRate = 10;
    private float f_cooldown = 0;

    public int i_maxUnits = 1;

    public List<GameObject> localEnemyList;
    bool squadIsFull = false;

    public bool isMoving = false;
    public bool isRetreating = true;

    public float f_morale = 50;
    public float f_moraleIncreaseRate = 1;

    float previousTotalHP = 0;

    public float f_moraleCooldown = 5;
    float f_moraleTimer = 0;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (localEnemyList.Count == 0)
        {
            squadIsFull = false;
        }

        if (f_cooldown <= f_spawnRate)
        {
            f_cooldown += Time.deltaTime;
        }
        else
        {
            if (localEnemyList.Count < i_maxUnits && !squadIsFull)
            {
                SpawnUnit();
                f_cooldown = 0;
                if (localEnemyList.Count >= i_maxUnits)
                {
                    squadIsFull = true;
                }
            }
        }

        if (localEnemyList.Count < i_maxUnits && squadIsFull && isRetreating)
        {
            squadIsFull = false;
        }

        List<GameObject> thingsToRemove = new List<GameObject>();
        foreach(GameObject thing in localEnemyList)
        {
            if (thing == null)
            {
                thingsToRemove.Add(thing);
            }
        }

        foreach(GameObject thing in thingsToRemove)
        {
            localEnemyList.Remove(thing);
        }

        isMoving = CheckIfMoving();
        UpdateMorale();
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

        previousTotalHP += tempEnemy.GetComponent<EnemyBehaviour>().f_health;
        f_morale += 10;
        
    }

    public void MoveUnits(Vector3 position)
    {
        isMoving = true;
        isRetreating = false;
        foreach (GameObject unit in localEnemyList)
        {
            unit.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;
            unit.GetComponent<EnemyBehaviour>().destination = position;
        }
    }

    public void RetreatUnits(Vector3 position)
    {
        MoveUnits(position);
        isRetreating = true;
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

    void UpdateMorale()
    {

        float currentTotalHP = 0;
        foreach (GameObject unit in localEnemyList)
        {
            currentTotalHP += unit.GetComponent<EnemyBehaviour>().f_health;
        }

        if (previousTotalHP == currentTotalHP)
        {
            if (f_moraleTimer < f_moraleCooldown)
            {
                f_moraleTimer += Time.deltaTime;
            }

            if (f_moraleTimer >= f_moraleCooldown)
            {
                f_morale = Mathf.Min(100,f_morale + f_moraleIncreaseRate * Time.deltaTime);
            }
        }
        else
        {
            f_moraleTimer = 0;
        }


        if (previousTotalHP > currentTotalHP)
        {
            f_morale = Mathf.Max(0, f_morale - 50 * (previousTotalHP - currentTotalHP) / previousTotalHP);
        }

        previousTotalHP = currentTotalHP;
    }
}
