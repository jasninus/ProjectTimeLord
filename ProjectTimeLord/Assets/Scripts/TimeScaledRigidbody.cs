using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimeScaledRigidbody : TimeScaled
{
    private Rigidbody2D rb;

    private bool first;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void UpdateTimeScale()
    {
        if (!first)
        {
            rb.mass *= TimeScale;
            rb.velocity /= TimeScale;
            rb.angularVelocity /= TimeScale;
        }
        first = false;

        //_timeScale = Mathf.Abs(value);

        //rb.mass /= timeScale;
        //rb.velocity *= timeScale;
        //rb.angularVelocity *= timeScale;
    }

    public override void TimeScaledUpdate(float timeScale)
    {
    }

    public override void TimeScaledFixedUpdate(float timeScale)
    {
    }
}