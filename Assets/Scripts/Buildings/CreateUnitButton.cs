using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUnitButton : MonoBehaviour {

    public GameObject go_unitPrefab;
    PlayerInfo playerInfo;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();

        transform.GetChild(0).GetComponent<Text>().text = go_unitPrefab.name + "\nCosts:";
        if (go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_woodCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nWood:" + go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_woodCost;
        }
        if (go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_stoneCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nStone:" + go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_stoneCost;
        }
        if (go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_magicStoneCost > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text += "\nMagicStone:" + go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_magicStoneCost;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (playerInfo.i_stone >= go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_stoneCost && playerInfo.i_wood >= go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_woodCost 
            && playerInfo.i_magicStone >= go_unitPrefab.GetComponent<PlayerUnitBehaviour>().i_magicStoneCost)
        {
            if (go_unitPrefab != null)
            {
                CreateEntities createEntitiy = GameObject.FindGameObjectWithTag("GameFunctions").GetComponent<CreateEntities>();
                createEntitiy.go_playerPrefab = go_unitPrefab;
                createEntitiy.BuildPlayerUnit(transform.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position);
                transform.parent.parent.gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchInput>().b_Cancelled = true;
                GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().SetBackToSelect();
            }
        }
    }
}
