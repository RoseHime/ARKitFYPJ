using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    enum EnemyUnitState {
           EUS_GUARD,
           EUS_ATTACK,
           EUS_MOVE,
           EUS_MAX_STATES
    };

    public float f_health = 50;
    public float f_range = 0.1f;
    public float f_atkRange = 0.02f;
    public float f_speed = 0.01f;
    private GameObject go_playerList;
    private GameObject[] goList_playerList;
    private GameObject go_LockOnPlayerUnit;
    EnemyUnitState EUS;

	// Use this for initialization
	void Start () {
        go_playerList = GameObject.FindGameObjectWithTag("PlayerUnit");
        EUS = EnemyUnitState.EUS_GUARD; ;
	}
	
	// Update is called once per frame
	void Update () {

        switch (EUS)
        {
            case EnemyUnitState.EUS_MOVE:
                break;

            case EnemyUnitState.EUS_ATTACK:
                break;

            case EnemyUnitState.EUS_GUARD:
                DetectPlayerUnit();
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

    void DetectPlayerUnit()
    {
        foreach (Transform go_playerUnitChild in go_playerList.transform)
        {
            float f_distanceCheck = (go_playerUnitChild.transform.position - gameObject.transform.position).sqrMagnitude;
            if (f_distanceCheck < f_range)
            {
                go_LockOnPlayerUnit = go_playerUnitChild.gameObject;
                gameObject.transform.LookAt(go_LockOnPlayerUnit.transform.position);
                EUS = EnemyUnitState.EUS_ATTACK;
            }
            else
            {
                EUS = EnemyUnitState.EUS_GUARD;
            }
        }
    }

    void AttackPlayerUnit()
    {
        float f_distanceCheck = (go_LockOnPlayerUnit.transform.position - gameObject.transform.position).sqrMagnitude;
        if (f_distanceCheck >= f_atkRange)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, go_LockOnPlayerUnit.transform.position, f_speed * Time.deltaTime);
    }
}
