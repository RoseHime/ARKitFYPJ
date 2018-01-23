using System.Collections;
using UnityEngine;

public class Audio : MonoBehaviour {

    AudioSource audio;
    public AudioClip clip1;
    public AudioClip clip2;

    private PlayerInfo pi;
    private bool b_NextSong;

	// Use this for initialization
	void Start () {
        pi = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfo>();

        b_NextSong = false;
        audio = GetComponent<AudioSource>();
        audio.clip = clip1;
        audio.Play();

        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update () {

        if (!b_NextSong && pi.i_playerLevel == 2)
        {
            b_NextSong = true;
            audio.Stop();
            audio.clip = clip2;
            audio.Play();
        }
	}
}
