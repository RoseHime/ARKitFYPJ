using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

    private AudioSource _audioSource;
    private Slider _slider;

	// Use this for initialization
	void Start () {
        _audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        _slider = gameObject.GetComponent<Slider>();
        _slider.value = _audioSource.volume * 100;
	}
	
	// Update is called once per frame
	void Update () {
        _audioSource.volume = _slider.value / 100;
	}


}
