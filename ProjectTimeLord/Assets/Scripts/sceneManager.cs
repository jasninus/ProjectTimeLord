using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class sceneManager : MonoBehaviour
{

    public Text valueText;

    private void Start()
    {
        //valueText.text = PersistantManager.instance.value.ToString();
    }

    public void headToFirstScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //PersistantManager.instance.value++;
    }

    public void headToSecondScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        //PersistantManager.instance.value++;
    }

}
