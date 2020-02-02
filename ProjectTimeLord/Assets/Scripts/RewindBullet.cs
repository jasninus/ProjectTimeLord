using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindBullet : TimeBullet
{
    protected override void Collide(Collision2D other)
    {
        foreach (IRewindable rewindable in other.gameObject.GetComponents<IRewindable>())
        {
            rewindable.StartRewind();
        }
    }
}