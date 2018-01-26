using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CreateUnitButton : MonoBehaviour {

    public GameObject go_unitPrefab;
    PlayerInfo playerInfo;

    private ButtonControl bc;

    // Use this for initialization
    void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();

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
                GameObject unit = createEntitiy.BuildPlayerUnit(transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.position);
                //unit.transform.position = Vector3.MoveTowards(unit.transform.position, transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position, unit.GetComponent<PlayerUnitBehaviour>().GetSpeed() * Time.deltaTime);
                //unit.GetComponent<PlayerUnitBehaviour>().SetTargetPos(transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position);
                unit.GetComponent<NavMeshAgent>().SetDestination(transform.parent.parent.parent.parent.GetComponent<BarracksPanelInfo>().go_SelectedBarracks.transform.GetChild(0).position);
                //unit.GetComponent<PlayerUnitBehaviour>().b_Moving = true;
                //transform.parent.parent.parent.parent.gameObject.SetActive(false);
                //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchInput>().b_Cancelled = true;
                //GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>().SetBackToSelect();
            }
        }
    }
}
