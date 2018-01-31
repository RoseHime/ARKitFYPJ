using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour {

    AudioSource audio;
    public AudioClip clip1;
    public AudioClip clip2;

    private PlayerInfo pi;
    private bool b_NextSong;

    Scene currentScene;
    string sceneName;

    // Use this for initialization
    void Start () {

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        
        if (sceneName == "UnityARKitScene")
            pi = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();

        b_NextSong = false;
        audio = GetComponent<AudioSource>();
        audio.clip = clip1;
        audio.Play();

        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update() {

        if (sceneName == "UnityARKitScene")
        {
            if (!b_NextSong && pi.i_playerLevel == 2)
            {
                b_NextSong = true;
                audio.Stop();
                audio.clip = clip2;
                audio.Play();
            }
        }
	}

    public void StopMusic()
    {
        audio.Stop();
    }

}
