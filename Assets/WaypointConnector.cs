using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointConnector : MonoBehaviour
{
    Transform[] childsT;
    private int i;

    GameObject currentWaypoint;
    GameObject nextWaypoint;
    GameObject go_TargetWaypoint;
    GameObject go_CurrentWaypoint;
    Vector3 v3_NextWaypoint;

    // Use this for initialization
    void Start()
    {
        childsT = new Transform[transform.childCount];
        i = 0;
        foreach(Transform t_child in transform)
        {
            childsT[i] = t_child.transform;
            i++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childsT.Length; i++)
        {
            currentWaypoint = childsT[i].gameObject;
            for (int j = 0; j < childsT.Length; j++)
            {
                if (j == i)
                { continue; }
                else
                {
                    nextWaypoint = childsT[j].gameObject;
                    if (childsT[i].GetComponent<Collider>().bounds.Intersects(childsT[j].GetComponent<Collider>().bounds))
                        Debug.DrawLine(childsT[i].transform.position, childsT[j].transform.position);
                }
            }
        }
    }
    
    public void FindCurrentClosestWaypoint(Vector3 pos)
    {
        foreach (Transform t_child in transform)
        {
            if ((t_child.position - pos).sqrMagnitude < 0.1f)
            {
                go_CurrentWaypoint = t_child.gameObject;
            }
        }
    }

    public GameObject GetCurrentClosestWaypoint()
    {
        return go_CurrentWaypoint;
    }

    public void FindTargetClosestWaypoint(Vector3 pos)
    {
        foreach (Transform t_child in transform)
        {
            if ((t_child.position - pos).sqrMagnitude < 0.1f)
            {
                go_TargetWaypoint = t_child.gameObject;
            }
        }
    }

    public GameObject GetTargetClosestWaypoint()
    {
        return go_TargetWaypoint;
    }

    public Vector3 MoveToNextWaypoint(GameObject lastWaypoint)
    {
        float distance = Mathf.Infinity;
        for (int i = 0; i < childsT.Length; i++)
        {
            if (childsT[i] == currentWaypoint)
                continue;
            else
            {
                if (childsT[i].GetComponent<Collider>().bounds.Intersects(currentWaypoint.GetComponent<Collider>().bounds))
                {
                    if ((childsT[i].position - lastWaypoint.transform.position).sqrMagnitude < distance)
                    {
                        v3_NextWaypoint = childsT[i].transform.position;
                    }
                }
            }
        }
        return v3_NextWaypoint;
    }
}
