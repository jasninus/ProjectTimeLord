using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct RigidbodyTimePoint
{
    public RigidbodyTimePoint(Vector3 position, Vector3 velocity, Quaternion rotation)
    {
        transformTimePoint = new TransformTimePoint(position, rotation);
        this.velocity = velocity;
    }

    public TransformTimePoint transformTimePoint;
    public Vector3 velocity;
}

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyRewind : TransformRewind
{
    [SerializeField] protected Behaviour[] disableWhenRewinding;

    protected new List<RigidbodyTimePoint> timePoints = new List<RigidbodyTimePoint>();

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartRewind()
    {
        base.StartRewind();
        rb.isKinematic = true;
        SetActiveBehavioursForRewind(false);
    }

    protected override void CheckApplyTimePoint()
    {
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

    private void SetActiveBehavioursForRewind(bool value)
    {
        foreach (Behaviour behaviour in disableWhenRewinding)
        {
            behaviour.enabled = value;
        }
    }

    public override void StopRewind()
    {
        base.StopRewind();
        rb.isKinematic = false;
        SetActiveBehavioursForRewind(true);

        if (timePoints.Count > 0)
        {
            rb.velocity = timePoints.Last().velocity;
        }
    }

    protected void ApplyTimePoint(RigidbodyTimePoint tp)
    {
        ApplyTimePoint(tp.transformTimePoint);
        //rb.velocity = tp.velocity;
        rb.velocity = Vector2.zero;
    }

    protected override void PushCurrentTimePoint()
    {
        float rewindableFrames = maxRewindableSeconds / Time.fixedDeltaTime;

        if (timePoints.Count >= rewindableFrames && timePoints.Count > 0)
        {
            RemoveOldestTimePoint();
        }

        AddNewTimePoint();
    }

    protected override void RemoveOldestTimePoint()
    {
        if (timePoints.Count > 0)
        {
            timePoints.RemoveAt(0);
        }
    }

    protected override void AddNewTimePoint()
    {
        timePoints.Add(new RigidbodyTimePoint(transform.position, rb.velocity, transform.rotation));
    }
}