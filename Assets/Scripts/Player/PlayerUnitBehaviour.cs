using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitBehaviour : MonoBehaviour
{

    private Transform T_Enemy;
    public Color detectedColor;

    // Use this for initialization
    void Start()
    {
        T_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Transform T_enemyChild in T_Enemy)
        {
            if ((T_enemyChild.position - gameObject.GetComponent<Transform>().position).sqrMagnitude <= gameObject.GetComponent<PlayerUnitUpdate>().GetRange())
            {
                //DO something
                gameObject.GetComponent<Renderer>().material.color = detectedColor;
            }

        }
    }
}