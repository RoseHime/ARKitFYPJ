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

    GameObject go_Start;

    // Use this for initialization
    void Start () {
        go_Start = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(2).gameObject;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        
        b_NextSong = false;
        audio = GetComponent<AudioSource>();
        audio.clip = clip1;
        audio.Play();

        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update() {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("UnityARKitScene"))
        {
            pi = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();
            if (!b_NextSong && pi.i_playerLevel == 2)
            {
                b_NextSong = true;
                audio.Stop();
                audio.clip = clip2;
                audio.Play();
            }
        }

        //if(go_Start.GetComponent<ChangeScene>().nextScene)
        //{
        //    if (!b_NextSong && pi.i_playerLevel == 2)
        //    {
        //        b_NextSong = true;
        //        audio.Stop();
        //        audio.clip = clip2;
        //        audio.Play();
        //    }
        //}
	}

    public void StopMusic()
    {
        audio.Stop();
    }

}
