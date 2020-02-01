using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioSource[] audioTracks;
    public Animator musicAnim, musicAnim2;

    private void Awake()
    {
        audioTracks[0].Play();
        audioTracks[1].Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            musicAnim.SetTrigger("fadeOut");
            musicAnim2.SetTrigger("fadeIn2");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            musicAnim.SetTrigger("fadeIn");
            musicAnim2.SetTrigger("fadeOut2");
        }

    }

}
