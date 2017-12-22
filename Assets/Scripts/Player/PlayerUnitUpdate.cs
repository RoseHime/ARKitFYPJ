using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerUnitUpdate : MonoBehaviour
{
    //Unit Individual Info
    public int i_HealthPoint;
    public float f_speed;
    public float f_range;

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

    public bool b_buildBuilding;
    public bool b_Moving;
    private bool b_Rotating;

    void Start()
    {
        rb_Body = gameObject.GetComponent<Rigidbody>();
        go_CommandMenu.SetActive(false);
        b_Selected = false;
        b_Moving = false;
        b_buildBuilding = false;
        b_Rotating = false;
    }

    void OnTouchDown()
    {
        go_CommandMenu.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
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

        if (b_Moving && !b_Rotating)
        {
            rb_Body.isKinematic = false;
            MoveToTargetPos();
        }
        else
        {
            rb_Body.isKinematic = true;
        }

        if (b_Rotating)
        {
            CheckAngleWithTarget();
            RotateTowardTarget();
        }
    }

    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
        b_Selected = false;
        b_Rotating = true;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if ((v3_currentPos - (v3_targetPos + offset_Y)).magnitude > 0.01f)
        {
            gameObject.transform.position = Vector3.MoveTowards(v3_currentPos, v3_targetPos + offset_Y, f_speed * Time.deltaTime);
        }
        else
        {
            if (b_buildBuilding)
            {
                GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>().BuildBuilding(v3_currentPos);
                b_buildBuilding = false;
            }
            b_Moving = false;
        }
    }

    void CheckAngleWithTarget()
    {
        float f_angle = Vector3.Angle(v3_targetPos, gameObject.transform.forward);
        if (f_angle <= 0.0f)
        {
            b_Moving = true;
            b_Rotating = false;
        }
    }

    void RotateTowardTarget()
    {
        Vector3 targetDir = (v3_targetPos + offset_Y) - v3_currentPos;
        float f_rotateSpeed = f_speed * Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(targetDir);
        //Vector3 v3_newDir = Vector3.RotateTowards(gameObject.transform.forward, targetDir, f_rotateSpeed, 0.0f);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, f_rotateSpeed);
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

    public float GetRange()
    {
        return f_range;
    }
    public Color GetDefault()
    {
        return defaultColour;
    }
}
