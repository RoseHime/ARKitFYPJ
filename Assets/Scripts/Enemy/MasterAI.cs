using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour {

    public enum STRATEGY
    {
        PINCER,
        TURTLE,
        DIRECT,
    }

    public STRATEGY masterStrategy;

    public List<EnemySpawnerBehaviour> spawnerList;

    bool startStrategy = false;

    private int i_PincerWaypointCount;
    private int i_TurtleWaypointCount;
    private int i_DirectWaypointCount;
    private int i_CurrentWaypoint = 0;

    bool b_IsDefending = false;

	// Use this for initialization
	void Start () {
        i_PincerWaypointCount = transform.GetChild(0).GetChild(0).childCount;
        i_TurtleWaypointCount = transform.GetChild(1).GetChild(0).childCount;
        i_DirectWaypointCount = transform.GetChild(2).GetChild(0).childCount;
    }
	
	// Update is called once per frame
	void Update () {
        switch (masterStrategy)
        {
            case STRATEGY.PINCER:
                PincerStrat();
                break;
            case STRATEGY.TURTLE:
                TurtleStrat();
                break;
            case STRATEGY.DIRECT:
                DirectStrat();
                break;
        }	
	}

    void PincerStrat()
    {
        if (!startStrategy)
        {
            bool checkIfAllSpawned = true;
            for (int i = 0; i < spawnerList.Count; ++i)
            {
                if (spawnerList[i].localEnemyList.Count != spawnerList[i].i_maxUnits)
                {
                    checkIfAllSpawned = false;
                }
            }
            if (checkIfAllSpawned)
            {
                startStrategy = true;
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    spawnerList[i].isRetreating = false;
                }
            }
        }
        else
        {
            if (!CheckIfUnitsMoving() && i_CurrentWaypoint < i_PincerWaypointCount)
            {
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    if (!spawnerList[i].isRetreating)
                    {
                        spawnerList[i].MoveUnits(transform.GetChild(0).GetChild(i).GetChild(i_CurrentWaypoint).position);
                    }                    
                }
                i_CurrentWaypoint++;
            }
            for (int i = 0; i < spawnerList.Count; i++)
            {
                if (spawnerList[i].f_morale <= 20 && !spawnerList[i].isRetreating)
                {
                    spawnerList[i].RetreatUnits(spawnerList[i].transform.GetChild(0).position);
                }
            }

            if (!CheckIfUnitsAlive())
            {
                startStrategy = false;
                i_CurrentWaypoint = 0;
            }
        }
    }

    bool CheckIfUnitsMoving()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            if (spawnerList[i].isMoving && !spawnerList[i].isRetreating)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckIfUnitsAlive()
    {
        for (int i = 0;i < spawnerList.Count; i++)
        {
            if (spawnerList[i].localEnemyList.Count > 0 && !spawnerList[i].isRetreating)
            {
                return true;
            }
        }
        return false;
    }

    void TurtleStrat()
    {
        if (!startStrategy)
        {
            bool checkIfAllSpawned = true;
            for (int i = 0; i < spawnerList.Count; ++i)
            {
                if (spawnerList[i].localEnemyList.Count != spawnerList[i].i_maxUnits)
                {
                    checkIfAllSpawned = false;
                }
            }
            if (checkIfAllSpawned)
            {
                startStrategy = true;
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    spawnerList[i].isRetreating = false;
                }
            }
        }
        else
        {
            if (!CheckIfUnitsMoving() && i_CurrentWaypoint < i_TurtleWaypointCount)
            {
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    if (!spawnerList[i].isRetreating)
                    {
                        spawnerList[i].MoveUnits(transform.GetChild(1).GetChild(i).GetChild(i_CurrentWaypoint).position);
                    }
                }
                i_CurrentWaypoint++;
            }
            for (int i = 0; i < spawnerList.Count; i++)
            {
                if (spawnerList[i].f_morale <= 20 && !spawnerList[i].isRetreating)
                {
                    spawnerList[i].RetreatUnits(spawnerList[i].transform.GetChild(0).position);
                }
            }

            foreach (Transform building in GameObject.FindGameObjectWithTag("EnemyBuildingList").transform)
            {
                BuildingInfo buildInfo = building.GetComponent<BuildingInfo>();
                if (buildInfo.b_IsUnderAttack)
                {
                    b_IsDefending = true;
                    for (int i = 0; i < spawnerList.Count; i++)
                    {
                        spawnerList[i].MoveUnits(buildInfo.transform.GetChild(0).position);
                    }
                }
            }

            if (!CheckIfUnitsAlive() && !b_IsDefending)
            {
                startStrategy = false;
                i_CurrentWaypoint = 0;
            }
        }
    }

    void DirectStrat()
    {
        if (!startStrategy)
        {
            bool checkIfAllSpawned = true;
            for (int i = 0; i < spawnerList.Count; ++i)
            {
                if (spawnerList[i].localEnemyList.Count != spawnerList[i].i_maxUnits)
                {
                    checkIfAllSpawned = false;
                }
            }
            if (checkIfAllSpawned)
            {
                startStrategy = true;
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    spawnerList[i].isRetreating = false;
                }
            }
        }
        else
        {
            if (!CheckIfUnitsMoving() && i_CurrentWaypoint < i_DirectWaypointCount)
            {
                for (int i = 0; i < spawnerList.Count; i++)
                {
                    if (!spawnerList[i].isRetreating)
                    {
                        spawnerList[i].MoveUnits(transform.GetChild(2).GetChild(i).GetChild(i_CurrentWaypoint).position);
                    }
                }
                i_CurrentWaypoint++;
            }
            for (int i = 0; i < spawnerList.Count; i++)
            {
                if (spawnerList[i].f_morale <= 20 && !spawnerList[i].isRetreating)
                {
                    spawnerList[i].RetreatUnits(spawnerList[i].transform.GetChild(0).position);
                }
            }

            if (!CheckIfUnitsAlive()  && !b_IsDefending)
            {
                startStrategy = false;
                i_CurrentWaypoint = 0;
            }
        }
    }
}
