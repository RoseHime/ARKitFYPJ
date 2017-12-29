using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineBehaviour : BuildingInfo
{

    private int i_goldDistributed = 1;

    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public int CollectGold()
    {
        return i_goldDistributed;
    }

    void OnTouchDown()
    {
        Transform go_commandPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0);
        go_commandPanel.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_commandPanel.GetComponent<CreateActionButton>().CreateButtons();
        go_commandPanel.gameObject.SetActive(true);
    }

   //public override string GetUnitsInfo()
   //{
   //    string unitInfo = "NAME:" + gameObject.name + "\n";
   //    unitInfo += "GOLD" + i_goldDistributed + "\nRATE:" + (1);
   //    return unitInfo;
   //}
}
