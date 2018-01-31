using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public GameObject go_PlayerUnitList;
    public List<Transform> listOfUnit = new List<Transform>();

    bool b_Reupdating;

    private GameObject go_Pivot;

	// Use this for initialization
	void Start () {
        AddToListAtStart();
        b_Reupdating = false;

        go_Pivot = GameObject.FindGameObjectWithTag("Pivot").transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Count" + listOfUnit.Count);
        Debug.Log("Capacity" + listOfUnit.Capacity);
        
        if(go_Pivot.activeSelf)
            UpdateState();
        //Update list of unit in the game without readding the unit that is added.
        // UpdateList();
        for (int i = 0; i < listOfUnit.Count; i++)
        {
            if (listOfUnit[i] != null)
            {
                b_Reupdating = true;
            }
        }

        if (b_Reupdating)
        {
            ReupdateList();
        }
    }

    void AddToListAtStart()
    {
        foreach (Transform unitChild in go_PlayerUnitList.transform)
        {
            listOfUnit.Add(unitChild);
            //if (listOfUnit.Count > 0)
            //{
            //    for (int i = 0; i < listOfUnit.Count; i++)
            //    {
            //        if (unitChild.GetInstanceID() != listOfUnit[i].GetInstanceID())
            //        {
            //            listOfUnit.Add(unitChild.gameObject);
            //        }
            //    }
            //}
            //else
            //{
            //    listOfUnit.Add(unitChild.gameObject);
            //}
            //
            //if (unitChild == null)
            //{
            //    for (int i = 0; i < listOfUnit.Count; i++)
            //    {
            //        if (unitChild.GetInstanceID() == listOfUnit[i].GetInstanceID())
            //        {
            //            listOfUnit.Remove(unitChild.gameObject);
            //        }
            //    }
            //}
        }
    }

    void UpdateState()
    {
        for (int i = 0; i < listOfUnit.Count; i++)
        {
            if (listOfUnit[i] != null)
            {
                if (listOfUnit[i].GetComponent<PlayerUnitInfo>().PUS == PlayerUnitInfo.PlayerUnitState.PUS_MOVE)
                {
                    listOfUnit[i].GetComponent<PlayerFSM>().MoveToTargetPos();
                    //If unit type is not worker to detect enemy
                    if (listOfUnit[i].GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                    {
                        listOfUnit[i].GetComponent<PlayerFSM>().DetectEnemy();
                    }
                }
                else if (listOfUnit[i].GetComponent<PlayerUnitInfo>().PUS == PlayerUnitInfo.PlayerUnitState.PUS_GUARD)
                {
                    //If unit type is not worker to enemy detect
                    if (listOfUnit[i].GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                    {
                        listOfUnit[i].GetComponent<PlayerFSM>().DetectEnemy();
                    }
                }
                else if (listOfUnit[i].GetComponent<PlayerUnitInfo>().PUS == PlayerUnitInfo.PlayerUnitState.PUS_HARVEST)
                {
                    //If unit type is worker
                    if (listOfUnit[i].GetComponent<PlayerUnitInfo>().GetUnitType() == PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                    {
                        listOfUnit[i].GetComponent<PlayerFSM>().OnHarvestMode();
                        listOfUnit[i].GetComponent<PlayerFSM>().StartHarvesting();
                    }
                }
                else if (listOfUnit[i].GetComponent<PlayerUnitInfo>().PUS == PlayerUnitInfo.PlayerUnitState.PUS_ATTACK)
                {
                    //If unit type is not worker to fight
                    if (listOfUnit[i].GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
                    {
                        listOfUnit[i].GetComponent<PlayerFSM>().AttackEnemy();
                    }
                }
            }
        }
    }

    void ReupdateList()
    {
        if (b_Reupdating)
        {
            listOfUnit.Clear();
            foreach (Transform unitChild in go_PlayerUnitList.transform)
            {
                listOfUnit.Add(unitChild);
            }
            b_Reupdating = false;
        }
    }

    private List<Transform> GetListofUnit()
    {
        return listOfUnit;
    }
}
