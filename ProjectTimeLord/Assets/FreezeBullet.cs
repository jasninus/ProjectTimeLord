using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : TimeBullet
{
    private static List<Rigidbody2D> frozenObjects = new List<Rigidbody2D>();

    protected override void Collide(Collision2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

        if (rb)
        {
            if (frozenObjects.Contains(rb))
            {
                rb.isKinematic = false;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.None;
                frozenObjects.Remove(rb);
            }
            else
            {
                //scaled.TimeScale = 0;
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                frozenObjects.Add(rb);
            }
        }
    }
}