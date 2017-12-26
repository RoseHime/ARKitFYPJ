using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitBehaviour : MonoBehaviour
{
    enum PlayerUnitState
    {
        PUS_GUARD,
        PUS_ATTACK,
        PUS_MOVE,
        PUS_HARVEST,
        PUS_MAX_STATES
    }
    PlayerUnitState PUS;

    private Transform T_Enemy;
    public Color detectedColor;
    private GameObject go_GoldMine;
    private GameObject go_Depot;

    private int i_resource;
    bool b_StartHarvest;

    // Use this for initialization
    void Start()
    {
        i_resource = 0;
        T_Enemy = GameObject.FindGameObjectWithTag("EnemyList").transform;
        PUS = PlayerUnitState.PUS_GUARD;
    }

    // Update is called once per frame
    void Update()
    {

        switch (PUS)
        {
            case PlayerUnitState.PUS_MOVE:
                break;

            case PlayerUnitState.PUS_ATTACK:
                {
                    break;
                }

            case PlayerUnitState.PUS_GUARD:
                {
                    DetectEnemyUnit();
                    break;
                }
            case PlayerUnitState.PUS_HARVEST:
                {
                    OnHarvestMode();
                    break;
                }
        }

      
    }

    void DetectEnemyUnit()
    {
        foreach (Transform T_enemyChild in T_Enemy)
        {
            if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= gameObject.GetComponent<PlayerUnitUpdate>().GetRange() * gameObject.GetComponent<PlayerUnitUpdate>().GetRange())
            {
                //DO something
                PUS = PlayerUnitState.PUS_ATTACK;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                Debug.Log("Detected");
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = gameObject.GetComponent<PlayerUnitUpdate>().GetDefault();
            }

        }
    }

    public void OnHarvestMode()
    {
        go_GoldMine = GameObject.FindGameObjectWithTag("Resources");
        go_Depot = GameObject.FindGameObjectWithTag("BuildingList");
        foreach (Transform go_PlayerBuilding in go_Depot.transform)
        {
            if (go_PlayerBuilding.gameObject.name == "ResourceDepot")
            {
                go_Depot = go_PlayerBuilding.gameObject;
                b_StartHarvest = true;
                break;
            }
        }

        if (b_StartHarvest)
        {
            if (!go_GoldMine.GetComponent<GoldMineBehaviour>().CollectGold())
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position, 
                                                                    go_GoldMine.GetComponent<Transform>().position, 
                                                                    gameObject.GetComponent<PlayerUnitUpdate>().GetSpeed() * Time.deltaTime);
            }
            else if (go_GoldMine.GetComponent<GoldMineBehaviour>().CollectGold())
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                                                                    go_Depot.GetComponent<Transform>().position,
                                                                    gameObject.GetComponent<PlayerUnitUpdate>().GetSpeed() * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == go_GoldMine)
        {
            if (i_resource != 1 && go_GoldMine.GetComponent<GoldMineBehaviour>().CollectGold())
            {
                i_resource += go_GoldMine.GetComponent<GoldMineBehaviour>().i_goldDistributed;
            }

        }
        else if (collision.gameObject == go_Depot)
        {
            go_Depot.GetComponent<ResourceDepotBehaviour>().CollectGold(i_resource);
            i_resource = 0;
        }
    }
}