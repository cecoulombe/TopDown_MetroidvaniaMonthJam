using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicControl : MonoBehaviour
{
    // make a class which can control the volume of the music and sounds from a menu and that will have the changes carry through between scenes

    public Slider volumeSlider;
    public GameObject ObjectMusic;


    // value from the slider and converts it to volume level
    private float MusicVolume = 0f;
    private AudioSource AudioSource;

    private void Start()
    {
        ObjectMusic = GameObject.FindWithTag("DepressionMusic");
        AudioSource = ObjectMusic.GetComponent<AudioSource>();

        //set the volume

        MusicVolume = PlayerPrefs.GetFloat("volume");
        AudioSource.volume = MusicVolume;
        volumeSlider.value = MusicVolume;
    }

    private void Update()
    {
        AudioSource.volume = MusicVolume;
        PlayerPrefs.SetFloat("volume", MusicVolume);
    }

    public void VolumeUpdater(float volume)
    {
        MusicVolume = volume;
    }
}
