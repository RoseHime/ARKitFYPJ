using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    private Transform go_GetUnit;

    private void Update()
    {
        GameObject go_PlayerUnitList = GameObject.FindGameObjectWithTag("Terrain");
        foreach (Transform go_PULChild in go_PlayerUnitList.transform)
        {
            Physics.IgnoreCollision(go_PULChild.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "PlayerUnit")
        //{
        //    go_GetUnit = other.gameObject;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerUnit")
        {
            go_GetUnit = collision.transform;
        }
    }

    public Transform getGO()
    {
        return go_GetUnit;
    }
}
