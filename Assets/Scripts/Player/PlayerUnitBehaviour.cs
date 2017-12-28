using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private GameObject go_Resource;
    private GameObject go_Depot;
    string s_name;

    //Unit Individual Info
    public float f_HealthPoint;
    public float f_speed;
    public float f_range;

    private int i_resourceWOOD;
    private int i_resourceGOLD;
    public bool b_StartHarvest;
    bool b_HoldingResource;

    public bool b_Selected;
    private GameObject go_CommandMenu;
    private Rigidbody rb_Body;
    RaycastHit rcHit;
    Vector3 rcHitPosition;

    private Vector3 v3_currentPos;
    private Vector3 v3_targetPos;
    private float f_distanceY;
    private Vector3 offset_Y;

    public bool b_buildBuilding;
    public bool b_Moving;

    // Use this for initialization
    void Start()
    {
        go_CommandMenu = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        go_CommandMenu.SetActive(false);

        i_resourceWOOD = 0;
        i_resourceGOLD = 0;
        T_Enemy = GameObject.FindGameObjectWithTag("EnemyList").transform;
        PUS = PlayerUnitState.PUS_GUARD;
        rb_Body = gameObject.GetComponent<Rigidbody>();

        b_StartHarvest = false;
        b_HoldingResource = false;
        b_Selected = false;
        b_Moving = false;
        b_buildBuilding = false;
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 10, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
            rcHitPosition = hit.point;
            f_distanceY = gameObject.GetComponent<Transform>().position.y - rcHitPosition.y;

            offset_Y = new Vector3(0, f_distanceY, 0);
        }


    }



    // Update is called once per frame
    void Update()
    {

        if (b_StartHarvest)
        {
            PUS = PlayerUnitState.PUS_HARVEST;
        }
        if (b_Moving)
            PUS = PlayerUnitState.PUS_MOVE;
        else if (!b_Moving && !b_StartHarvest)
            PUS = PlayerUnitState.PUS_GUARD;

        CheckWhetherStillOnGround();
        //if (Physics.Raycast(gameObject.GetComponent<Transform>().transform.position, Vector3.down, out rcHit))
        //{
        //    rcHitPosition = rcHit.point;
        //    f_distanceY = gameObject.GetComponent<Transform>().transform.position.y - rcHitPosition.y;
        //    offset_Y = new Vector3(0, f_distanceY, 0);
        //}

        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 10, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + f_distanceY, transform.position.z);
        }

        switch (PUS)
        {
            case PlayerUnitState.PUS_MOVE:
                {
                    rb_Body.isKinematic = false;
                    MoveToTargetPos();
                    break;

                }

            case PlayerUnitState.PUS_ATTACK:
                {
                    break;
                }

            case PlayerUnitState.PUS_GUARD:
                {
                    rb_Body.isKinematic = true;
                    DetectEnemyUnit();
                    break;
                }
            case PlayerUnitState.PUS_HARVEST:
                {
                    rb_Body.isKinematic = false;
                    OnHarvestMode();
                    break;
                }
        }
    }

    void DetectEnemyUnit()
    {
        foreach (Transform T_enemyChild in T_Enemy)
        {
            if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= GetRange() * GetRange())
            {
                //DO something
                PUS = PlayerUnitState.PUS_ATTACK;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                //Debug.Log("Detected");
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.gray;
            }

        }
    }

    public void OnHarvestMode()
    {
        if (s_name == "GoldMine")
            go_Resource = GameObject.FindGameObjectWithTag("Resources").transform.GetChild(0).gameObject;
        else if (s_name == "Tree")
            go_Resource = GameObject.FindGameObjectWithTag("Resources").transform.GetChild(1).gameObject;

        go_Depot = GameObject.FindGameObjectWithTag("BuildingList");
        foreach (Transform go_PlayerBuilding in go_Depot.transform)
        {
            if (go_PlayerBuilding.gameObject.name == "ResourceDepot")
            {
                go_Depot = go_PlayerBuilding.gameObject;
                break;
            }
        }

        if (b_StartHarvest)
        {
            //GetComponent<Rigidbody>().useGravity = true;
            if (!b_HoldingResource)
            {
                Vector3 lookAtMine = new Vector3(go_Resource.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Resource.GetComponent<Transform>().position.z);
                gameObject.transform.LookAt(lookAtMine);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                                                                                                go_Resource.GetComponent<Transform>().position,
                                                                                                GetSpeed() * Time.deltaTime);
            }
            else if (b_HoldingResource)
            {
                Vector3 lookAtDepot = new Vector3(go_Depot.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Depot.GetComponent<Transform>().position.z);
                gameObject.transform.LookAt(lookAtDepot);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                                                                                                go_Depot.GetComponent<Transform>().position,
                                                                                                GetSpeed() * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == go_Resource)
        {
            Debug.Log("Touch GoldMine");
            if (s_name == "GoldMine")
            {
                if (i_resourceGOLD != 1 && go_Resource.GetComponent<GoldMineBehaviour>().CollectGold())
                {
                    b_HoldingResource = true;
                    i_resourceGOLD += go_Resource.GetComponent<GoldMineBehaviour>().i_goldDistributed;
                }
            }
            else if (s_name == "Tree")
            {
                if (i_resourceWOOD != 1 && go_Resource.GetComponent<TreeBehaviour>().CollectWood())
                {
                    b_HoldingResource = true;
                    i_resourceWOOD += go_Resource.GetComponent<TreeBehaviour>().i_woodDistributed;
                }
            }

        }
        else if (collision.gameObject == go_Depot)
        {
            go_Depot.GetComponent<ResourceDepotBehaviour>().StoreGold(i_resourceGOLD);
            go_Depot.GetComponent<ResourceDepotBehaviour>().StoreWood(i_resourceWOOD);
            b_HoldingResource = false;
            i_resourceGOLD = 0;
            i_resourceWOOD = 0;
        }
    }

    public void SetBuildingTargetPos(Vector3 v3_bTargetPos, string name)
    {
        s_name = name;
        v3_targetPos = v3_bTargetPos;
        // b_Selected = false;
        b_StartHarvest = true;
    }

    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        // b_Selected = false;
        b_StartHarvest = false;
        b_Moving = true;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if ((v3_currentPos - (v3_targetPos + offset_Y)).magnitude > 0.01f)
        {
            Vector3 v3_seeTarget = new Vector3(v3_targetPos.x, gameObject.transform.position.y, v3_targetPos.z);
            gameObject.transform.LookAt(v3_seeTarget);
            gameObject.transform.position = Vector3.MoveTowards(v3_currentPos, v3_targetPos + offset_Y, f_speed * Time.deltaTime);
            //GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            if (b_buildBuilding)
            {
                GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>().BuildBuilding(v3_currentPos);
                b_buildBuilding = false;
            }
            b_Moving = false;
            //GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void CheckWhetherStillOnGround()
    {
        if ((gameObject.GetComponent<Transform>().transform.position.y - rcHitPosition.y) != f_distanceY)
        {
            gameObject.GetComponent<Transform>().transform.position.Set(gameObject.GetComponent<Transform>().transform.position.x,
                                                                        rcHitPosition.y + f_distanceY,
                                                                        gameObject.GetComponent<Transform>().transform.position.z);
        }
    }

    void OnClick()
    {
        go_CommandMenu.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_CommandMenu.GetComponent<CreateActionButton>().CreateButtons();
        go_CommandMenu.SetActive(true);
    }

    public float GetRange()
    {
        return f_range;
    }
    public float GetSpeed()
    {
        return f_speed;
    }
}