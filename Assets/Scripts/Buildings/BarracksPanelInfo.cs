﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksPanelInfo : MonoBehaviour {

    public GameObject go_SelectedBarracks;
    private PlayerInfo playerInfo;

    public GameObject go_lvl2Block;
    public GameObject go_lvl3Block;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();		
	}
	
	// Update is called once per frame
	void Update () {
        if (go_SelectedBarracks != null)
        {
            if (go_SelectedBarracks.GetComponent<BarracksBehaviour>().i_barrackLevel > 1)
            {
                go_lvl2Block.SetActive(false);
            }
            else
            {
                go_lvl2Block.SetActive(true);
            }
            if (go_SelectedBarracks.GetComponent<BarracksBehaviour>().i_barrackLevel > 2)
            {
                go_lvl3Block.SetActive(false);
            }
            else
            {
                go_lvl3Block.SetActive(true);
            }
        }
    }
}
