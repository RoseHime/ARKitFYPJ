using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitBehaviour : MonoBehaviour
{
    public enum PlayerUnitType
    {
        PUN_WORKER,
        PUN_MELEE,
        PUN_RANGE,
        PUN_TANK,
        PUN_MAX
    }
    public PlayerUnitType PUN;

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
    private GameObject go_TargetedEnemy;
    public Color detectedColor;
    private GameObject go_Resource;
    private GameObject go_Depot;
    string s_name;

    //Unit Individual Info
    public float f_HealthPoint;
    public float f_range;
    public float f_atkDmg;

    public int i_magicStoneCost = 0;

    private int i_resourceWOOD;
    private int i_resourceSTONE;
    public bool b_StartHarvest;
    public bool b_HoldingResource;
    private bool b_isHarvesting;
    private float f_HarvestingTime;
    private bool b_isWoodHarvested;
    private bool b_isStoneHarvested;
    private GameObject go_TargetedTree;
    public bool b_toHarvestStone;
    public bool b_toHarvestTree;

    public bool b_Selected;
    private int i_AmountOfButtons;
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

    //Navmesh Agent
    [SerializeField]
    NavMeshHit navMeshHit;
    //Area Mask
    private int slope;
    private float f_goingUpSlope;
    private float f_goingDownSlope;
    private float f_onLandSpeed;

    NavMeshAgent _navMeshAgent;

    //Projectile
    private GameObject bullet_Prefab;
    public float f_bulletSpeed = 0.1f;
    public float f_fireRate = 1;
    private float f_fireCooldown = 0;

    // Use this for initialization
    void Start()
    {
        go_CommandMenu = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        go_CommandMenu.SetActive(false);

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        //_navMeshAgent.updateRotation = false;
        //Area Mask
        slope = 1 << NavMesh.GetAreaFromName("WalkableSlope");

        i_resourceWOOD = 0;
        i_resourceSTONE = 0;
        T_Enemy = GameObject.FindGameObjectWithTag("EnemyList").transform;
        PUS = PlayerUnitState.PUS_GUARD;
        //rb_Body = gameObject.GetComponent<Rigidbody>();
        b_Selected = false;
        b_Moving = false;

        if (PUN == PlayerUnitType.PUN_WORKER)
        {
            b_StartHarvest = false;
            b_HoldingResource = false;
            b_buildBuilding = false;
            b_isHarvesting = false;
            f_HarvestingTime = 0;
            b_isStoneHarvested = false;
            b_isWoodHarvested = false;
            b_toHarvestStone = false;
            b_toHarvestTree = false;
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 10, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
            rcHitPosition = hit.point;
            f_distanceY = gameObject.GetComponent<Transform>().position.y - rcHitPosition.y;

            offset_Y = new Vector3(0, f_distanceY, 0);
        }

        f_goingUpSlope = _navMeshAgent.speed / 2;
        f_goingDownSlope = _navMeshAgent.speed * 1.5f;
        f_onLandSpeed = _navMeshAgent.speed;
    }



    // Update is called once per frame
    void Update()
    {
        if (b_Selected)
        {
            _navMeshAgent.enabled = true;
        }

        if (f_HealthPoint <= 0)
        {
            Destroy(gameObject);
        }
        if (b_StartHarvest)
        {
            PUS = PlayerUnitState.PUS_HARVEST;
        }
        else if (!b_StartHarvest)
        {
            b_toHarvestStone = false;
            b_toHarvestTree = false;
        }

        if (PUN == PlayerUnitType.PUN_WORKER)
        {
            if (b_Moving)
                PUS = PlayerUnitState.PUS_MOVE;
            else if (!b_Moving && !b_StartHarvest)
                PUS = PlayerUnitState.PUS_GUARD;
        }
        else
        {
            if (b_Moving)
                PUS = PlayerUnitState.PUS_MOVE;
            else if (!b_Moving)
                PUS = PlayerUnitState.PUS_GUARD;
        }


        CheckWhetherStillOnGround();
        if (PUN != PlayerUnitType.PUN_WORKER && !b_Moving)
        {
            DetectEnemyUnit();
        }
     
        if (b_isHarvesting)
        {
            f_HarvestingTime += Time.deltaTime;
            if (f_HarvestingTime >= 2)
            {
                b_HoldingResource = true;
                _navMeshAgent.speed = f_onLandSpeed;
                if (b_isStoneHarvested)
                    i_resourceSTONE += go_Resource.GetComponent<StoneMineBehaviour>().CollectStone();
                else if (b_isWoodHarvested)
                    i_resourceWOOD += go_Resource.GetComponent<TreeBehaviour>().CollectWood();

                f_HarvestingTime = 0;
                b_isHarvesting = false;
            }
        }

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
                    if (PUN == PlayerUnitType.PUN_WORKER)
                        b_StartHarvest = false;
                    //rb_Body.isKinematic = false;
                    MoveToTargetPos();
                    break;

                }

            case PlayerUnitState.PUS_ATTACK:
                {
                    AttackEnemyUnit();
                    break;
                }

            case PlayerUnitState.PUS_GUARD:
                {
                    this.GetComponent<NavMeshAgent>().enabled = false;
                    //rb_Body.isKinematic = true;
                    DetectEnemyUnit();
                    break;
                }
            case PlayerUnitState.PUS_HARVEST:
                {
                    if (PUN == PlayerUnitType.PUN_WORKER)
                    {
                        IgnoreCollision();
                        //rb_Body.isKinematic = false;
                        OnHarvestMode();
                    }
                    break;
                }
        }
    }

    GameObject DetectEnemyUnit()
    {
        GameObject nearestTarget = null;

        foreach (Transform T_enemyChild in T_Enemy)
        {
            if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= GetRange() * GetRange())
            {
                //DO something
                nearestTarget = T_enemyChild.gameObject;
                PUS = PlayerUnitState.PUS_ATTACK;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                //Debug.Log("Detected");
            }
            else
            {
                nearestTarget = null;
                PUS = PlayerUnitState.PUS_GUARD;
                gameObject.GetComponent<Renderer>().material.color = Color.gray;
            }
        }

        foreach (Transform T_enemyBuilding in GameObject.FindGameObjectWithTag("EnemyBuildingList").transform)
        {
            if ((T_enemyBuilding.position - transform.position).sqrMagnitude < GetRange() * GetRange())
            {
                nearestTarget = T_enemyBuilding.gameObject;
            }
        }

        return nearestTarget;
    }

    void AttackEnemyUnit()
    {
        if (go_TargetedEnemy != null)
        {
            Vector3 difference = go_TargetedEnemy.transform.position - transform.position;

            if (difference.sqrMagnitude < GetRange() * GetRange())
            {
                if (PUN == PlayerUnitType.PUN_MELEE || PUN == PlayerUnitType.PUN_TANK)
                {
                    if (_navMeshAgent.transform.position != go_TargetedEnemy.transform.position)
                    {
                        _navMeshAgent.SetDestination(go_TargetedEnemy.transform.position);

                    }
                    else
                    {
                        //DealDmg();
                        go_TargetedEnemy.GetComponent<EnemyBehaviour>().f_health -= 1;
                    }
                }
                else if (PUN == PlayerUnitType.PUN_RANGE)
                {
                    if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                    {
                        f_fireCooldown = 0;
                        FireBullet(difference.normalized);
                    }
                }
            }
            else
            {
                //Chase?
            }
        }
        else
        {
            PUS = PlayerUnitState.PUS_GUARD;
        }
    }

    void FireBullet(Vector3 direction)
    {
        GameObject tempBullet = Instantiate(bullet_Prefab);
        tempBullet.transform.SetParent(GameObject.FindGameObjectWithTag("BulletPool").transform);
        tempBullet.transform.position = gameObject.transform.position;
        tempBullet.transform.localScale = bullet_Prefab.transform.lossyScale;
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = GetAttack();
        bullet_behaviour.direction = direction;

        tempBullet.SetActive(true);
    }

    public void OnHarvestMode()
    {
        if (b_toHarvestStone)
        {
            b_toHarvestTree = false;
            go_Resource = GameObject.FindGameObjectWithTag("Resources").transform.GetChild(0).gameObject;
        }
        else if (b_toHarvestTree)
        {
            b_toHarvestStone = false;
            foreach (Transform GetTree in GameObject.FindGameObjectWithTag("Resources").transform)
            {
                if (GetTree.name == s_name)
                    go_Resource = GetTree.gameObject;
            }
        }

        go_Depot = GameObject.FindGameObjectWithTag("BuildingList");
        foreach (Transform go_PlayerBuilding in go_Depot.transform)
        {
            if (go_PlayerBuilding.gameObject.name == "Base")
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
                //Vector3 lookAtMine = new Vector3(go_Resource.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Resource.GetComponent<Transform>().position.z);
                //gameObject.transform.LookAt(lookAtMine);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                //                                                                                go_Resource.GetComponent<Transform>().position,
                //                                                                                GetSpeed() * Time.deltaTime);
                _navMeshAgent.SetDestination(go_Resource.GetComponent<Transform>().position);
            }
            else if (b_HoldingResource)
            {
                //Vector3 lookAtDepot = new Vector3(go_Depot.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Depot.GetComponent<Transform>().position.z);
                //gameObject.transform.LookAt(lookAtDepot);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                //                                                                                go_Depot.GetComponent<Transform>().position,
                //                                                                                GetSpeed() * Time.deltaTime);
                _navMeshAgent.SetDestination(go_Depot.GetComponent<Transform>().position);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == go_Resource)
        {
            Debug.Log("Collided");
            _navMeshAgent.speed = 0f;
            if (collision.gameObject.tag == "StoneMine" && b_toHarvestStone && !b_HoldingResource)
                b_isStoneHarvested = true;
            else if (collision.gameObject.tag == "Tree" && b_toHarvestTree)
                b_isWoodHarvested = true;

            b_isHarvesting = true;
        }
        else if (collision.gameObject == go_Depot)
        {
            //_navMeshAgent.speed = 0f;
            if (b_isStoneHarvested)
            {

                go_Depot.GetComponent<TownHallBehaviour>().StoreStone(i_resourceSTONE);
                i_resourceSTONE = 0;
                b_isStoneHarvested = false;
            }
            else if (b_isWoodHarvested)
            {
                go_Depot.GetComponent<TownHallBehaviour>().StoreWood(i_resourceWOOD);
                i_resourceWOOD = 0;
                b_isWoodHarvested = false;
            }
            b_HoldingResource = false;
        }
    }

    public void SetBuildingTargetPos(Vector3 v3_bTargetPos, string name)
    {
        s_name = name;
        v3_targetPos = v3_bTargetPos;
        // b_Selected = false;
        b_Moving = false;
        b_StartHarvest = true;
    }

    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        // b_Selected = false;
        b_Moving = true;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if ((v3_currentPos - (v3_targetPos + offset_Y)).magnitude > 0.01f)
        {
            //Vector3 v3_seeTarget = new Vector3(v3_targetPos.x, gameObject.transform.position.y, v3_targetPos.z);
            //gameObject.transform.LookAt(v3_seeTarget);
            //gameObject.transform.position = Vector3.MoveTowards(v3_currentPos, v3_targetPos + offset_Y, f_speed * Time.deltaTime);
            _navMeshAgent.SetDestination(v3_targetPos + offset_Y);
            //GetComponent<Rigidbody>().useGravity = true;
            _navMeshAgent.SamplePathPosition(-1, 0.0f, out navMeshHit);
            if ((navMeshHit.mask == slope) && v3_targetPos.y > gameObject.transform.position.y)
            {
                Debug.Log("Going up Slope");
                _navMeshAgent.speed = f_goingUpSlope;
            }
            else if ((navMeshHit.mask == slope) && v3_targetPos.y < gameObject.transform.position.y)
            {
                Debug.Log("Going down  Slope");
                _navMeshAgent.speed = f_goingDownSlope;
            }
            else
            {
                Debug.Log("Land");
                _navMeshAgent.speed = f_onLandSpeed;
            }

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

    void IgnoreCollision()
    {
        GameObject go_PlayerUnitList = GameObject.FindGameObjectWithTag("PlayerList");
        foreach (Transform go_PULChild in go_PlayerUnitList.transform)
        {
            Physics.IgnoreCollision(go_PULChild.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
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


    public PlayerUnitType getType()
    {
        return PUN;
    }

    public int getAmountOfButton()
    {
        if (PUN == PlayerUnitType.PUN_WORKER)
        {
            return i_AmountOfButtons = 6;
        }
        else
        {
            return i_AmountOfButtons = 5;
        }
    }

    public float GetSpeed()
    {
        return _navMeshAgent.speed;
    }

    public float GetAttack()
    {
        return f_atkDmg;
    }

    public GameObject getCommand()
    {
        return go_CommandMenu;
    }
}