using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour {

    bool gameisPaused;
    public GameObject go_pauseMenu;

    public void Start()
    {
        gameisPaused = false;
    }

    public void TriggerPause()
    {
        if (!gameisPaused)
        {
            go_pauseMenu.SetActive(true);
            gameisPaused = true;

        }
        else if (gameisPaused)
        {
            go_pauseMenu.SetActive(false);
            gameisPaused = false;
        }

    }

}
