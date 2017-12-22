using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnitButton : MonoBehaviour {

    public GameObject go_unitPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (go_unitPrefab != null)
        {
            CreateEntities createEntitiy = GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>();
            createEntitiy.go_playerPrefab = go_unitPrefab;
            createEntitiy.BuildPlayerUnit(transform.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.position + new Vector3(0.05f,0,0));
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
