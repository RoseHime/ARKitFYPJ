using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineBehaviour : BuildingInfo {

    public int i_goldDistributed = 1;

    public float f_cooldown = 1;
    private float f_timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (f_timer < f_cooldown)
        {
            f_timer += Time.deltaTime;
        }
	}

    public bool CollectGold()
    {
        if (f_timer >= f_cooldown)
        {
            f_timer = 0;
            return true;
        }
        return false;
    }

    void OnTouchDown()
    {
        Transform go_commandPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0);
        go_commandPanel.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_commandPanel.GetComponent<CreateActionButton>().CreateButtons();
        go_commandPanel.gameObject.SetActive(true);
    }

    public override string GetUnitsInfo()
    {
        string unitInfo = "NAME:" + gameObject.name + "\n";
        unitInfo += "GOLD" + i_goldDistributed + "\nRATE:" + (1 / f_cooldown);
        return unitInfo;
    }
}
