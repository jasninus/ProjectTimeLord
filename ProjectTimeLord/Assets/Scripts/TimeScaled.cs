using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeScaled : MonoBehaviour
{
    [SerializeField] private float timeScale;

    [SerializeField] private bool hasRigidbody;

    public float TimeScale
    {
        get => timeScale;
        set
        {
            timeScale = value;
            UpdateTimeScale();
        }
    }

    protected virtual void UpdateTimeScale()
    {
    }

    protected virtual void Update()
    {
        TimeScaledUpdate(timeScale);
    }

    protected virtual void FixedUpdate()
    {
        TimeScaledFixedUpdate(timeScale);
    }

    public abstract void TimeScaledUpdate(float timeScale);

    public abstract void TimeScaledFixedUpdate(float timeScale);
}