using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour {

    PlayerInfo playerInfo;


    public int i_UnitCapacity = 10;
    public int i_SeriousLevel = 2;

    private int i_unitLevelIncrement = 5;

    List<EnemySquad> attackingSquads;
    public List<GameObject> defendingUnits;

	// Use this for initialization
	void Start () {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
        defendingUnits = new List<GameObject>();
        attackingSquads = new List<EnemySquad>();

        EnemySquad squad = new EnemySquad();
        squad.Start();
        squad.i_maxUnits = 1;
        attackingSquads.Add(squad);
    }

    // Update is called once per frame
    void Update()
    {
        i_UnitCapacity = (playerInfo.i_playerLevel - 1) * i_unitLevelIncrement + 15;
       
        if (playerInfo.i_playerLevel >= i_SeriousLevel)
        {
            if (defendingUnits.Count > 16)
            {
                GameObject unit = defendingUnits[Random.Range(0, defendingUnits.Count)];
                if (attackingSquads[attackingSquads.Count - 1].unitList.Count < attackingSquads[attackingSquads.Count - 1].i_maxUnits)
                {
                    attackingSquads[attackingSquads.Count - 1].unitList.Add(unit);                   
                }
                else
                {
                    EnemySquad squad = new EnemySquad();
                    squad.Start();
                    squad.i_maxUnits = playerInfo.i_playerLevel;
                    attackingSquads.Add(squad);
                    attackingSquads[attackingSquads.Count - 1].unitList.Add(unit);
                }
                defendingUnits.Remove(unit);
            }
        }

        UpdateAttackSquads();
    }

    void UpdateAttackSquads()
    {
        foreach (EnemySquad squad in attackingSquads)
        {
            if (squad.unitList.Count >= squad.i_maxUnits && squad.pathToFollow.Count == 0)
            {
                Transform Waypoints = transform.GetChild(0);
                int random = Random.Range(0, 3);
                foreach (Transform point in Waypoints.GetChild(random))
                {
                    squad.pathToFollow.Add(point.position);
                }
            }

            squad.Update();
            
        }
    }
}
