using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour {

    PlayerInfo playerInfo;

    public int i_UnitCapacity = 10;
    public int i_SeriousLevel = 2;

    private int i_unitLevelIncrement = 1;
    public float f_unitIncreaseTime = 30;
    private float f_unitIncreaseTimer = 0;

    List<EnemySquad> attackingSquads;
    public List<GameObject> defendingUnits;

    Transform enemyList;
    Transform playerList;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        defendingUnits = new List<GameObject>();
        attackingSquads = new List<EnemySquad>();

        enemyList = GameObject.FindGameObjectWithTag("EnemyList").transform;
        playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyList.childCount > GetAttackerUnitCount())
        {
            PincerStrat();
        }
        else
        {
            DefenceStrat();
        }
        UpdateAttackSquads();
        if ((f_unitIncreaseTimer += Time.deltaTime) > f_unitIncreaseTime && i_UnitCapacity < 20)
        {
            i_UnitCapacity += i_unitLevelIncrement;
            f_unitIncreaseTimer = 0;
        }
    }

    void PincerStrat()
    {
        if (playerInfo.i_playerLevel >= i_SeriousLevel && defendingUnits.Count > 6)
        {
            Debug.Log("Strats activated");
            Transform Waypoints = transform.GetChild(0);
            for (int i = 0;i < 3;++i)
            {
                EnemySquad squad = new EnemySquad();
                squad.Start();
                squad.i_maxUnits = playerInfo.i_playerLevel;
                attackingSquads.Add(squad);             
                foreach (Transform point in Waypoints.GetChild(i))
                {
                    squad.pathToFollow.Add(point.position);
                }
                List<GameObject> unitsToRemove = new List<GameObject>();
                for (int j = 0;j < defendingUnits.Count/3;++j)
                {
                    attackingSquads[attackingSquads.Count - 1].unitList.Add(defendingUnits[j]);
                    unitsToRemove.Add(defendingUnits[j]);
                }
                foreach (GameObject unit in unitsToRemove)
                {
                    defendingUnits.Remove(unit);
                }
            }
        }
    }

    void DefenceStrat()
    {
        if (defendingUnits.Count > 10)
        {
            if (attackingSquads.Count == 0)
            {
                EnemySquad squad = new EnemySquad();
                squad.Start();
                squad.i_maxUnits = playerInfo.i_playerLevel;
                attackingSquads.Add(squad);
            }
            GameObject unit = defendingUnits[Random.Range(0, defendingUnits.Count)];
            if (attackingSquads[attackingSquads.Count - 1].unitList.Count < attackingSquads[attackingSquads.Count - 1].i_maxUnits)
            {
                attackingSquads[attackingSquads.Count - 1].unitList.Add(unit);
            }
            else
            {
                EnemySquad squad = new EnemySquad();
                squad.Start();
                squad.i_maxUnits = playerInfo.i_playerLevel;
                attackingSquads.Add(squad);
                attackingSquads[attackingSquads.Count - 1].unitList.Add(unit);
            }
            defendingUnits.Remove(unit);
        }
    }

    void UpdateAttackSquads()
    {
        //Debug.Log(attackingSquads.Count);
        foreach (EnemySquad squad in attackingSquads)
        {
            if (squad.unitList.Count >= squad.i_maxUnits && squad.pathToFollow.Count == 0)
            {
                Transform Waypoints = transform.GetChild(0);
                int random = Random.Range(0, 3);
                foreach (Transform point in Waypoints.GetChild(random))
                {
                    squad.pathToFollow.Add(point.position);
                }
            }

            squad.Update();
            
        }
    }

    int GetAttackerUnitCount()
    {
        int counter = 0;
        foreach (Transform unit in playerList)
        {
            if (unit.GetComponent<PlayerUnitInfo>().PUN != PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
            {
                counter++;
            }
        }
        return counter;
    }
}
