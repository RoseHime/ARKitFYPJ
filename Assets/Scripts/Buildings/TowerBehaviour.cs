using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

    public float f_fireRate = 1;
    public bool b_active = true;
    public float f_damage = 1;
    public float f_bulletSpeed = 0.1f;
    public float f_range = 0.5f;

    public GameObject bullet_Prefab;

    public GameObject enemyList;
    public GameObject playerList;

    float f_bulletTimer = 0;

    public enum TOWERTYPE
    {
        PLAYER,
        ENEMY
    }

    public TOWERTYPE type = TOWERTYPE.PLAYER;

	// Use this for initialization
	void Start () {
        enemyList = GameObject.FindGameObjectWithTag("EnemyList");
        playerList = GameObject.FindGameObjectWithTag("PlayerList");
    }
	
	// Update is called once per frame
	void Update () {
        Transform nearestEnemy = null;

        float tempDistance = f_range * f_range;

        if (type == TOWERTYPE.PLAYER)
        {
            foreach (Transform child in enemyList.transform)
            {
                if ((child.position - transform.position).sqrMagnitude < tempDistance)
                {
                    tempDistance = (child.position - transform.position).sqrMagnitude;
                    nearestEnemy = child;
                }
            }
        }
        else
        {
            foreach (Transform child in playerList.transform)
            {
                if ((child.position - transform.position).sqrMagnitude < tempDistance)
                {
                    tempDistance = (child.position - transform.position).sqrMagnitude;
                    nearestEnemy = child;
                }
            }
        }

        if (nearestEnemy != null)
        {
            if ((f_bulletTimer += Time.deltaTime) > 1/f_fireRate)
            {
                Vector3 direction = (nearestEnemy.position - transform.position).normalized;

                FireBullet(direction);
                f_bulletTimer = 0;
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

        if (type == TOWERTYPE.PLAYER)
        {
            tempBullet.GetComponent<BulletBehaviour>().target = BulletBehaviour.BULLETTARGET.ENEMY;
        }
        else
        {
            tempBullet.GetComponent<BulletBehaviour>().target = BulletBehaviour.BULLETTARGET.PLAYER;
        }

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = f_damage;
        bullet_behaviour.direction = direction;

        tempBullet.SetActive(true);
    }
}
