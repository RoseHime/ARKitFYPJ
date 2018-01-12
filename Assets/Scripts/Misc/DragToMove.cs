using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEngine.XR.iOS
{
    public class DragToMove : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                //Handle movement based on touch phase.
                switch (touch.phase)
                {
                    //Record initial touch position
                    case TouchPhase.Began:
                        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.name == "Player")
                            {

                            }
                        }
                        break;

                    case TouchPhase.Moved:
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.name == "Player")
                            {
                                float distance_to_screen = Camera.main.WorldToScreenPoint(hit.transform.position).z;
                                Vector2 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                                ARPoint point = new ARPoint
                                {
                                    x = screenPosition.x,
                                    y = screenPosition.y
                                };
                                hit.transform.position = (Camera.main.ScreenToWorldPoint(new Vector3((float)point.x, (float)point.y, distance_to_screen)));
                            }
                        }
                        break;


                }
            }
        }
    }
}