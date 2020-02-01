using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimeScaledRigidbody : TimeScaled
{
    private Rigidbody2D rb;

    private Vector2 prevVelocity;

    [SerializeField] private bool doTheThing;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (doTheThing)
        {
            doTheThing = false;
            TimeScale = 0.5f;
        }
    }

    protected override void UpdateTimeScale(float prevTimeScale, float newTimeScale)
    {
        if (prevTimeScale != 0)
        {
            rb.velocity /= prevTimeScale;
        }

        if (newTimeScale == 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity *= newTimeScale;
        }

        prevVelocity = rb.velocity;
    }

    public override void TimeScaledFixedUpdate(float timeScale)
    {
        Vector2 velocityDiff = rb.velocity - prevVelocity;
        rb.velocity += velocityDiff * (timeScale - 1);
        prevVelocity = rb.velocity;
    }
}