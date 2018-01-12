using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWaypoint : MonoBehaviour {

    private GameObject go_Parent;

    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    private Transform origin;
    private Transform destination;

    private float lineDrawSpeed = 0.01f;

    private int i;
    private int j;

    private void Start()
    {
        go_Parent = GameObject.FindGameObjectWithTag("MoveParent");

        lineRenderer = GetComponent<LineRenderer>();
        origin = GetComponent<Transform>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        i = 0;
        j = 1;
    }

    private void Update()
    {
        foreach (Transform t_child in go_Parent.transform)
        {
            if (t_child == this.gameObject.transform)
                continue;
            else
            {
                if (t_child.GetComponent<Collider>().bounds.Intersects(this.GetComponent<Collider>().bounds))
                {
                    lineRenderer.SetPosition(i, origin.position);
                    destination = t_child.transform;
                    dist = Vector3.Distance(origin.position, destination.position);

                    if (counter < dist)
                    {
                        counter += 0.01f / lineDrawSpeed;

                        float x = Mathf.Lerp(0, dist, counter);

                        Vector3 pointA = origin.position;
                        Vector3 pointB = destination.position;

                        Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                        lineRenderer.SetPosition(j, pointAlongLine);
                    }
                }
            }
            i++;
            j++;
        }

     
        //}
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.collider.tag == "MoveWaypoint")
    //    {
    //        destination = collision.transform;
    //        dist = Vector3.Distance(origin.position, destination.position);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "MoveWaypoint")
    //    {
    //        destination = collision.transform;
    //        dist = Vector3.Distance(origin.position, destination.position);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<Collider>().tag == "MoveWaypoint")
    //    {
    //        destination = other.transform;
    //        dist = Vector3.Distance(origin.position, destination.position);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Collider>().tag == "MoveWaypoint")
    //    {
    //        destination = other.transform;
    //        dist = Vector3.Distance(origin.position, destination.position);
    //    }
    //}
}
