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
    private GameObject go_CommandMenu;
    private Rigidbody rb_Body;
    RaycastHit rcHit;
    Vector3 rcHitPosition;

    private Vector3 v3_currentPos;
    private Vector3 v3_targetPos;
    private float f_distanceY;
    private Vector3 offset_Y;

    public bool b_buildBuilding;
    public bool b_Moving;

    void Start()
    {
        rb_Body = gameObject.GetComponent<Rigidbody>();
        go_CommandMenu = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        go_CommandMenu.SetActive(false);
        b_Selected = false;
        b_Moving = false;
        b_buildBuilding = false;
    }

    void OnTouchDown()
    {
        go_CommandMenu.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_CommandMenu.GetComponent<CreateActionButton>().CreateButtons();
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
        b_Selected = false;
        b_Moving = true;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if ((v3_currentPos - (v3_targetPos + offset_Y)).magnitude > 0.01f)
        {
            Vector3 v3_seeTarget = new Vector3(v3_targetPos.x, gameObject.transform.position.y, v3_targetPos.z);
            gameObject.transform.LookAt(v3_seeTarget);
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
