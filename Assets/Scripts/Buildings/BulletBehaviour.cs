using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public float f_damage = 0;
    public float f_speed = 10;
    public Vector3 direction;

    public float f_lifetime = 5;

    float f_lifetimeCounter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        MoveBullet();
	}

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Enemy")
        {
            collisionInfo.transform.GetComponent<EnemyBehaviour>().f_health -= f_damage;
            Destroy(gameObject);
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
