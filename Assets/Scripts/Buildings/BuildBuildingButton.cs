using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuildingButton : MonoBehaviour {

    private GameObject go_MainCamera;
    private TouchInput ti;

    public GameObject buildingPrefab;

    void Start()
    {
        go_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ti = go_MainCamera.GetComponent<TouchInput>();
    }

    public void onClick()
    {
        ti.go_towerPrefab = buildingPrefab;
        ti.b_BuildTower = true;
        ti.b_TargetChose = true;
        transform.parent.parent.gameObject.SetActive(false);
    }
}
