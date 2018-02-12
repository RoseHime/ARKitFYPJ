using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string destinationScene;
    public bool nextScene;

	public void onClick(bool additive)
    {
        nextScene = true;
        if (!additive)
            SceneManager.LoadScene(destinationScene);
        else
            SceneManager.LoadScene(destinationScene, LoadSceneMode.Additive);
    }

    public void UnloadScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
}
