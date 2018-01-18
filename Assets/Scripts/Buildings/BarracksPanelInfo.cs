using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksPanelInfo : MonoBehaviour {

    public GameObject go_SelectedBarracks;

    public GameObject LVL2BLOCK;
    public GameObject LVL3BLOCK;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
        if (go_SelectedBarracks != null)
        {
            if (go_SelectedBarracks.GetComponent<BarracksBehaviour>().i_barrackLevel > 1)
            {
                LVL2BLOCK.SetActive(false);
            }
            else
            {
                LVL2BLOCK.SetActive(true);
            }
            if (go_SelectedBarracks.GetComponent<BarracksBehaviour>().i_barrackLevel > 2)
            {
                LVL3BLOCK.SetActive(false);
            }
            else
            {
                LVL3BLOCK.SetActive(true);
            }
        }
    }
}
