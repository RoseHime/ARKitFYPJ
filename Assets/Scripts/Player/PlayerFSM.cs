using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFSM : MonoBehaviour {

    /* Unit Target */
    Vector3 v3_targetPos;
    Vector3 v3_CurrentTreeHarvestingPos;

    /* Unit Bool */
    public bool b_Selected;                        // To check if unit is selected
    bool IsSelected()
    {
        if (b_Selected)     // if unit is selected
            return true;    // return true;
        return false;       // else it false;
    }

    /* Worker Only */
    public bool b_buildBuilding;                   // For worker, if to build structure     
    public GameObject go_BuildingPrefab;           // For worker, to get the structure prefab


    /* Unit Data*/
    private NavMeshAgent _navmeshAgent;                             //Unit Navmesh Component
    public NavMeshAgent getAgent() { return _navmeshAgent; }

    private Animator _animator;                                     //Unit Animator Component
    public Animator getAnimator() { return _animator; }

    /* Find Object */
    private GameObject go_CommandMenu;                          //Unit Menu
    public GameObject getCommandMenu() { return go_CommandMenu; }

    private Transform T_EnemyUnit;                              //Enemy Unit List
    Transform getEnemyUnitList() { return T_EnemyUnit; }

    private Transform T_EnemyBase;                              //Enemy Base List
    Transform getEnemyBaseList() { return T_EnemyBase; }

    private GameObject go_TargetedEnemy;                        //Targeted Enemy
    GameObject getTargetedEnemy() { return go_TargetedEnemy; }

    private Transform T_Resource;
    Transform getResourceTarget() { return T_Resource; }        //Targeted Resource
    private bool b_isHarvsted;
    private bool b_isHarvesting;
    float f_HarvestingTime;
    private bool b_GetStone;
    private bool b_GetWood;
    int i_gotResourceStone;
    int i_gotResourceWood;

    private GameObject go_TownHall;                             //Own Town Hall
    GameObject getTownHall() { return go_TownHall; }

    /* Temp to store enemy */
    GameObject go_TempEnemyHolder;

    //Projectile for range unit
    public GameObject bullet_Prefab;
    public float f_bulletSpeed;
    public float f_fireRate;
    private float f_fireCooldown;

    // Use this for initialization
    void Start () {

        //Get Unit Component
        _navmeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();

        //Find GameObject
        go_CommandMenu = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        go_CommandMenu.SetActive(false);

        T_EnemyUnit = GameObject.FindGameObjectWithTag("EnemyList").transform;
        T_EnemyBase = GameObject.FindGameObjectWithTag("EnemyBuildingList").transform;

        //Set State
        b_Selected = false;
        go_TargetedEnemy = null;
        //gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_GUARD);
        gameObject.GetComponent<PlayerUnitInfo>().PUS = PlayerUnitInfo.PlayerUnitState.PUS_GUARD;
        gameObject.GetComponent<PlayerUnitInfo>().SetOriginSpeed(getAgent().speed);

        if (gameObject.GetComponent<PlayerUnitInfo>().GetUnitType() == PlayerUnitInfo.PlayerUnitType.PUN_WORKER)
        {
            b_isHarvsted = false;
            b_isHarvesting = false;
            f_HarvestingTime = 0;
            b_GetStone = false;
            b_GetWood = false;
            i_gotResourceStone = 0;
            i_gotResourceWood = 0;
        }

    }

    // Update is called once per frame
    void Update () {

        //Render a circle below the unit to show that it is selected
        RenderSelectionIcon();
        SnapToGround();

        if (gameObject.GetComponent<PlayerUnitInfo>().GetUnitState() == PlayerUnitInfo.PlayerUnitState.PUS_GUARD)
        {
            getAgent().avoidancePriority = 50;
        }

        //If unit's HP reach 0 or less.
        if (gameObject.GetComponent<PlayerUnitInfo>().GetUnitHealth() <= 0)
        {
            getCommandMenu().SetActive(false);
            Destroy(gameObject);
        }

    }

    //When unit is selected, show something to show that it is selected
    void RenderSelectionIcon()
    {
        if (IsSelected())
        {
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
    }

    //Snap unit to ground all the time
    void SnapToGround()
    {
        //Root object onto ground at all time
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            // _navMeshAgent.Warp(lastpos);

        }
    }

    // Give unit a target position to move to
    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        getAgent().speed = gameObject.GetComponent<PlayerUnitInfo>().GetOriginSpeed();

        gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_MOVE);
    }

    public void MoveToTargetPos()
    {
        getAgent().isStopped = false;
        getAgent().stoppingDistance = 0.05f;
        getAgent().avoidancePriority = 0;
        getAnimator().SetTrigger("b_IsMoving");

        //Find the direction
        Vector3 dir = getAgent().velocity + gameObject.transform.position;

        //Look at the direction
        Vector3 lookAtDir = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
        gameObject.transform.LookAt(lookAtDir);

        //If detected enemy while on the way
        if (getTargetedEnemy() != null)
            gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_ATTACK);

        //If reached target point
        if ((gameObject.transform.position - v3_targetPos).sqrMagnitude < getAgent().stoppingDistance * getAgent().stoppingDistance)
        {
            getAgent().avoidancePriority = 50;
            _animator.ResetTrigger("b_IsMoving");
            if (b_buildBuilding)
            {
                ConstructBuilding(v3_targetPos);
                b_buildBuilding = false;
            }
            gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_GUARD);
        }
        else //If not reached target point
            getAgent().SetDestination(v3_targetPos);
    }

    // Let all unit except worker to detect for enemy at all time
    public GameObject DetectEnemy()
    {
        // if there currently no targeted enemy, search for one until one come in range
        if (go_TargetedEnemy == null)
        {
            //Find all enemy within detect range and add to a list
            List<Transform> nearbyEnemy = new List<Transform>();
            nearbyEnemy.Clear();

                foreach (Transform T_enemyChild in T_EnemyUnit)
                {
                    if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= gameObject.GetComponent<PlayerUnitInfo>().GetUnitDetectRange())
                    {
                        nearbyEnemy.Add(T_enemyChild);
                        gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_ATTACK);
                        break;
                    }
                }
            foreach (Transform T_enemyChild in T_EnemyBase)
            {
                if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= gameObject.GetComponent<PlayerUnitInfo>().GetUnitDetectRange())
                {
                    nearbyEnemy.Add(T_enemyChild);
                    gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_ATTACK);
                    break;
                }
            }

            //Now find the closest enemy     
            int i_Enemy = 0;
            while (i_Enemy < nearbyEnemy.Count)
            {
                if (i_Enemy > 0)
                {
                    if ((gameObject.transform.position - nearbyEnemy[i_Enemy].position).sqrMagnitude < (gameObject.transform.position - go_TempEnemyHolder.transform.position).sqrMagnitude)
                    {
                        go_TempEnemyHolder = nearbyEnemy[i_Enemy].gameObject;
                    }
                    if (i_Enemy == nearbyEnemy.Count - 1)
                    {
                        go_TargetedEnemy = go_TempEnemyHolder;
                    }
                }
                else
                {
                    go_TempEnemyHolder = nearbyEnemy[i_Enemy].gameObject;
                    if (i_Enemy == nearbyEnemy.Count - 1)
                    {

                        go_TargetedEnemy = go_TempEnemyHolder;
                    }
                }
                i_Enemy++;
            }
        }
        return go_TargetedEnemy;
    }

    public void AttackEnemy()
    {
        if (go_TargetedEnemy != null)
        {
            getAgent().avoidancePriority = 0;
            //If enemy unit not within attack range
            if (go_TargetedEnemy.tag == "Enemy")
            {
                if ((go_TargetedEnemy.transform.position - gameObject.GetComponent<Transform>().position).sqrMagnitude >= gameObject.GetComponent<PlayerUnitInfo>().GetUnitAttackRange())
                {
                    getAgent().stoppingDistance = 0.02f;
                    getAgent().isStopped = false;
                    //Find the direction
                    Vector3 dir = getAgent().velocity + gameObject.transform.position;

                    //Look at the direction
                    Vector3 lookAtDir = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
                    gameObject.transform.LookAt(lookAtDir);

                    _animator.SetTrigger("b_IsMoving");
                    getAgent().SetDestination(go_TargetedEnemy.transform.position);
                }
                else
                {
                    getAgent().isStopped = true;
                    getAgent().avoidancePriority = 100;

                    //Look at the target
                    Vector3 lookAtEnemy = new Vector3(go_TargetedEnemy.transform.position.x, gameObject.transform.position.y, go_TargetedEnemy.transform.position.z);
                    gameObject.transform.LookAt(lookAtEnemy);
                    _animator.SetTrigger("b_IsAttacking");

                    //If it not a range unit
                    if (gameObject.GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_RANGE)
                    {
                        //Deal dmg
                        if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                        {
                            f_fireCooldown = 0;
                            //Enemy unit get damaged
                            go_TargetedEnemy.gameObject.GetComponent<EnemyBehaviour>().f_health -= gameObject.GetComponent<PlayerUnitInfo>().GetUnitAttackDmg();
                        }
                    }

                    //If the enemy is dead or destroyed or out or range
                    if ((go_TargetedEnemy.tag == "Enemy" && go_TargetedEnemy.GetComponent<EnemyBehaviour>().f_health <= 0) ||
                        (go_TargetedEnemy.transform.position - gameObject.GetComponent<Transform>().position).sqrMagnitude > gameObject.GetComponent<PlayerUnitInfo>().GetUnitDetectRange())
                    {
                        _animator.ResetTrigger("b_IsMoving");
                        _animator.ResetTrigger("b_IsAttacking");
                        //gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_GUARD);
                        gameObject.GetComponent<PlayerUnitInfo>().PUS = PlayerUnitInfo.PlayerUnitState.PUS_GUARD;
                        go_TargetedEnemy = null;
                    }
                }
            }
            //If enemy base is not within range
            else
            {
                if ((go_TargetedEnemy.transform.position - gameObject.GetComponent<Transform>().position).sqrMagnitude >= gameObject.GetComponent<PlayerUnitInfo>().GetBaseAttackRange())
                {
                    getAgent().stoppingDistance = 0.05f;
                    getAgent().isStopped = false;
                    //Find the direction
                    Vector3 dir = getAgent().velocity + gameObject.transform.position;

                    //Look at the direction
                    Vector3 lookAtDir = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
                    gameObject.transform.LookAt(lookAtDir);

                    _animator.SetTrigger("b_IsMoving");
                    getAgent().SetDestination(go_TargetedEnemy.transform.position);
                }
                else
                {
                    getAgent().isStopped = true;

                    //Look at the target
                    Vector3 lookAtEnemy = new Vector3(go_TargetedEnemy.transform.position.x, gameObject.transform.position.y, go_TargetedEnemy.transform.position.z);
                    gameObject.transform.LookAt(lookAtEnemy);
                    _animator.SetTrigger("b_IsAttacking");

                    //If it not a range unit
                    if (gameObject.GetComponent<PlayerUnitInfo>().GetUnitType() != PlayerUnitInfo.PlayerUnitType.PUN_RANGE)
                    {
                        //Deal dmg
                        if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                        {
                            f_fireCooldown = 0;
                            //Enemy base get damaged
                            if(go_TargetedEnemy.name != "Base")
                                go_TargetedEnemy.gameObject.GetComponent<BuildingInfo>().f_health -= gameObject.GetComponent<PlayerUnitInfo>().GetUnitAttackDmg();
                            else
                                go_TargetedEnemy.gameObject.GetComponent<TownHallBehaviour>().f_health -= gameObject.GetComponent<PlayerUnitInfo>().GetUnitAttackDmg();
                        }
                    }

                    //If the enemy is dead or destroyed or out or range
                    if ((go_TargetedEnemy.tag == "SelectableBuilding" && go_TargetedEnemy.GetComponent<BuildingInfo>().f_health <= 0) ||
                        (go_TargetedEnemy.name == "Base" && go_TargetedEnemy.GetComponent<TownHallBehaviour>().f_health <= 0) ||
                        (go_TargetedEnemy.transform.position - gameObject.GetComponent<Transform>().position).sqrMagnitude > gameObject.GetComponent<PlayerUnitInfo>().GetUnitDetectRange())
                    {
                        _animator.ResetTrigger("b_IsMoving");
                        _animator.ResetTrigger("b_IsAttacking");
                        //gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_GUARD);
                        gameObject.GetComponent<PlayerUnitInfo>().PUS = PlayerUnitInfo.PlayerUnitState.PUS_GUARD;
                        go_TargetedEnemy = null;
                    }
                }
            }
        }
        else
        {
            gameObject.GetComponent<PlayerUnitInfo>().PUS = PlayerUnitInfo.PlayerUnitState.PUS_GUARD;
        }
    }

    // Aim at an enemy unit
    public GameObject GetEnemyTargetPos(GameObject go_Enemy)
    {
        gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_ATTACK);
        return go_TargetedEnemy = go_Enemy;
    }

    //For worker only, to target on harvestable object
    public Transform GetBuildingTargetPos(Transform T_targetBuilding)
    {
        getAgent().speed = gameObject.GetComponent<PlayerUnitInfo>().GetOriginSpeed();
        gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_HARVEST);

        //Store the current tree pos
        v3_CurrentTreeHarvestingPos = T_targetBuilding.position;

        //Find your townhall for resource harvesting to deposit resource in.
        go_TownHall = GameObject.FindGameObjectWithTag("BuildingList");
        foreach (Transform go_PlayerBuilding in go_TownHall.transform)
        {
            if (go_PlayerBuilding.gameObject.name == "TownHall")
            {
                go_TownHall = go_PlayerBuilding.gameObject;
                break;
            }
        }

        return T_Resource = T_targetBuilding;
    }

    public void OnHarvestMode()
    {
        _animator.SetTrigger("b_IsMoving");

        //Hardcore stop distance for harvesting purpose =_=
        if (T_Resource.tag == "StoneMine")
            getAgent().stoppingDistance = 0.08f;
        else if (T_Resource.tag == "Tree")
            getAgent().stoppingDistance = 0.06f;

        getAgent().autoBraking = false;
        getAgent().avoidancePriority = 50;

        //Find the direction
        Vector3 dir = getAgent().velocity + gameObject.transform.position;

        //Look at the direction
        Vector3 lookAtDir = new Vector3(dir.x, gameObject.transform.position.y, dir.z);
        gameObject.transform.LookAt(lookAtDir);

        //Harvested or not
        if (!b_isHarvsted)
        {
            //Move to the resource position
            if ((T_Resource.position- gameObject.transform.position).sqrMagnitude < getAgent().stoppingDistance * getAgent().stoppingDistance)
            {
                getAgent().avoidancePriority = 0;
                b_isHarvesting = true;

                //Stop moving when reach
                Vector3 lookAtResource = new Vector3(T_Resource.position.x, gameObject.transform.position.y, T_Resource.position.z);
                getAgent().isStopped = true;

                gameObject.transform.LookAt(lookAtResource);
            }
            else
            {
                getAgent().SetDestination(T_Resource.position);
            }
        }
        else
        {
            //Move to townhall to store resource
            if ((go_TownHall.transform.position - gameObject.transform.position).sqrMagnitude > getAgent().stoppingDistance * getAgent().stoppingDistance)
            {
                getAgent().isStopped = false;
                getAgent().SetDestination(go_TownHall.transform.position);
            }
            else
            {
                b_isHarvsted = false;
                if (b_GetStone)
                {
                    //Collected the stone
                    go_TownHall.GetComponent<TownHallBehaviour>().StoreStone(i_gotResourceStone);
                    i_gotResourceStone = 0;
                    b_GetStone = false;
                }
                else if (b_GetWood)
                {
                    //Collected the wood
                    go_TownHall.GetComponent<TownHallBehaviour>().StoreWood(i_gotResourceWood);
                    i_gotResourceWood = 0;
                    b_GetWood = false;
                }

            }
        }

        //If the resource turn to null, find the next one
        foreach (Transform GetTree in GameObject.FindGameObjectWithTag("Resources").transform)
        {
            if (T_Resource == null && (GetTree.position - v3_CurrentTreeHarvestingPos).sqrMagnitude < 0.005f)
            {
                if (GetTree.tag == "Tree")
                {
                    T_Resource = GetTree.transform;
                    v3_CurrentTreeHarvestingPos = T_Resource.transform.position;
                }
            }
        }
    }

    public void StartHarvesting()
    {
        if (b_isHarvesting)
        {
            _animator.SetTrigger("b_isHarvesting");
            f_HarvestingTime += Time.deltaTime;

            //Stop the animation and return to townhall
            if(f_HarvestingTime >= 2)
            {
                b_isHarvsted = true;

                //Check which resource you are harvesting from.
                if (T_Resource.tag == "StoneMine")
                {
                    b_GetStone = true;
                    i_gotResourceStone = T_Resource.GetComponent<StoneMineBehaviour>().CollectStone();
                }
                else if (T_Resource.tag == "Tree")
                {
                    b_GetWood = true;
                    i_gotResourceWood = T_Resource.GetComponent<TreeBehaviour>().CollectWood();
                }

                _animator.ResetTrigger("b_isHarvesting");
                f_HarvestingTime = 0;
                b_isHarvesting = false;
            }
        }
    }

    //Worker Only, for construct purpose
    public void ConstructBuilding(Vector3 pos)
    {
        GameObject prefab = GameObject.FindGameObjectWithTag("GameFunctions").gameObject;
        prefab.GetComponent<CreateEntities>().go_TowerPrefab = go_BuildingPrefab;
        prefab.GetComponent<CreateEntities>().BuildBuilding(pos);
    }

    //Stop all unit action when called
    public void StopAllActions()
    {
        gameObject.GetComponent<PlayerUnitInfo>().SetUnitState(PlayerUnitInfo.PlayerUnitState.PUS_GUARD);
        _animator.ResetTrigger("b_IsMoving");
        _animator.ResetTrigger("b_IsAttacking");
        _animator.ResetTrigger("b_isHarvesting");
        go_TargetedEnemy = null;
        getAgent().isStopped = true;
    }

    //Open up menu when unit is selected
    void OnClick()
    {
        go_CommandMenu.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_CommandMenu.GetComponent<CreateActionButton>().CreateButtons();
        go_CommandMenu.SetActive(true);
    }

    //Fire projectile for range unit
    void FireBullet(Vector3 direction)
    {
        GameObject tempBullet = Instantiate(bullet_Prefab);
        tempBullet.transform.SetParent(GameObject.FindGameObjectWithTag("BulletPool").transform);
        tempBullet.transform.position = gameObject.transform.position;
        tempBullet.transform.localScale = bullet_Prefab.transform.lossyScale;
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = gameObject.GetComponent<PlayerUnitInfo>().GetUnitAttackDmg();
        bullet_behaviour.direction = direction;
        bullet_behaviour.target = BulletBehaviour.BULLETTARGET.ENEMY;

        tempBullet.SetActive(true);
    }
}
