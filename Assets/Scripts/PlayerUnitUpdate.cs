using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerUnitUpdate : MonoBehaviour
{
    //Unit Individual Info
    public int i_HealthPoint;
    public float f_speed;


    public Color defaultColour;
    public Color selectedColour;
    public bool b_Selected;
    public GameObject go_CommandMenu;
    private Rigidbody rb_Body;
    RaycastHit rcHit;
    Vector3 rcHitPosition;

    private Vector3 v3_currentPos;
    private Vector3 v3_targetPos;
    private float f_distanceY;
    private Vector3 offset_Y;


    public bool b_Moving;

    void Start()
    {
        rb_Body = gameObject.GetComponent<Rigidbody>();
        go_CommandMenu.SetActive(false);
        b_Selected = false;
        b_Moving = false;
    }

    void OnTouchDown()
    {
        go_CommandMenu.SetActive(true);
    }

    private void Update()
    {
        CheckWhetherStillOnGround();
        if (Physics.Raycast(gameObject.GetComponent<Transform>().transform.position, Vector3.down, out rcHit))
        {
            rcHitPosition = rcHit.point;
            f_distanceY = gameObject.GetComponent<Transform>().transform.position.y - rcHitPosition.y;
            offset_Y = new Vector3(0, f_distanceY, 0);
        }

        if (b_Selected)
            gameObject.GetComponent<Renderer>().material.color = selectedColour;
        else if (!b_Selected)
        {
            gameObject.GetComponent<Renderer>().material.color = defaultColour;
        }

        if (b_Moving)
        {
            rb_Body.isKinematic = false;
            MoveToTargetPos();
        }
        else
        {
            rb_Body.isKinematic = true;
        }
    }

    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        b_Moving = true;
        b_Selected = false;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if (v3_currentPos != (v3_targetPos + offset_Y))
        {
            gameObject.transform.position = Vector3.MoveTowards(v3_currentPos, v3_targetPos + offset_Y, f_speed * Time.deltaTime);
        }
        else
        {
            b_Moving = false;
        }
    }

    void CheckWhetherStillOnGround()
    {
        if ((gameObject.GetComponent<Transform>().transform.position.y - rcHitPosition.y) != f_distanceY)
        {
            gameObject.GetComponent<Transform>().transform.position.Set(gameObject.GetComponent<Transform>().transform.position.x, 
                                                                        rcHitPosition.y + f_distanceY, 
                                                                        gameObject.GetComponent<Transform>().transform.position.z);
        }
    }
}
