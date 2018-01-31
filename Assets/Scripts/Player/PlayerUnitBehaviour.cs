using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    public enum PlayerUnitState
    {
        PUS_GUARD,
        PUS_ATTACK,
        PUS_MOVE,
        PUS_HARVEST,
        PUS_MAX_STATES
    }
    public PlayerUnitState PUS;

    private Animator _animator;
    private Transform T_Enemy;
    private Transform T_ENemyBase;
    private GameObject go_TargetedEnemy;
    List<Transform> listOfEnemy = new List<Transform>();
    private GameObject go_TempEnemyHolder;
    public Color detectedColor;
    private GameObject go_Resource;
    private GameObject go_Depot;
    string s_name;

    //Unit Individual Info
    public float f_HealthPoint;
    public float f_range;
    public float f_speed;
    private float f_OriginSpeed;
    public float f_atkDmg;

    //
    List<Transform> getpath = new List<Transform>();
    public float reachDist = 1.0f;
    public int currentPoint = 0;
    //

    public int i_woodCost = 0;
    public int i_stoneCost = 0;
    public int i_magicStoneCost = 0;

    private int i_resourceWOOD;
    private int i_resourceSTONE;
    public bool b_StartHarvest;
    public bool b_HoldingResource;
    private bool b_isHarvesting;
    private float f_HarvestingTime;
    private bool b_isWoodHarvested;
    private bool b_isStoneHarvested;
    public bool b_toHarvestStone;
    public bool b_toHarvestTree;
    private Vector3 v3_CurrentHarvestTarget;

    public bool b_Selected;
    private int i_AmountOfButtons;
    private GameObject go_CommandMenu;
  
    private Vector3 v3_currentPos;
    private Vector3 v3_targetPos;

    public bool b_buildBuilding;
    public bool b_Moving;
    private bool b_DetectEnemy;
    private bool b_CollidedWithEnemy;
    private bool b_AttackingEnemy;
    private bool b_TargetedEnemy;
    private bool b_MovingToEnemy;

    //Projectile
    public GameObject bullet_Prefab;
    public float f_bulletSpeed;
    public float f_fireRate;
    private float f_fireCooldown;

    //ForWorker Building
    public GameObject go_BuildingPrefab;

    GameObject debugLog;

    //WaypointConnector WC;

    //
    NavMeshAgent _navmeshAgent;

    // Use this for initialization
    void Start()
    {
        _navmeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        //getpath = GameObject.FindGameObjectWithTag("MoveParent").GetComponent<WaypointConnector>().getCreatePath();

        //debugLog = GameObject.FindGameObjectWithTag("DebugPurpose").transform.GetChild(0).gameObject;
        //WC = GameObject.FindGameObjectWithTag("MoveParent").GetComponent<WaypointConnector>();

        go_CommandMenu = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        go_CommandMenu.SetActive(false);

        i_resourceWOOD = 0;
        i_resourceSTONE = 0;
        T_Enemy = GameObject.FindGameObjectWithTag("EnemyList").transform;
        T_ENemyBase = GameObject.FindGameObjectWithTag("EnemyBuildingList").transform;
        PUS = PlayerUnitState.PUS_GUARD;
        //rb_Body = gameObject.GetComponent<Rigidbody>();
        b_Selected = false;
        b_Moving = false;
        b_DetectEnemy = false;
        b_TargetedEnemy = false;
        b_CollidedWithEnemy = false;

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

        f_OriginSpeed = _navmeshAgent.speed;//f_speed;
        go_TargetedEnemy = null;

    }



    // Update is called once per frame
    void Update()
    {
        //debugLog.GetComponent<Text>().text = "state" + PUS + "," + "/n" +
        //                                     "CurrentPos"+ v3_currentPos + "," + "/n" +
        //                                     "targetpos" + v3_targetPos + "," + "/n" +
        //                                     "navmesh" + _navMeshAgent.enabled +  "," + "/n" +
        //                                     "isMoving" + b_Moving + "," + "/n" +
        //                                     "selected" + b_Selected;
        if (b_Selected)
        {
            //Debug.Log(PUS);

            foreach (Transform child in gameObject.transform)
            {
                if (child.transform.tag == "SelectionIcon")
                    child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.transform.tag == "SelectionIcon")
                    child.gameObject.SetActive(false);
            }
        }

        if (f_HealthPoint <= 0)
        {
            go_CommandMenu.SetActive(false);
            Destroy(gameObject);
        }
        if (b_StartHarvest)
        {
            PUS = PlayerUnitState.PUS_HARVEST;
        }
        else if (!b_StartHarvest)
        {
            _navmeshAgent.stoppingDistance = 0f;
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
            if (b_Moving || b_MovingToEnemy)
                PUS = PlayerUnitState.PUS_MOVE;
            else if (!b_Moving && !b_DetectEnemy && !b_TargetedEnemy)
                PUS = PlayerUnitState.PUS_GUARD;
            else if (b_TargetedEnemy)
                PUS = PlayerUnitState.PUS_ATTACK;
        }


        //if (PUN != PlayerUnitType.PUN_WORKER && !b_Moving)
        //{
        //    DetectEnemyUnit();
        //}
     
        if (b_isHarvesting)
        {
            _animator.ResetTrigger("b_IsMoving");
            _animator.SetTrigger("b_isHarvesting");
            f_HarvestingTime += Time.deltaTime;
            if (f_HarvestingTime >= 2)
            {
                b_HoldingResource = true;
                _animator.ResetTrigger("b_isHarvesting");
                _animator.SetTrigger("b_IsMoving");
                _navmeshAgent.speed = f_OriginSpeed;
                if (b_isStoneHarvested)
                    i_resourceSTONE += go_Resource.GetComponent<StoneMineBehaviour>().CollectStone();
                else if (b_isWoodHarvested)
                    i_resourceWOOD += go_Resource.GetComponent<TreeBehaviour>().CollectWood();

                f_HarvestingTime = 0;
                b_isHarvesting = false;
            }
        }
        SnapToGround();
        //CheckWhetherStillOnGround();

        switch (PUS)
        {
            case PlayerUnitState.PUS_MOVE:
                {
                    if (PUN == PlayerUnitType.PUN_WORKER)
                        b_StartHarvest = false;
                    //rb_Body.isKinematic = false;
                    if (PUN != PlayerUnitType.PUN_WORKER && !b_AttackingEnemy)
                        DetectEnemyUnit();

                    if (b_Moving)
                        MoveToTargetPos();
                    else if (b_MovingToEnemy)
                        MoveToEnemyPos();
                    break;

                }

            case PlayerUnitState.PUS_ATTACK:
                { 
                    AttackEnemyUnit();
                    break;
                }

            case PlayerUnitState.PUS_GUARD:
                {
                    //this.GetComponent<NavMeshAgent>().enabled = false;
                    //rb_Body.isKinematic = true;
                    if (PUN != PlayerUnitType.PUN_WORKER && !b_AttackingEnemy)
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
        if (b_DetectEnemy == false && go_TargetedEnemy == null)
        {
            List<Transform> nearbyEnemy = new List<Transform>();
            foreach (Transform T_enemyChild in T_Enemy)
            {
                if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= GetRange() * GetRange())
                {
                    //DO something
                    nearbyEnemy.Add(T_enemyChild);
                    //gameObject.GetComponent<Renderer>().material.color = Color.green;
                    //Debug.Log("Detected");
                }
            }
            foreach (Transform T_enemyBuilding in T_ENemyBase)
            {
                if ((T_enemyBuilding.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= GetRange() * GetRange())
                {
                    nearbyEnemy.Add(T_enemyBuilding);
                }
            }


            int i_Enemy = 0;
            while (i_Enemy < nearbyEnemy.Count)
            {
                if ( i_Enemy > 0)
                {
                    if ((gameObject.transform.position - nearbyEnemy[i_Enemy].position).sqrMagnitude < (gameObject.transform.position - go_TempEnemyHolder.transform.position).sqrMagnitude)
                    {
                        go_TempEnemyHolder = nearbyEnemy[i_Enemy].gameObject;
                    }
                    if (i_Enemy == nearbyEnemy.Count - 1)
                    {
                        PUS = PlayerUnitState.PUS_ATTACK;
                        go_TargetedEnemy = go_TempEnemyHolder;
                        listOfEnemy.Add(go_TargetedEnemy.transform);
                        b_DetectEnemy = true;
                    }
                }
                else
                {
                    go_TempEnemyHolder = nearbyEnemy[i_Enemy].gameObject;
                    if (i_Enemy == nearbyEnemy.Count - 1)
                    {
                        PUS = PlayerUnitState.PUS_ATTACK;
                        go_TargetedEnemy = go_TempEnemyHolder;
                        listOfEnemy.Add(go_TargetedEnemy.transform);
                        b_DetectEnemy = true;
                    }
                }
                i_Enemy++;
            }

         
        }

        return go_TargetedEnemy;
    }

    void AttackEnemyUnit()
    {
        if (go_TargetedEnemy != null)
        {
            if (b_DetectEnemy || b_TargetedEnemy)
            {
                listOfEnemy.Clear();
                b_Moving = false;
                b_AttackingEnemy = true;
                Vector3 enemyLoc = new Vector3(go_TargetedEnemy.transform.position.x, gameObject.transform.position.y, go_TargetedEnemy.transform.position.z);
                Vector3 difference = go_TargetedEnemy.transform.position - gameObject.transform.position;
                gameObject.transform.LookAt(enemyLoc);
                if (PUN == PlayerUnitType.PUN_MELEE || PUN == PlayerUnitType.PUN_TANK)
                {
                    //_navmeshAgent.stoppingDistance = 0.05f;
                    if (go_TargetedEnemy.tag == "Enemy")
                    {
                        if (difference.sqrMagnitude > 0.08f * 0.08f)
                        {
                            _navmeshAgent.stoppingDistance = 0.04f;
                            _animator.SetTrigger("b_IsMoving");
                            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, go_TargetedEnemy.transform.position, GetSpeed() * Time.deltaTime);
                            _navmeshAgent.SetDestination(go_TargetedEnemy.transform.position);
                            f_fireCooldown = 0;
                        }
                        else
                        {
                            _animator.ResetTrigger("b_IsMoving");

                            if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                            {
                                _animator.SetTrigger("b_IsAttacking");
                                f_fireCooldown = 0;
                                go_TargetedEnemy.gameObject.GetComponent<EnemyBehaviour>().f_health -= GetAttack();
                            }
                        }
                    }
                    else
                    {
                        if (difference.sqrMagnitude > 0.1f * 0.1f)
                        {
                            _navmeshAgent.stoppingDistance = 0.1f;

                            _animator.SetTrigger("b_IsMoving");
                            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, go_TargetedEnemy.transform.position, GetSpeed() * Time.deltaTime);
                            _navmeshAgent.SetDestination(go_TargetedEnemy.transform.position);
                            f_fireCooldown = 0;
                        }
                        else
                        {
                            _animator.ResetTrigger("b_IsMoving");
                            _navmeshAgent.ResetPath();

                            if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                            {
                                _animator.SetTrigger("b_IsAttacking");
                                f_fireCooldown = 0;
                                if (go_TargetedEnemy.name != "Base")
                                    go_TargetedEnemy.gameObject.GetComponent<BuildingInfo>().f_health -= GetAttack();
                                else
                                    go_TargetedEnemy.gameObject.GetComponent<TownHallBehaviour>().f_health -= GetAttack();
                            }
                        }
                    }
                }
                else if (PUN == PlayerUnitType.PUN_RANGE)
                {
                    if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                    {
                        _animator.SetTrigger("b_IsAttacking");
                        f_fireCooldown = 0;
                        FireBullet(difference.normalized);
                    }
                }
            }
        }


        //if ((go_TargetedEnemy.tag == "Enemy" && go_TargetedEnemy.GetComponent<EnemyBehaviour>().f_health <= 0) || 
        //    (go_TargetedEnemy.tag == "SelectableBuilding" && go_TargetedEnemy.GetComponent<BuildingInfo>().f_health <= 0) ||
        //   (go_TargetedEnemy.name == "Base" && go_TargetedEnemy.GetComponent<TownHallBehaviour>().f_health <= 0))
        if (go_TargetedEnemy == null)
        {
            //listOfEnemy.Remove(go_TargetedEnemy.transform);
            listOfEnemy.Clear();
            b_DetectEnemy = false;
            b_AttackingEnemy = false;
            b_TargetedEnemy = false;
            b_CollidedWithEnemy = false;
            //b_Moving = true;
            _animator.ResetTrigger("b_IsAttacking");
            _animator.ResetTrigger("b_IsMoving");
            _navmeshAgent.speed = f_OriginSpeed;
            f_speed = f_OriginSpeed;
            PUS = PlayerUnitState.PUS_GUARD;
        }
        else if ((go_TargetedEnemy.transform.position - gameObject.GetComponent<Transform>().position).sqrMagnitude > GetRange() * GetRange())
        {
            //listOfEnemy.Remove(go_TargetedEnemy.transform);
            b_DetectEnemy = false;
            b_AttackingEnemy = false;
            go_TargetedEnemy = null;
            b_CollidedWithEnemy = false;
            _animator.ResetTrigger("b_IsAttacking");
            _animator.ResetTrigger("b_IsMoving");
            //b_Moving = true;
            _navmeshAgent.speed = f_OriginSpeed;
            f_speed = f_OriginSpeed;
            PUS = PlayerUnitState.PUS_GUARD;
        }
    }

    void FireBullet(Vector3 direction)
    {
        GameObject tempBullet = Instantiate(bullet_Prefab);
        tempBullet.transform.SetParent(GameObject.FindGameObjectWithTag("BulletPool").transform);
        tempBullet.transform.position = gameObject.transform.position + new Vector3(0, GetComponent<Collider>().bounds.size.y / 2, 0);
        tempBullet.transform.localScale = bullet_Prefab.transform.lossyScale;
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = GetAttack();
        bullet_behaviour.direction = direction;
        bullet_behaviour.target = BulletBehaviour.BULLETTARGET.ENEMY;

        tempBullet.SetActive(true);
    }

    public void OnHarvestMode()
    {
        _navmeshAgent.speed = f_OriginSpeed;
        _animator.SetTrigger("b_IsMoving");

        if (b_toHarvestStone)
        {
            _navmeshAgent.stoppingDistance = 0.05f;
            b_toHarvestTree = false;
            foreach (Transform GetStone in GameObject.FindGameObjectWithTag("Resources").transform)
            {
                if (GetStone.name == s_name)
                    go_Resource = GetStone.gameObject;
            }
        }
        else if (b_toHarvestTree)
        {
            _navmeshAgent.stoppingDistance = 0.045f;

            b_toHarvestStone = false;
            foreach (Transform GetTree in GameObject.FindGameObjectWithTag("Resources").transform)
            {
                if (GetTree.name == s_name)
                {
                    go_Resource = GetTree.gameObject;
                    v3_CurrentHarvestTarget = go_Resource.transform.position;
                }
                else if (go_Resource == null && (GetTree.position - v3_CurrentHarvestTarget).sqrMagnitude < 0.005f)
                {
                    if (GetTree.tag == "Tree")
                    {
                        go_Resource = GetTree.gameObject;
                        v3_CurrentHarvestTarget = go_Resource.transform.position;
                    }
                }
            }
        }


        go_Depot = GameObject.FindGameObjectWithTag("BuildingList");
        foreach (Transform go_PlayerBuilding in go_Depot.transform)
        {
            if (go_PlayerBuilding.gameObject.name == "TownHall")
            {
                go_Depot = go_PlayerBuilding.gameObject;
                break;
            }
        }

        if (b_StartHarvest && _navmeshAgent.speed > 0)
        {
            //GetComponent<Rigidbody>().useGravity = true;
            if (!b_HoldingResource && go_Resource != null)
            {
                Vector3 lookAtMine = new Vector3(go_Resource.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Resource.GetComponent<Transform>().position.z);
                gameObject.transform.LookAt(lookAtMine);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                //                                                                                go_Resource.GetComponent<Transform>().position,
                //                                                                               GetSpeed() * Time.deltaTime);
                _navmeshAgent.SetDestination(lookAtMine);
            }
            else if (b_HoldingResource)
            {
                Vector3 lookAtDepot = new Vector3(go_Depot.GetComponent<Transform>().position.x, gameObject.transform.position.y, go_Depot.GetComponent<Transform>().position.z);
                gameObject.transform.LookAt(lookAtDepot);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position,
                //                                                                                go_Depot.GetComponent<Transform>().position,
                //                                                                                GetSpeed() * Time.deltaTime);
                _navmeshAgent.SetDestination(lookAtDepot);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == go_Resource)
        {
            Debug.Log("Collided");
            _navmeshAgent.speed = 0f;
            gameObject.transform.GetComponent<PlayerUnitBehaviour>().f_speed = 0f;
            _animator.SetTrigger("b_isHarvesting");

            if (collision.gameObject.tag == "StoneMine" && b_toHarvestStone && !b_HoldingResource)
                b_isStoneHarvested = true;
            else if (collision.gameObject.tag == "Tree" && b_toHarvestTree)
                b_isWoodHarvested = true;

            b_isHarvesting = true;
        }
        else if (collision.gameObject == go_Depot)
        {
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

        if (PUN == PlayerUnitType.PUN_MELEE)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                b_CollidedWithEnemy = true;
                f_speed = 0f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (PUN == PlayerUnitType.PUN_MELEE)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                b_CollidedWithEnemy = false;
                f_speed = f_OriginSpeed;
            }
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
        _navmeshAgent = GetComponent<NavMeshAgent>();
        v3_targetPos = v3_targetpos;
        v3_currentPos = gameObject.transform.position;
        _navmeshAgent.speed = f_OriginSpeed;
        _navmeshAgent.isStopped = false;
        //WC.SetCurrentClosestWaypoint(v3_currentPos);
        //WC.FindTargetClosestWaypoint(v3_targetPos);
        //currentPoint = 0;

        // b_Selected = false;
        b_Moving = true;
    }

    public void SetSpawnTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        v3_currentPos = gameObject.transform.position;
        b_Moving = true;
    }

    public GameObject SetEnemyTargetPos(GameObject go_Enemy)
    {
        v3_targetPos = go_Enemy.transform.position;
        go_TargetedEnemy = go_Enemy;
        listOfEnemy.Add(go_TargetedEnemy.transform);
        v3_currentPos = gameObject.transform.position;
        b_MovingToEnemy = true;

        return go_TargetedEnemy;
    }

    public void ConstructBuilding(Vector3 pos)
    {
        GameObject prefab = GameObject.FindGameObjectWithTag("GameFunctions").gameObject;
        prefab.GetComponent<CreateEntities>().go_TowerPrefab = go_BuildingPrefab;
        prefab.GetComponent<CreateEntities>().BuildBuilding(pos);
    }

    private void MoveToTargetPos()
    {
        _navmeshAgent.SetDestination(v3_targetPos);
        _animator.SetTrigger("b_IsMoving");
            //Vector3 dir = gameObject.transform.position - _navmeshAgent.nextPosition;
        Vector3 dir = _navmeshAgent.velocity + gameObject.transform.position;
        Vector3 lookAt = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
            gameObject.transform.LookAt(lookAt);
        if ((gameObject.transform.position - v3_targetPos).sqrMagnitude < 0.05f * 0.05f)
        {
            if (b_buildBuilding)
            {
                ConstructBuilding(v3_targetPos);
                b_buildBuilding = false;
            }

            PUS = PlayerUnitState.PUS_GUARD;
            b_Moving = false;
            _animator.ResetTrigger("b_IsMoving");
            _navmeshAgent.isStopped = true;
        }
    }

    private void MoveToEnemyPos()
    {
        _navmeshAgent.SetDestination(go_TargetedEnemy.transform.position);
        _animator.SetTrigger("b_IsMoving");
        Vector3 dir = _navmeshAgent.velocity + gameObject.transform.position;
        Vector3 lookAt = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
        gameObject.transform.LookAt(lookAt);
        if ((gameObject.transform.position - go_TargetedEnemy.transform.position).sqrMagnitude < GetRange() * GetRange())
        {
            b_TargetedEnemy = true;
            _animator.ResetTrigger("b_IsMoving");
            b_MovingToEnemy = false;
        }
    }

    public void ShowSelected()
    {

    }

    public void StopAllActions()
    {
        PUS = PlayerUnitState.PUS_GUARD;
        _animator.ResetTrigger("b_IsAttacking");
        _animator.ResetTrigger("b_IsMoving");
        _animator.ResetTrigger("b_isHarvesting");
        b_Moving = false;
        b_TargetedEnemy = false;
        _navmeshAgent.speed = 0f;
    }

    //public Transform[] getPath()
    //{
    //    return getpath;
    //}

    void IgnoreCollision()
    {
        GameObject go_PlayerUnitList = GameObject.FindGameObjectWithTag("PlayerList");
        foreach (Transform go_PULChild in go_PlayerUnitList.transform)
        {
            Physics.IgnoreCollision(go_PULChild.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }

    void OnClick()
    {
        go_CommandMenu.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_CommandMenu.GetComponent<CreateActionButton>().CreateButtons();
        go_CommandMenu.SetActive(true);
    }

    void SnapToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
           transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
           // _navMeshAgent.Warp(lastpos);

        }
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
            return i_AmountOfButtons = 3;
        }
        else
        {
            return i_AmountOfButtons = 2;
        }
    }

    public float GetSpeed()
    {
        return f_speed;
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