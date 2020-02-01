using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    [SerializeField] private GameObject[] resetObjects;

    [SerializeField] private bool testReset;

    private void Update()
    {
        if (testReset)
        {
            testReset = false;

            foreach (IResettable reset in resetObjects.Select(o => o.GetComponent<IResettable>()).Where(o => o != null))
            {
                reset.ResetObject();
            }
        }
    }
}