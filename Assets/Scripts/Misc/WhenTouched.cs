using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhenTouched : MonoBehaviour {

    public Color defaultColour;
    public Color selectedColour;
    private Material mat;
    private GameObject _playerUnit;
    GameObject playerGhost;
    Vector3 _playerPos;
    Vector3 targetPos;
    bool toMove;

    //Command Variable
    bool b_UnitIsSeleceted;
    public GameObject go_CommandMenu;

    void Start()
    {
        b_UnitIsSeleceted = false;
        mat = GetComponent<Renderer>().material;
        _playerUnit = GetComponent<GameObject>().gameObject;
        toMove = false;

        go_CommandMenu.SetActive(false);
    }

    void OnTouchDown()
    {
        mat.color = selectedColour;
        Vector3 touchPosition = Input.GetTouch(0).position;
        targetPos = touchPosition;

        if (!b_UnitIsSeleceted)
        {
            go_CommandMenu.SetActive(true);
            b_UnitIsSeleceted = true;
        }
        toMove = false;
    }
    void OnTouchUp()
    {
        mat.color = defaultColour;
        //Destroy(playerGhost, 1);
        toMove = true;
        MoveToDestination();
    }
    void OnTouchStay()
    {
        mat.color = selectedColour;

        //Vector3 touchPosition = Input.GetTouch(0).position;
        ////float distance_to_screen = Camera.main.WorldToScreenPoint(playerGhost.transform.position).z;
        //Physics.IgnoreCollision(playerGhost.GetComponent<Collider>(), _playerUnit.GetComponent<Collider>());
        //
        //playerGhost.transform.Translate(touchPosition.x, touchPosition.y, 0);
        //targetPos = (Camera.main.ScreenToWorldPoint(touchPosition));

    }
    void OnTouchExit()
    {
        mat.color = defaultColour;
    }

    private void MoveToDestination()
    {
        if(toMove)
            _playerUnit.transform.Translate(targetPos);
    }

    private void Update()
    {
        if (go_CommandMenu.activeSelf == false)
        { 
            b_UnitIsSeleceted = false;
        }
    }

    public bool isSelected()
    {
        if (b_UnitIsSeleceted)
            return true;
        return false;
    }
}
