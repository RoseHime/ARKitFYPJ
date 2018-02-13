using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour {

    public enum EnemyUnitState {
        EUS_MOVE,
        EUS_IDLE,
        EUS_CHASE,
        EUS_DEFEND,
        EUS_ATTACK
    };

    public enum EnemyType
    {
        ET_RANGED,
        ET_MELEE,
        ET_TOTAL
    }

    public float f_health = 50;
    private float f_maxHealth;
    private float f_previousHealth;

    public float f_range = 0.5f;
    public float f_atkRange = 0.1f;
    public float f_speed = 0.01f;
    public float f_bulletSpeed = 0.1f;
    public float f_damage = 1;
    public float f_fireRate = 1;
    private float f_fireCooldown = 0;
    public float f_defendRange = 1.0f;

    public int i_magicStoneDrop = 1;

    public GameObject bullet_Prefab;
    private Transform T_playerList;
    private GameObject go_LockOnUnit;
    public EnemyUnitState EUS = EnemyUnitState.EUS_IDLE;
    public EnemyType ET = EnemyType.ET_RANGED;

    public Vector3 destination;

    bool isMoving = false;
    public bool isDefending = false;
    public bool isDead = false;

    NavMeshAgent _navMeshAgent;

    Animator _animator;

    private Image healthbar;

    public float f_healthBarDisplayTime = 5;
    float f_healthBarDisplayTimer = 0;
    bool isAttacked = false;
    float f_deathAnimationTimer = 0.0f;
    float f_deathTimer = 0.0f;

    // Use this for initialization
    void Start() {
        T_playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;     

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = f_speed;

        _animator = gameObject.GetComponent<Animator>();
        f_previousHealth = f_health;
        f_maxHealth = f_health;

        healthbar = transform.Find("HP").GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (isDead)
        {
            DeathAnimation();
        }
        else
        {
            SnapToGround();
        }
        
        switch (EUS)
        {
            case EnemyUnitState.EUS_MOVE:
                {
                    Move();
                }
                break;
            case EnemyUnitState.EUS_IDLE:
                {
                    Idle();
                }
                break;
            case EnemyUnitState.EUS_CHASE:
                {
                    Chase();
                }
                break;
            case EnemyUnitState.EUS_DEFEND:
                {
                    Defend();
                }
                break;
            case EnemyUnitState.EUS_ATTACK:
                {
                    Attack();
                }
                break;
        }

        DeathCheck();
    }

    void DeathCheck()
    {
        if (f_health < f_previousHealth)
        {
            f_previousHealth = f_health;
            isAttacked = true;
            f_healthBarDisplayTimer = 0;
        }
        if (isAttacked)
        {
            if ((f_healthBarDisplayTimer += Time.deltaTime) >= f_healthBarDisplayTime)
            {
                isAttacked = false;
                healthbar.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                healthbar.transform.parent.gameObject.SetActive(true);
            }
        }

        healthbar.fillAmount = f_health / f_maxHealth;

        if (f_health <= 0)
        {
            _animator.SetTrigger("b_IsDead");
            GetComponent<NavMeshAgent>().enabled = false;
            GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().i_magicStone += i_magicStoneDrop;
            isDead = true;
        }
    }

    void DeathAnimation()
    {
        if ((f_deathAnimationTimer += Time.deltaTime) > 1.5)
        {
            transform.position -= new Vector3(0, 0.025f, 0) * Time.deltaTime;
            f_deathTimer += Time.deltaTime;
            if (f_deathTimer > 2)
            {
                Destroy(gameObject);
            }
        }
    }

    GameObject DetectPlayerUnit()
    {
        float tempDist = float.MaxValue;

        GameObject nearestUnit = null;

        foreach (Transform go_playerUnitChild in T_playerList)
        {
            if ((go_playerUnitChild.position - transform.position).sqrMagnitude < tempDist && go_playerUnitChild && go_playerUnitChild.GetComponent<PlayerUnitInfo>().f_HealthPoint > 0)
            {
                tempDist = (go_playerUnitChild.position - transform.position).sqrMagnitude;
                nearestUnit = go_playerUnitChild.gameObject;
            }
        }

        foreach (Transform building in GameObject.FindGameObjectWithTag("BuildingList").transform)
        {
            if ((building.position - transform.position).sqrMagnitude < tempDist)
            {
                tempDist = (building.position - transform.position).sqrMagnitude;
                nearestUnit = building.gameObject;
            }
        }

        return nearestUnit;
    }


    void Idle()
    {
        GameObject tempPlayer = DetectPlayerUnit();
        _animator.ResetTrigger("b_IsAttacking");
        _animator.ResetTrigger("b_IsMoving");
        if (tempPlayer != null)
        {
            if ((tempPlayer.transform.position - transform.position).sqrMagnitude <= f_range * f_range)
            {
                go_LockOnUnit = tempPlayer;
                EUS = EnemyUnitState.EUS_CHASE;
            }
            else if (isMoving)
            {
                EUS = EnemyUnitState.EUS_MOVE;
            }
        }
        else if (isMoving)
        {
            EUS = EnemyUnitState.EUS_MOVE;
        }
    }


    void Chase()
    {
        if (go_LockOnUnit != null)
        {
            _animator.SetTrigger("b_IsMoving");
            _animator.ResetTrigger("b_IsAttacking");
            Vector3 difference = go_LockOnUnit.transform.position - transform.position;

            if (difference.sqrMagnitude < f_atkRange * f_atkRange)
            {
                EUS = EnemyUnitState.EUS_ATTACK;
                _navMeshAgent.ResetPath();
            }
            else if (difference.sqrMagnitude > f_range * f_range || ((transform.position - destination).sqrMagnitude > f_defendRange * f_defendRange && isDefending))
            {
                if (isDefending)
                {
                    EUS = EnemyUnitState.EUS_DEFEND;
                }
                else
                {
                    EUS = EnemyUnitState.EUS_IDLE;
                    _navMeshAgent.ResetPath();
                }
            }
            else
            {
                //transform.position += difference.normalized * Time.deltaTime * f_speed;
                LookDirection();
                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(go_LockOnUnit.transform.position);                
            }
        }
        else
        {
            if (isDefending)
            {
                EUS = EnemyUnitState.EUS_DEFEND;
            }
            else
            {
                EUS = EnemyUnitState.EUS_IDLE;
                _navMeshAgent.ResetPath();

            }
        }

    }

    void Attack()
    {
        if (go_LockOnUnit != null)
        {
            if (go_LockOnUnit.GetComponent<PlayerUnitInfo>().f_HealthPoint > 0)
            {
                Vector3 difference = go_LockOnUnit.transform.position - transform.position;

                if (difference.sqrMagnitude < f_atkRange * f_atkRange)
                {
                    _animator.SetTrigger("b_IsAttacking");
                    Vector3 look = go_LockOnUnit.transform.position;
                    look.y = transform.position.y;
                    transform.LookAt(look);
                    if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
                    {
                        f_fireCooldown = 0;
                        if (ET == EnemyType.ET_RANGED)
                            FireBullet(difference.normalized);
                        else
                        {
                            AttackUnit(go_LockOnUnit);
                        }
                    }
                }
                else
                {
                    EUS = EnemyUnitState.EUS_CHASE;
                }
            }
            else
            {
                go_LockOnUnit = null;
                EUS = EnemyUnitState.EUS_IDLE;
            }
        }
        else
        {
            EUS = EnemyUnitState.EUS_IDLE;
        }
    }

    void Move()
    {
        _animator.SetTrigger("b_IsMoving");
        isMoving = true;
        Vector3 offset = destination - transform.position;
        offset.y = 0;
        if (offset.sqrMagnitude < 0.05 * 0.05)
        {
            EUS = EnemyUnitState.EUS_IDLE;
            isMoving = false;
            //_navMeshAgent.ResetPath();
        }
        else
        {
            //transform.position += offset.normalized * Time.deltaTime * f_speed;
            _navMeshAgent.SetDestination(destination);
            LookDirection();
        }

        GameObject tempPlayer = DetectPlayerUnit();

        if (tempPlayer != null)
        {
            if ((tempPlayer.transform.position - transform.position).sqrMagnitude <= f_range * f_range)
            {
                go_LockOnUnit = tempPlayer;
                EUS = EnemyUnitState.EUS_CHASE;
            }
        }
    }

    void FireBullet(Vector3 direction)
    {
        GameObject tempBullet = Instantiate(bullet_Prefab);
        tempBullet.transform.SetParent(GameObject.FindGameObjectWithTag("BulletPool").transform);
        tempBullet.transform.position = gameObject.transform.position + new Vector3(0, GetComponent<Collider>().bounds.size.y/2, 0);
        tempBullet.transform.localScale = bullet_Prefab.transform.lossyScale;
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = f_damage;
        bullet_behaviour.direction = direction;
        bullet_behaviour.target = BulletBehaviour.BULLETTARGET.PLAYER;

        tempBullet.SetActive(true);
    }

    void AttackUnit(GameObject target)
    {
        if (target.tag == "PlayerUnit")
        {
            target.transform.GetComponent<PlayerUnitInfo>().f_HealthPoint -= f_damage;
            target.transform.GetComponent<PlayerFSM>().b_IsAttacked = true;

        }
        else if (target.transform.parent == GameObject.FindGameObjectWithTag("BuildingList").transform)
        {
            target.transform.GetComponent<BuildingInfo>().f_health -= f_damage;
        }
    }

    void SnapToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0,1,0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray,out hit,float.MaxValue))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            //_navMeshAgent.Warp(new Vector3(transform.position.x, hit.point.y, transform.position.z));
            //_navMeshAgent.updatePosition = true;
        }
    }

    void Defend()
    {
        _navMeshAgent.SetDestination(destination);
        LookDirection();
        Vector3 difference = destination - transform.position;
        //transform.position += difference.normalized * Time.deltaTime * f_speed;
        if (difference.sqrMagnitude < 0.1f * 0.1f)
        {
            EUS = EnemyUnitState.EUS_IDLE;
            _navMeshAgent.ResetPath();
        }
    }

    void LookDirection()
    {
        Vector3 look = _navMeshAgent.velocity + transform.position;
        look.y = transform.position.y;
        transform.LookAt(look);
    }
}
