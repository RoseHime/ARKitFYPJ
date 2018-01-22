using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCrossHair : MonoBehaviour
{

    public GameObject go_Crosshair;
    private RaycastHit hit;
    public LayerMask touchInputMask;
    public Texture2D getImage;
    public Texture2D origin;
    public Texture2D T2D_HarvestTree_Icon;
    public Texture2D T2D_HarvestMine_Icon;
    public Texture2D T2D_Attack_Icon;
    private ButtonControl bc;

    // Use this for initialization
    void Start()
    {
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(transform.position);
        if (Physics.Raycast(ray, out hit, float.MaxValue, touchInputMask))
        {
            GameObject recipient = hit.collider.gameObject;

            //debugText.text = recipient.name + "\n" + hit.point;
            if (recipient != null)
            {
                if (recipient.tag == "PlayerUnit" || recipient.tag == "SelectableBuilding")
                    this.GetComponent<RawImage>().texture = getImage;
                else if (recipient.tag == "Tree" && bc.b_IsWorker)
                    this.GetComponent<RawImage>().texture = T2D_HarvestTree_Icon;
                else if (recipient.tag == "StoneMine" && bc.b_IsWorker)
                    this.GetComponent<RawImage>().texture = T2D_HarvestMine_Icon;
                else if (recipient.GetComponentInParent<Transform>().tag == "EnemyList" || recipient.GetComponentInParent<Transform>().tag == "EnemyBuildingList")
                    this.GetComponent<RawImage>().texture = T2D_Attack_Icon;
                else
                    this.GetComponent<RawImage>().texture = origin;
            }
        }
    }
}