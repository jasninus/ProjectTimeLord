using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public GameObject PausMenu;
    public static bool PauseEnabled = false;

    public void Resume()
    {
        PausMenu.SetActive(false);
        Time.timeScale = 1.0f;
        PauseEnabled = false;
    }

    void Pause()
    {
        PausMenu.SetActive(true);
        Time.timeScale = 0.0f;
        PauseEnabled = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseEnabled)
                Resume();
            else
                Pause();
        }
    }
}
