using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStructures : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildBuilding(GameObject prefab,Vector3 position)
    {
        GameObject tempBuilding = Instantiate(prefab);
        tempBuilding.transform.position = position;
        tempBuilding.name = "Tower";
    }
}
