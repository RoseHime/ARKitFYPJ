using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    public int i_stone;
    public int i_wood;
    public int i_magicStone;

    public Text stoneText;
    public Text woodText;
    public Text magicStoneText;

    public int i_playerLevel = 1;
    public int i_MaxUnitCapacity = 10;

    public int f_upgradeCost = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stoneText.text = "" + i_stone;
        woodText.text = "" + i_wood;
        magicStoneText.text = "" + i_magicStone;
    }

    public bool LevelUp()
    {
        if (i_stone >= f_upgradeCost)
        {
            i_stone -= f_upgradeCost;
            i_playerLevel++;
            return true;
        }
        return false;
    }
}
