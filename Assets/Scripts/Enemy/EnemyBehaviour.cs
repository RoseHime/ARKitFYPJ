using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    enum EnemyUnitState {
        EUS_IDLE,
        EUS_CHASE,
        EUS_ATTACK
    };

    public float f_health = 50;
    public float f_range = 0.5f;
    public float f_atkRange = 0.1f;
    public float f_speed = 0.01f;
    public float f_bulletSpeed = 0.1f;
    public float f_damage = 1;
    public float f_fireRate = 1;
    private float f_fireCooldown = 0;

    private GameObject bullet_Prefab;
    private Transform T_playerList;
    private GameObject go_LockOnPlayerUnit;
    EnemyUnitState EUS;

    // Use this for initialization
    void Start() {
        T_playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;
        EUS = EnemyUnitState.EUS_IDLE;
        bullet_Prefab = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update() {
        switch (EUS)
        {
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
            case EnemyUnitState.EUS_ATTACK:
                {
                    Attack();
                }
                break;
        }
        SnapToGround();
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

        return nearestUnit;
    }


    void Idle()
    {
        GameObject tempPlayer = DetectPlayerUnit();
        if ((tempPlayer.transform.position - transform.position).sqrMagnitude <= f_range * f_range)
        {
            go_LockOnPlayerUnit = tempPlayer;
            EUS = EnemyUnitState.EUS_CHASE;
        }
    }


    void Chase()
    {
        Vector3 difference = go_LockOnPlayerUnit.transform.position - transform.position;

        if (difference.sqrMagnitude < f_atkRange * f_atkRange)
        {
            EUS = EnemyUnitState.EUS_ATTACK;
        }
        else if (difference.sqrMagnitude > f_range)
        {
            EUS = EnemyUnitState.EUS_IDLE;
        }
        else
        {
            difference.y = 0;
            transform.position += difference.normalized * Time.deltaTime * f_speed;
        }
    }

    void Attack()
    {
        Vector3 difference = go_LockOnPlayerUnit.transform.position - transform.position;

        if (difference.sqrMagnitude < f_atkRange * f_atkRange)
        {
            if ((f_fireCooldown += Time.deltaTime) >= 1 / f_fireRate)
            {
                f_fireCooldown = 0;
                FireBullet(difference.normalized);
            }
        }
        else
        {
            EUS = EnemyUnitState.EUS_CHASE;
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

    void SnapToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0,1,0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray,out hit,float.MaxValue))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
        }
    }
}
