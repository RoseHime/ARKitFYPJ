using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointConnector : MonoBehaviour
{
    public Transform[] childsT;
    //private Transform[] createPath;
    List<Transform> createPath = new List<Transform>();
    private int i;

    GameObject currentWaypoint;
    GameObject nextWaypoint;
    public GameObject go_TargetWaypoint;
    GameObject go_CurrentWaypoint;
    Vector3 v3_NextWaypoint;
    private int i_WaypointCounter;
    private int i_EndPointCounter;

    private GameObject current_holder;
    private GameObject temp;
    private GameObject temp2;
    private GameObject temp3;


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

        i_WaypointCounter = 0;
        i_EndPointCounter = 1;
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

    public void SetWayPointPath(Vector3 currpos, Vector3 lastpos)
    {
       

       
    }

    //Find closest waypoint to move to it first
    public void SetCurrentClosestWaypoint(Vector3 pos)
    {
        i_WaypointCounter = 0;
        //First find the closest waypoint nearest to the player
        //Transform[] nearestPoint = new Transform[i];
        List<Transform> nearestTran = new List<Transform>();
        foreach (Transform t_child in transform)
        {
            float f = (t_child.position - pos).sqrMagnitude;
            float dist = Vector3.Distance(pos, t_child.position);
            if (dist <= 0.2f)
            {
                nearestTran.Add(t_child);
            }
        }

        //Now set one of the closest waypoint nearest to the player
        int j = 0;
        while (j < nearestTran.Count)
        {
            if (j > 0)
            {
                if ((pos - nearestTran[j].position).sqrMagnitude < (pos - nearestTran[j - 1].position).sqrMagnitude)
                {
                    temp = nearestTran[j].gameObject;
                }
                if (j == nearestTran.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    current_holder = temp;
                    createPath.Add(temp.transform);
                    i_WaypointCounter += 1;
                }
            }
            else
            {
                temp = nearestTran[j].gameObject;
                if (j == nearestTran.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    current_holder = temp;
                    createPath.Add(temp.transform);
                    i_WaypointCounter += 1;
                }
            }
            j++;
        }
    }

    //Get where the end target waypoint is at.
    public void FindTargetClosestWaypoint(Vector3 pos)
    {
        //Find the closest waypoint to the lastpos
        List<Transform> nearestEnd = new List<Transform>();
        foreach (Transform t_child in transform)
        {
            float f = (t_child.position - pos).sqrMagnitude;
            float dist = Vector3.Distance(pos, t_child.position);
            if (dist <= 0.1f)
            {
                nearestEnd.Add(t_child);
            }
        }

        //Now Set one of the nearest as endpoint
        int end = 0;
        while (end < nearestEnd.Count)
        {
            if (end > 0)
            {
                if ((pos - nearestEnd[end].position).sqrMagnitude < (pos - temp2.transform.position).sqrMagnitude)
                {
                    temp2 = nearestEnd[end].gameObject;
                }
                if (end == nearestEnd.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    go_TargetWaypoint = temp2;
                }
            }
            else
            {
                temp2 = nearestEnd[end].gameObject;
                if (end == nearestEnd.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    go_TargetWaypoint = temp2;
                }
            }
            end++;
        }
    }

    public GameObject GetTargetClosestWaypoint()
    {
        return go_TargetWaypoint;
    }



    public void FindNextWaypoint()
    {
        //Now find the next waypoint closest to the current one while goigjn toward last point

        //int np = 0;
        //Transform[] nextPoint = new Transform[np];
        List<Transform> nearestPoint = new List<Transform>();
            int iTemp = 0;
            //Get the closest to the one currently
            foreach (Transform t_child in transform)
            {
                if (t_child.GetComponent<Collider>().bounds.Intersects(current_holder.GetComponent<Collider>().bounds))
                {
                    nearestPoint.Add(t_child);
                }
            }

        //Register the closest one
        while (iTemp < nearestPoint.Count)
        {
            if (iTemp > 0)
            {
               if ((GetTargetClosestWaypoint().transform.position - nearestPoint[iTemp].position).sqrMagnitude < (GetTargetClosestWaypoint().transform.position - temp3.transform.position).sqrMagnitude)
                {
                    temp3 = nearestPoint[iTemp].gameObject;
                }
                if (iTemp == nearestPoint.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    current_holder = temp3;
                    createPath.Add(temp3.transform);
                    i_WaypointCounter += 1;
                }
            }
            else
            {
                temp3 = nearestPoint[iTemp].gameObject;
                if (iTemp == nearestPoint.Count - 1)
                {
                    //createPath[0] = temp.transform;
                    current_holder = temp3;
                    createPath.Add(temp3.transform);
                    i_WaypointCounter += 1;
                }
            }
            iTemp++;
        }

       
    }

    public Vector3 MoveToNextWaypoint()
    {
        return v3_NextWaypoint;
    }

    public List<Transform> getCreatePath()
    {
        return createPath;
    }

    public int GetCounter()
    {
        return i_WaypointCounter + i_EndPointCounter;
    }
}
