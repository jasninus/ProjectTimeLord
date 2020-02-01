using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TransformTimePoint
{
    public TransformTimePoint(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public Vector3 position;
    public Quaternion rotation;
}

public class TransformRewind : MonoBehaviour, IRewindable
{
    [SerializeField] private float maxRewindableSeconds;

    protected List<TransformTimePoint> timePoints = new List<TransformTimePoint>();

    [SerializeField] private bool isRewinding;

    public bool IsRewinding => isRewinding;

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            PushCurrentTimePoint();
        }
        else
        {
            RewindFrame();
        }
    }

    private void RewindFrame()
    {
        if (!isRewinding)
        {
            return;
        }

        if (timePoints.Count > 1)
        {
            ApplyTimePoint(timePoints.Last());
            timePoints.RemoveAt(timePoints.Count - 1);
        }
        else
        {
            StopRewind();
        }
    }

    protected void ApplyTimePoint(TransformTimePoint tp)
    {
        transform.position = tp.position;
        transform.rotation = tp.rotation;
    }

    public virtual void StartRewind()
    {
        isRewinding = true;
    }

    public virtual void StopRewind()
    {
        isRewinding = false;
    }

    private void PushCurrentTimePoint()
    {
        float rewindableFrames = maxRewindableSeconds / Time.fixedDeltaTime;

        if (timePoints.Count >= rewindableFrames && timePoints.Count > 0)
        {
            RemoveOldestTimePoint();
        }

        AddNewTimePoint();
    }

    protected virtual void RemoveOldestTimePoint()
    {
        if (timePoints.Count > 0)
        {
            timePoints.RemoveAt(0);
        }
    }

    protected virtual void AddNewTimePoint()
    {
        timePoints.Add(new TransformTimePoint(transform.position, transform.rotation));
    }
}