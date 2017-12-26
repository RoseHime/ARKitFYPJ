using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour {

    public int i_health = 50;

    private TowerBehaviour towerBehaviour;

	// Use this for initialization
	void Start () {
		if (transform.GetComponent<TowerBehaviour>() != null)
        {
            towerBehaviour = transform.GetComponent<TowerBehaviour>();
        }
	}

    public virtual string GetUnitsInfo()
    {
        string unitInfo;
        if (towerBehaviour != null)
        {         
            unitInfo = "HP:" + i_health + "\nDMG" + towerBehaviour.f_damage + "\nRANGE" + towerBehaviour.f_range + "\nSPD" + towerBehaviour.f_fireRate;         
        }
        else
        {
            unitInfo = "HP:" + i_health;
        }
        return unitInfo;
    }

    void OnTouchDown()
    {
        Transform go_commandPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0);
        go_commandPanel.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_commandPanel.GetComponent<CreateActionButton>().CreateButtons();
        go_commandPanel.gameObject.SetActive(true);
    }
}
