using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public float f_health = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DeathCheck();
	}

    void DeathCheck()
    {
        if (f_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
