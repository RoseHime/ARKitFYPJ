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
}
