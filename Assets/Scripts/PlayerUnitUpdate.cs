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
    private Material mat;
    private Transform tran;
    private Collider coll;
    Vector3 _playerPos;
    bool toMove;

    public GameObject go_Land;

    //Command Variable
    bool b_UnitIsSeleceted;
    public GameObject go_CommandMenu;
    private GameObject go_MainCamera;
    private TouchInput ti;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    void Start()
    {
        b_UnitIsSeleceted = false;
        mat = GetComponent<Renderer>().material;
        tran = GetComponent<Transform>();
        go_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ti = go_MainCamera.GetComponent<TouchInput>();
        go_CommandMenu.SetActive(false);
        toMove = false;
    }

    void OnTouchDown()
    {
        if (!b_UnitIsSeleceted)
        {
            go_CommandMenu.SetActive(true);
            b_UnitIsSeleceted = true;
        }
    }
    //void OnTouchUp()
    //{
    //    mat.color = defaultColour;
    //}
    //void OnTouchStay()
    //{
    //    mat.color = selectedColour;
    //}
    //void OnTouchExit()
    //{
    //    mat.color = defaultColour;
    //}

    private void Update()
    {
        if (go_CommandMenu.activeSelf == false)
        {
            b_UnitIsSeleceted = false;
        }

        if (ti.b_TargetChose)
        {
            mat.color = selectedColour;
            tran.position = Vector3.MoveTowards(tran.position, ti.rayHitTarget(), Time.deltaTime);
            if (tran.position == ti.rayHitTarget())
                ti.b_TargetChose = false;
        }
        else if (!ti.b_TargetChose)
        {
            mat.color = defaultColour;
        }

        CheckWhetherStillOnGround();
    }

    //[MenuItem("Tools/Transfrom Tools/Align %t")]
    //static void AlignToGround()
    //{
    //    Transform[] transforms = Selection.transforms;
    //    foreach(Transform myTransform in transforms)
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(myTransform.position, -Vector3.up, out hit))
    //        {
    //            Vector3 targetPosition = hit.point;
    //            if (myTransform.GetComponent<MeshFilter>() != null)
    //            {
    //                Bounds bounds = myTransform.GetComponent<MeshFilter>().sharedMesh.bounds;
    //                targetPosition.y += bounds.extents.y;
    //            }
    //            myTransform.position = targetPosition;
    //            Vector3 targetRotation = new Vector3(hit.normal.x, myTransform.eulerAngles.y, hit.normal.z);
    //            myTransform.eulerAngles = targetRotation;
    //        }
    //    }
    //}

    void CheckWhetherStillOnGround()
    {
        float distance_between = go_Land.transform.position.y - gameObject.transform.position.y;
        if (((go_Land.transform.position.y - gameObject.transform.position.y) > distance_between) ||
            ((go_Land.transform.position.y - gameObject.transform.position.y) < distance_between))
        {
            tran.position.Set(tran.position.x, tran.position.y - distance_between, tran.position.z);
        }
    }

    public bool isSelected()
    {
        if (b_UnitIsSeleceted)
            return true;
        return false;
    }
}
