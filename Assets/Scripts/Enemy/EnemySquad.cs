using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad {

    public List<GameObject> unitList;

    public List<Vector3> pathToFollow;

    public int i_maxUnits;
    public int i_currentWaypoint;

    // Use this for initialization
    public void Start () {
        i_maxUnits = 1;
        unitList = new List<GameObject>();
        pathToFollow = new List<Vector3>();
	}
	
	// Update is called once per frame
	public void Update () {
        List<GameObject> unitsToRemove = new List<GameObject>();
        foreach (GameObject unit in unitList)
        {
            if (unit == null)
            {
                unitsToRemove.Add(unit);
            }
        }
        foreach (GameObject unit in unitsToRemove)
        {
            unitList.Remove(unit);
        }

            if (i_currentWaypoint < pathToFollow.Count)
        {
            bool check = true;
            foreach (GameObject unit in unitList)
            {
                if (unit.GetComponent<EnemyBehaviour>().EUS == EnemyBehaviour.EnemyUnitState.EUS_MOVE)
                {
                    check = false;
                }
            }
            if (check == true)
            {
                MoveUnits(pathToFollow[i_currentWaypoint]);
                i_currentWaypoint++;
            }
        }
	}

    void MoveUnits(Vector3 position)
    {
        foreach (GameObject unit in unitList)
        {
            unit.GetComponent<EnemyBehaviour>().destination = position;
            unit.GetComponent<EnemyBehaviour>().EUS = EnemyBehaviour.EnemyUnitState.EUS_MOVE;
        }
    }
}
