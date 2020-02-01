using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManagerScript : MonoBehaviour
{
    public static PersistantManagerScript instance { get; private set; }
    public int value;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
