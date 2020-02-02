using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : TimeBullet
{
    protected override void Collide(Collision2D other)
    {
        TimeScaledRigidbody scaled = other.gameObject.GetComponent<TimeScaledRigidbody>();
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

        if (scaled && rb)
        {
            //scaled.TimeScale = 0;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}