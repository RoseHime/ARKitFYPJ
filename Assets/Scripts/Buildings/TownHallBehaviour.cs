using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHallBehaviour : BuildingInfo {

    PlayerInfo playerInfo;

    // Use this for initialization
    void Start() {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        f_maxHealth = f_health;
        f_previousHealth = f_health;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public void StoreStone(int stone)
    {
        playerInfo.i_stone += stone;
    }

    public void StoreWood(int wood)
    {
        playerInfo.i_wood += wood;
    }

    public void StoreMagicstone(int magicstone)
    {
        playerInfo.i_magicStone += magicstone;
    }

    public override string GetUnitsInfo()
    {
        string unitInfo = "NAME:" + gameObject.name + "\n";
        unitInfo += "HP:" + f_health;
        unitInfo += "\nLVL:" + GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>().i_playerLevel;
        return unitInfo;
    }
}
