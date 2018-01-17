using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : BuildingInfo
{

    public int i_woodDistributed = 1;
    public int i_totalAmountOfWood = 2000;

    // Use this for initialization
    void Start()
    {

    }


    public int CollectWood()
    {
        if (i_totalAmountOfWood >= i_woodDistributed)
        {
            i_totalAmountOfWood -= i_woodDistributed;
        }
        else
        {
            i_woodDistributed = 0;
        }
        return i_woodDistributed;
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
        unitInfo += "WOOD" + i_woodDistributed;
        unitInfo += "\nTotal Wood:" + i_totalAmountOfWood;
        return unitInfo;
    }
}
