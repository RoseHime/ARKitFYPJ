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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        stoneText.text = "" + i_stone;
        woodText.text = "" + i_wood;
        magicStoneText.text = "" + i_magicStone;
    }
}
