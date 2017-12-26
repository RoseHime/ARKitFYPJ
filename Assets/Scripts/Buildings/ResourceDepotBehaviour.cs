using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDepotBehaviour : MonoBehaviour {

    PlayerInfo playerInfo;

    // Use this for initialization
    void Start() {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void CollectGold(int gold)
    {
        playerInfo.i_gold += gold;
    }

    public void CollectWood(int wood)
    {
        playerInfo.i_wood += wood;
    }

    public void CollectMagicstone(int magicstone)
    {
        playerInfo.i_magicStone += magicstone;
    }
}
