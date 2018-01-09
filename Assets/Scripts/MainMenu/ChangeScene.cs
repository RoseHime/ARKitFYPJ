using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string destinationScene;

	public void onClick(bool additive)
    {
        if (!additive)
            SceneManager.LoadScene(destinationScene);
        else
            SceneManager.LoadScene(destinationScene, LoadSceneMode.Additive);
    }
}
