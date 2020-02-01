using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeScaled : MonoBehaviour
{
    [SerializeField] private float timeScale = 1;

    private float prevFrameTimeScale;

    public virtual float TimeScale
    {
        get => timeScale;
        set
        {
            UpdateTimeScale(timeScale, value);
            timeScale = value;
        }
    }

    protected virtual void Start()
    {
        prevFrameTimeScale = timeScale;
        UpdateTimeScale(1, timeScale);
    }

    protected virtual void UpdateTimeScale(float prevTimeScale, float newTimeScale)
    {
    }

    protected virtual void Update()
    {
        if (prevFrameTimeScale != timeScale)
        {
            UpdateTimeScale(prevFrameTimeScale, timeScale);
            prevFrameTimeScale = timeScale;
        }

        TimeScaledUpdate(timeScale);
    }

    protected virtual void FixedUpdate()
    {
        TimeScaledFixedUpdate(timeScale);
    }

    public virtual void TimeScaledUpdate(float timeScale)
    {
    }

    public virtual void TimeScaledFixedUpdate(float timeScale)
    {
    }
}