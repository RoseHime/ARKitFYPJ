using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODScript : MonoBehaviour {

    public Mesh LowQuality = null;
    public Mesh MedQuality = null;
    public Mesh HighQuality = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float distance = (Camera.main.transform.position - transform.position).magnitude;
        PlayerInfo playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        if (distance < playerInfo.f_LODHighQuality)
        {
            GetComponent<MeshFilter>().mesh = HighQuality;
        }
        else if (distance < playerInfo.f_LODMedQuality)
        {
            GetComponent<MeshFilter>().mesh = MedQuality;
        }
        else
        {
            GetComponent<MeshFilter>().mesh = LowQuality;
        }
	}
}
