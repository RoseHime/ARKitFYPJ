using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public float f_damage = 0;
    public float f_speed = 10;
    public Vector3 direction;

    public float f_lifetime = 5;

    float f_lifetimeCounter = 0;

    public enum BULLETTARGET
    {
        PLAYER,
        ENEMY,
    }

    public BULLETTARGET target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //print("Hid was here!");
        MoveBullet();
	}

    void OnCollisionEnter(Collision collisionInfo)
    {
        //Debug.Log("Detected Collision");
        switch (target)
        {
            case BULLETTARGET.ENEMY:
                if (collisionInfo.gameObject.tag == "Enemy")
                {
                    collisionInfo.transform.GetComponent<EnemyBehaviour>().f_health -= f_damage;
                    Destroy(gameObject);
                }
                else if(collisionInfo.gameObject.transform.parent == GameObject.FindGameObjectWithTag("EnemyBuildingList").transform)
                {
                    collisionInfo.transform.GetComponent<BuildingInfo>().f_health -= f_damage;
                    Destroy(gameObject);
                }
                break;
            case BULLETTARGET.PLAYER:         
                if (collisionInfo.gameObject.tag == "PlayerUnit")
                {
                    //Debug.Log("IT HIT:" + collisionInfo.transform.name);
                    collisionInfo.transform.GetComponent<PlayerUnitBehaviour>().f_HealthPoint -= f_damage;
                    Destroy(gameObject);
                    
                }
                else if (collisionInfo.gameObject.transform.parent == GameObject.FindGameObjectWithTag("BuildingList").transform)
                {
                    collisionInfo.transform.GetComponent<BuildingInfo>().f_health -= f_damage;
                    Destroy(gameObject);
                }
                break;
        }

    }

    void MoveBullet()
    {
        if ((f_lifetimeCounter += Time.deltaTime) < f_lifetime)
        {
            gameObject.transform.position += direction * f_speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
