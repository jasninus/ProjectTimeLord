using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SetVolume : MonoBehaviour
{

    public AudioMixer Mixer;
    // Update is called once per frame
    public void SetVolumeLevel(float SliderValue)
    {
        Mixer.SetFloat("UIMixer", Mathf.Log10(SliderValue) * 20);
    }

    public void SetSFXVolumeLevel(float SliderValue)
    {
        Mixer.SetFloat("SFXVolume", Mathf.Log10(SliderValue) * 20);
    }

    private void OnEnable()
    {
        SetVolumeLevel(PlayerPrefs.GetFloat("UIMixer", 0));
        SetSFXVolumeLevel(PlayerPrefs.GetFloat("SFXVolume", 0));
    }

    private void OnDisable()
    {
        float MusicVolume = 0.0f;
        float SFXVolume = 0.0f;

        Mixer.GetFloat("UIMixer", out MusicVolume);
        Mixer.GetFloat("SFXVolume", out SFXVolume);

        PlayerPrefs.SetFloat("UIMixer", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }
}
