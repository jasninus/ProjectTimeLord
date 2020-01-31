using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableRubble : MonoBehaviour, IRewindable
{
    public void StartRewind()
    {
        transform.parent.GetComponent<IRewindable>()?.StartRewind(); ;
    }
}