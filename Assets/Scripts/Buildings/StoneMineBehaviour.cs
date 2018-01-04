using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMineBehaviour : BuildingInfo
{

    public int i_stoneDistributed = 1;
    public int i_totalAmountOfStones = 2000;

    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public int CollectStone()
    {
        if (i_totalAmountOfStones >= i_stoneDistributed)
        {
            i_totalAmountOfStones -= i_stoneDistributed;
        }
        else
        {
            i_stoneDistributed = 0;
        }
        return i_stoneDistributed;
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
        unitInfo += "STONE" + i_stoneDistributed;
        unitInfo += "\nTotal Stones:" + i_totalAmountOfStones;
        return unitInfo;
    }
}
