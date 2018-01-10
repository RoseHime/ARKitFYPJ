using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksBehaviour : BuildingInfo {

    PlayerInfo playerInfo;

    public int i_barrackLevel = 1;
    public int i_levelUpCost = 10;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        f_maxHealth = f_health;
        f_previousHealth = f_health;
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public bool LevelUp()
    {
        if (playerInfo.i_magicStone >= i_levelUpCost && playerInfo.i_playerLevel > i_barrackLevel)
        {
            playerInfo.i_magicStone -= i_levelUpCost;
            i_barrackLevel++;
            i_levelUpCost *= 2;
            return true;
        }
        return false;
    }


    public override string GetUnitsInfo()
    {
        string unitInfo = "NAME:" + gameObject.name + "\n";
        unitInfo += "HP:" + f_health;
        unitInfo += "\nLVL:" + i_barrackLevel;
        return unitInfo;
    }
}
