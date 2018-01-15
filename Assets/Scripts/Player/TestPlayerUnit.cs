using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestPlayerUnit : MonoBehaviour {
    NavMeshAgent navMesh;
    public int buttonCount = 4;

    public float f_health = 10;
    public float f_speed = 1;
    public float f_range = 1;

    // Use this for initialization
    void Start() {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        SnapToGround();
        LookDirection();
    }

    public void SetDestination(Vector3 target)
    {
        //Debug.Log("Destination:" + target);
        navMesh.SetDestination(target);
    }

    void SnapToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), -Vector3.up);
        if (GameObject.FindGameObjectWithTag("Terrain").transform.GetComponent<Collider>().Raycast(ray, out hit, float.MaxValue))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    void LookDirection()
    {
        Vector3 look = navMesh.velocity + transform.position;
        look.y = transform.position.y;
        transform.LookAt(look);
    }
}
