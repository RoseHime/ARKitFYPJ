using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour {

    public enum STRATEGY
    {
        PINCER,
        TURTLE,
    }

    public STRATEGY masterStrategy;

    public List<EnemySpawnerBehaviour> spawnerList;

    bool startStrategy = false;

    private int i_WaypointCount;
    private int i_CurrentWaypoint = 0;

	// Use this for initialization
	void Start () {
        i_WaypointCount = transform.GetChild(0).childCount;
    }
	
	// Update is called once per frame
	void Update () {
        switch (masterStrategy)
        {
            case STRATEGY.PINCER:
                PincerStrat();
                break;
            case STRATEGY.TURTLE:
                break;
        }	
	}

    void PincerStrat()
    {
        if (!startStrategy)
        {
            bool checkIfAllSpawned = true;
            for (int i = 0; i < 3; ++i)
            {
                if (spawnerList[i].localEnemyList.Count != spawnerList[i].i_maxUnits)
                {
                    checkIfAllSpawned = false;
                }
            }
            if (checkIfAllSpawned)
            {
                startStrategy = true;
            }
        }
        else
        {
            if (!CheckIfUnitsMoving() && i_CurrentWaypoint < i_WaypointCount)
            {
                for (int i = 0; i < 3; i++)
                {
                    spawnerList[i].MoveUnits(transform.GetChild(i).GetChild(i_CurrentWaypoint).position);
                }
                i_CurrentWaypoint++;
            }
        }
    }

    bool CheckIfUnitsMoving()
    {
        for (int i = 0; i < 3; i++)
        {
            if (spawnerList[i].isMoving)
            {
                return true;
            }
        }
        return false;
    }
}
