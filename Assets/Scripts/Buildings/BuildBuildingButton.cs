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
        GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>().go_TowerPrefab = buildingPrefab;
        ti.b_BuildTower = true;
        transform.parent.parent.gameObject.SetActive(false);
    }

    public void offClick()
    {
        ti.b_TargetChose = true;
    }
}
