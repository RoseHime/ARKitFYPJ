﻿using System.Collections;
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

    float f_bulletTimer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Transform nearestEnemy = null;

        float tempDistance = f_range * f_range;
        foreach (Transform child in enemyList.transform)
        {
            if ((child.position - transform.position).sqrMagnitude < tempDistance)
            {
                tempDistance = (child.position - transform.position).sqrMagnitude;
                nearestEnemy = child;
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
        tempBullet.transform.SetParent(gameObject.transform);
        tempBullet.transform.position = gameObject.transform.position;
        tempBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        tempBullet.name = "TempBullet";

        BulletBehaviour bullet_behaviour = tempBullet.GetComponent<BulletBehaviour>();
        bullet_behaviour.f_speed = f_bulletSpeed;
        bullet_behaviour.f_damage = f_damage;
        bullet_behaviour.direction = direction;

        tempBullet.SetActive(true);
    }
}
