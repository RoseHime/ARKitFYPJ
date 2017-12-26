using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    enum EnemyUnitState {
           EUS_GUARD,
           EUS_ATTACK,
           EUS_MOVE,
           EUS_HARVEST,
           EUS_MAX_STATES
    };

    public float f_health = 50;
    public float f_range = 0.5f;
    public float f_atkRange = 0.02f;
    public float f_speed = 0.01f;
    private Transform T_playerList;
    //private GameObject[] goList_playerList;
    private GameObject go_LockOnPlayerUnit;
    EnemyUnitState EUS;

    RaycastHit rcHit;
    Vector3 rcHitPosition;
    private float f_distanceY;
    private Vector3 offset_Y;



    // Use this for initialization
    void Start () {
        T_playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;
        EUS = EnemyUnitState.EUS_GUARD;
	}
	
	// Update is called once per frame
	void Update () {

        CheckWhetherStillOnGround();
        if (Physics.Raycast(gameObject.GetComponent<Transform>().transform.position, Vector3.down, out rcHit))
        {
            rcHitPosition = rcHit.point;
            f_distanceY = gameObject.GetComponent<Transform>().transform.position.y - rcHitPosition.y;
            offset_Y = new Vector3(0, f_distanceY, 0);
        }

        switch (EUS)
        {
            case EnemyUnitState.EUS_MOVE:
                break;

            case EnemyUnitState.EUS_ATTACK:
                {
                    AttackPlayerUnit();
                    break;
                }

            case EnemyUnitState.EUS_GUARD:
                {
                    DetectPlayerUnit();
                    break;
                }
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

    void DetectPlayerUnit()
    {
        foreach (Transform go_playerUnitChild in T_playerList)
        {
            go_LockOnPlayerUnit = go_playerUnitChild.gameObject;
            float f_distanceCheck = (go_playerUnitChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude;
            if (f_distanceCheck < f_range*f_range)
            {
                EUS = EnemyUnitState.EUS_ATTACK;
                Debug.Log("Enemy Saw U");
            }
        }
    }

    void AttackPlayerUnit()
    {
        gameObject.GetComponent<Transform>().LookAt(go_LockOnPlayerUnit.GetComponent<Transform>().position);
        float f_distanceCheck = (go_LockOnPlayerUnit.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position).sqrMagnitude;
        if (f_distanceCheck >= f_atkRange && f_distanceCheck <= f_range*f_range)
            gameObject.GetComponent<Transform>().position = Vector3.MoveTowards(gameObject.GetComponent<Transform>().position, go_LockOnPlayerUnit.GetComponent<Transform>().position, f_speed * Time.deltaTime);
        else if (f_distanceCheck > f_range*f_range)
        {
            Debug.Log("ENemy lost sight of u");
            EUS = EnemyUnitState.EUS_GUARD;
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
}
