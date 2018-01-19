using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour {

    NavMeshSurface surface;

	// Use this for initialization
	void Start () {
        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
