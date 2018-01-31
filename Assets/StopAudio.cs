using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAudio : MonoBehaviour {

    private GameObject go_Audio;

	// Use this for initialization
	void Start () {
        go_Audio = GameObject.FindGameObjectWithTag("AudioSource").gameObject;
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        go_Audio.GetComponent<Audio>().StopMusic();
    }
}
