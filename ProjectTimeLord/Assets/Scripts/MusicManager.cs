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
    public string levelOneName, levelTwoName;

    private void Awake()
    {
        audioTracks[0].Play();
        audioTracks[1].Play();
        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene prev, Scene current)
    {
        //current.buildIndex // TODO cool stuff
        if (current.name == levelOneName)
        {
            musicAnim.SetTrigger("fadeOut");
            musicAnim2.SetTrigger("fadeIn2");
        }
        if (current.name == levelTwoName)
        {
            musicAnim.SetTrigger("fadeIn");
            musicAnim2.SetTrigger("fadeOut2");
        }
        Debug.Log(current.name);
    }

}
