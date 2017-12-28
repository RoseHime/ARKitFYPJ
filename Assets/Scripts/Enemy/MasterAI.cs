using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour {

    public enum STRATEGY
    {
        PINCER,
        TURTLE,
    }

    public STRATEGY masterStrategy;

    public List<EnemySpawnerBehaviour> spawnerList;

    bool startStrategy = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (masterStrategy)
        {
            case STRATEGY.PINCER:
                PincerStrat();
                break;
            case STRATEGY.TURTLE:
                break;
        }	
	}

    void PincerStrat()
    {
        if (!startStrategy)
        {
            bool checkIfAllSpawned = true;
            for (int i = 0; i < 3; ++i)
            {
                if (spawnerList[i].localEnemyList.Count != spawnerList[i].i_maxUnits)
                {
                    checkIfAllSpawned = false;
                }
            }

            if (checkIfAllSpawned)
            {
                startStrategy = true;
            }
        }
        else
        {

        }

    }
}
