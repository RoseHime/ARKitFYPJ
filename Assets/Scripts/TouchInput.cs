using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

    public LayerMask touchInputMask;
    public LayerMask GameLayerMask;
    //private List<GameObject> touchList = new List<GameObject>();
    //private GameObject[] touchesOld;
    private RaycastHit hit;
    Vector3 v3_rayPointTarget;
    public bool b_TargetChose;

    private void Start()
    {
        b_TargetChose = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (!b_TargetChose)
        {
            if (Input.touchCount > 0)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    Debug.Log(hit.point);

                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
        else if (b_TargetChose)
        {
            PickTargetPoint();
        }
	}

    public void PickTargetPoint()
    {
        if (Input.touchCount > 0)
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit, 100.0f, GameLayerMask))
            {
                v3_rayPointTarget = hit.point;
                Debug.Log(hit.point);
                b_TargetChose = false;
            }
        }
    }

    public Vector3 rayHitTarget()
    {
        return v3_rayPointTarget;
    }
}
