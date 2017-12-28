using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour {

    public float f_health = 50;

    private TowerBehaviour towerBehaviour;

    // Use this for initialization
    void Start () {
        if (transform.GetComponent<TowerBehaviour>() != null)
        {
            towerBehaviour = transform.GetComponent<TowerBehaviour>();
        }
	}

    void Update ()
    {
        if (f_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual string GetUnitsInfo()
    {
        string unitInfo = "NAME:" + gameObject.name + "\n";
        if (towerBehaviour != null)
        {         
            unitInfo += "HP:" + f_health + "\nDMG" + towerBehaviour.f_damage + "\nRANGE" + towerBehaviour.f_range + "\nSPD" + towerBehaviour.f_fireRate;         
        }
        else
        {
            unitInfo += "HP:" + f_health;
        }
        return unitInfo;
    }

    void OnClick()
    {
        Transform go_commandPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0);
        go_commandPanel.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_commandPanel.GetComponent<CreateActionButton>().CreateButtons();
        go_commandPanel.gameObject.SetActive(true);
    }
}
