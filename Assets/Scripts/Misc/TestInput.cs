using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

    public LayerMask layerMask;
    public GameObject selectedUnit;

    public bool b_MoveUnit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        EditorCameraMovement();
        HandleKeyboardInput();
#endif
        HandleTouchInput();
	}

    void HandleKeyboardInput()
    {
        if (Input.GetMouseButtonDown(0))  // if there is a left click on mouse
        {
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                GameObject recipient = hit.transform.gameObject;
                if (b_MoveUnit)
                {
                    if (selectedUnit.GetComponent<TestPlayerUnit>() != null)
                    {
                        selectedUnit.GetComponent<TestPlayerUnit>().SetDestination(hit.point);
                    }
                    b_MoveUnit = false;
                }
                else
                {
                    if (selectedUnit == null && (recipient.tag == "SelectableBuilding" || recipient.tag == "PlayerUnit"))
                    {
                        selectedUnit = recipient;
                        TestCreateActionButton actionScript = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<TestCreateActionButton>();
                        actionScript.go_selectedUnit = selectedUnit;
                        actionScript.CreateButtons();
                        actionScript.gameObject.SetActive(true);
                        //selectedUnit.SendMessage("OnClick",null,SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                GameObject recipient = hit.transform.gameObject;
                if (b_MoveUnit)
                {
                    if (selectedUnit.GetComponent<TestPlayerUnit>() != null)
                    {
                        selectedUnit.GetComponent<TestPlayerUnit>().SetDestination(hit.point);
                    }
                    b_MoveUnit = false;
                }
                else
                {
                    if (selectedUnit == null && (recipient.tag == "SelectableBuilding" || recipient.tag == "PlayerUnit"))
                    {
                        selectedUnit = recipient;
                        CreateActionButton actionScript = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<CreateActionButton>();
                        actionScript.go_selectedUnit = selectedUnit;
                        actionScript.CreateButtons();
                        actionScript.gameObject.SetActive(true);
                        //selectedUnit.SendMessage("OnClick",null,SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }

    void EditorCameraMovement()
    {
        float speed = 0.5f;

        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Camera.main.transform.forward * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position -= Camera.main.transform.forward * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position -= Camera.main.transform.right * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Camera.main.transform.right * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.R))
        {
            Camera.main.transform.position += Vector3.up * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.F))
        {
            Camera.main.transform.position -= Vector3.up * Time.deltaTime * speed;
        }
    }
}
