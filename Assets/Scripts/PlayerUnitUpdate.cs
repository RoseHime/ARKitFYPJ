using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerUnitUpdate : MonoBehaviour
{
    public class PlayerUnit
    {
        public int i_HealthPoint;
    }

    public Color defaultColour;
    public Color selectedColour;
    public bool b_Selected;
    public GameObject go_CommandMenu;

    private Vector3 v3_currentPos;
    private Vector3 v3_targetPos;

    void Start()
    {
        go_CommandMenu.SetActive(false);
        b_Selected = false;
    }

    void OnTouchDown()
    {
        go_CommandMenu.SetActive(true);
    }

    private void Update()
    {
        if (b_Selected)
            gameObject.GetComponent<Renderer>().material.color = selectedColour;
        else if (!b_Selected)
            gameObject.GetComponent<Renderer>().material.color = defaultColour;

        MoveToTargetPos();
    }

    public void SetTargetPos(Vector3 v3_targetpos)
    {
        v3_targetPos = v3_targetpos;
    }

    private void MoveToTargetPos()
    {
        v3_currentPos = gameObject.transform.position;
        if (v3_currentPos != v3_targetPos)
        {
            gameObject.transform.position = Vector3.MoveTowards(v3_currentPos, v3_currentPos, Time.deltaTime);
        }
    }

    void CheckWhetherStillOnGround()
    {

    }
}
