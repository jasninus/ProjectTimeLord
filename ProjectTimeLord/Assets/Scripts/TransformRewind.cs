using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TimePoint
{
    public TimePoint(Vector3 position, Vector3 velocity, Quaternion rotation)
    {
        this.position = position;
        this.velocity = velocity;
        this.rotation = rotation;
    }

    public Vector3 position, velocity;
    public Quaternion rotation;
}

[RequireComponent(typeof(Rigidbody2D))]
public class TransformRewind : MonoBehaviour
{
    [SerializeField] private float maxRewindableSeconds;

    private List<TimePoint> timePoints = new List<TimePoint>();

    private Rigidbody2D rb;

    [SerializeField] private bool isRewinding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

        if (timePoints.Count > 0)
        {
            ApplyTimePoint();
            timePoints.RemoveAt(timePoints.Count - 1);
        }
        else
        {
            StopRewind();
        }
    }

    private void ApplyTimePoint()
    {
        TimePoint tp = timePoints.Last();
        transform.position = tp.position;
        rb.velocity = tp.velocity;
        transform.rotation = tp.rotation;
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }

    private void PushCurrentTimePoint()
    {
        float rewindableFrames = maxRewindableSeconds / Time.fixedDeltaTime;

        if (timePoints.Count >= rewindableFrames)
        {
            timePoints.RemoveAt(0);
        }

        timePoints.Add(new TimePoint(transform.position, rb.velocity, transform.rotation));
    }
}