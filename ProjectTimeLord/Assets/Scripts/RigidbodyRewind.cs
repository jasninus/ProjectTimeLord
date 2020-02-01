using System.Collections;
using System.Collections.Generic;
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
    private new List<RigidbodyTimePoint> timePoints = new List<RigidbodyTimePoint>();

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartRewind()
    {
        base.StartRewind();
        rb.isKinematic = true;
    }

    public override void StopRewind()
    {
        base.StopRewind();
        rb.isKinematic = false;
    }

    protected override void AddNewTimePoint()
    {
        timePoints.Add(new RigidbodyTimePoint());
    }
}