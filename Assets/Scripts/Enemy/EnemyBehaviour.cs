﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float f_range = 0.5f;
    public float f_atkRange = 0.1f;
    public float f_speed = 0.01f;
    public float f_bulletSpeed = 0.1f;
    public float f_damage = 1;
    public float f_fireRate = 1;
    private float f_fireCooldown = 0;
    public float f_defendRange = 1.0f;

    private GameObject bullet_Prefab;
    private Transform T_playerList;
    private GameObject go_LockOnUnit;
    public EnemyUnitState EUS = EnemyUnitState.EUS_IDLE;
    public EnemyType ET = EnemyType.ET_RANGED;

    public Vector3 destination;

    bool isMoving = false;
    public bool isDefending = false;

    NavMeshAgent _navMeshAgent;
    //NavMeshObstacle _navMeshOb;

    // Use this for initialization
    void Start() {
        T_playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;     
        if (ET == EnemyType.ET_RANGED)   
            bullet_Prefab = transform.GetChild(0).gameObject;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_navMeshOb = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update() {
        SnapToGround();
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
        if (f_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    GameObject DetectPlayerUnit()
    {
        float tempDist = float.MaxValue;

        GameObject nearestUnit = null;

        foreach (Transform go_playerUnitChild in T_playerList)
        {
            if ((go_playerUnitChild.position - transform.position).sqrMagnitude < tempDist)
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
            Vector3 difference = go_LockOnUnit.transform.position - transform.position;

            if (difference.sqrMagnitude < f_atkRange * f_atkRange)
            {
                EUS = EnemyUnitState.EUS_ATTACK;
                _navMeshAgent.ResetPath();
                //_navMeshOb.enabled = true;
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
                    //_navMeshAgent.enabled = false;
                    //_navMeshOb.enabled = true;
                }
            }
            else
            {
                //transform.position += difference.normalized * Time.deltaTime * f_speed;
                // _navMeshAgent.enabled = true;
                _navMeshAgent.ResetPath();
               _navMeshAgent.SetDestination(go_LockOnUnit.transform.position);
               //_navMeshOb.enabled = false;
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
                //_navMeshAgent.enabled = false;
                //_navMeshOb.enabled = true;
            }
        }

    }

    void Attack()
    {
        if (go_LockOnUnit != null)
        {
            Vector3 difference = go_LockOnUnit.transform.position - transform.position;

            if (difference.sqrMagnitude < f_atkRange * f_atkRange)
            {
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
            EUS = EnemyUnitState.EUS_IDLE;
        }
    }

    void Move()
    {
        isMoving = true;
        Vector3 offset = destination - transform.position;
        offset.y = 0;
        if (offset.sqrMagnitude < 0.05 * 0.05)
        {
            EUS = EnemyUnitState.EUS_IDLE;
            isMoving = false;
            _navMeshAgent.ResetPath();
            //_navMeshAgent.enabled = false;
            //_navMeshOb.enabled = true;
            //Debug.Log("ITS TIME TO STOP");
        }
        else
        {
            //transform.position += offset.normalized * Time.deltaTime * f_speed;
            //_navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(destination);
            //_navMeshOb.enabled = false;
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
        tempBullet.transform.position = gameObject.transform.position;
        tempBullet.transform.localScale = bullet_Prefab.transform.lossyScale;
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = f_damage;
        bullet_behaviour.direction = direction;

        tempBullet.SetActive(true);
    }

    void AttackUnit(GameObject target)
    {
        if (target.tag == "PlayerUnit")
        {
            target.transform.GetComponent<PlayerUnitBehaviour>().f_HealthPoint -= f_damage;
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
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.05f, transform.position.z);
        }
    }

    void Defend()
    {
        _navMeshAgent.SetDestination(destination);
        if ((transform.position - destination).sqrMagnitude < 0.1f * 0.1f)
        {
            EUS = EnemyUnitState.EUS_IDLE;
        }
    }
}
