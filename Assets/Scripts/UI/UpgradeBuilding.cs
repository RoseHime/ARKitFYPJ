using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuilding : MonoBehaviour {

    public GameObject building = null;
    PlayerInfo playerInfo;

    GameObject oldBuilding = null;
    public Text textConfirmation;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
		if (oldBuilding != building)
        {
            oldBuilding = building;
            if (building.GetComponent<TownHallBehaviour>() != null)
            {
                if (playerInfo.i_playerLevel < playerInfo.i_maxLevel)
                {
                    textConfirmation.text = "Upgrade " + building.name + " for " + playerInfo.f_upgradeCost + " bones?";
                    transform.Find("Confirm").gameObject.SetActive(true);
                }
                else
                {
                    textConfirmation.text = "MAX LEVEL";
                    transform.Find("Confirm").gameObject.SetActive(false);
                }

            }
            else if (building.GetComponent<BarracksBehaviour>() != null)
            {
                if (building.GetComponent<BarracksBehaviour>().i_barrackLevel < playerInfo.i_playerLevel)
                {
                    textConfirmation.text = "Upgrade " + building.name + " for " + building.GetComponent<BarracksBehaviour>().i_levelUpCost + " bones?";
                    transform.Find("Confirm").gameObject.SetActive(true);
                }
                else
                {
                    if (building.GetComponent<BarracksBehaviour>().i_barrackLevel < playerInfo.i_maxLevel)
                    {
                        textConfirmation.text = "Please upgrade your town hall";
                        transform.Find("Confirm").gameObject.SetActive(false);
                    }
                    else
                    {
                        textConfirmation.text = "MAX LEVEL";
                        transform.Find("Confirm").gameObject.SetActive(false);
                    }

                }
            }
        }
	}

    public void OnClick()
    {
        if (building.GetComponent<TownHallBehaviour>() != null)
        {
            if (playerInfo.LevelUp())
            {
                gameObject.SetActive(false);
                oldBuilding = null;
            }
        }
        else if (building.GetComponent<BarracksBehaviour>() != null)
        {
            if (building.GetComponent<BarracksBehaviour>().LevelUp())
            {
                gameObject.SetActive(false);
                oldBuilding = null;
            }
        }
    }
}
