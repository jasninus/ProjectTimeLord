using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FreezeBullet : TimeBullet
{
    public static List<Rigidbody2D> frozenObjects = new List<Rigidbody2D>();

    public struct FrozenPlatform
    {
        public float unfrozenTimeScale;
        public Movingplatform platform;
    }

    public static List<FrozenPlatform> frozenPlatforms = new List<FrozenPlatform>();

    [SerializeField] private float freezeTime = 3;

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
                GameObject.FindWithTag("Player").GetComponentInChildren<TimeGun>().StartUnFreeze(rb, freezeTime);
            }
        }
        else
        {
            Movingplatform platform = other.gameObject.GetComponent<Movingplatform>();

            if (platform)
            {
                FrozenPlatform frozen = frozenPlatforms.FirstOrDefault(p => p.platform == platform);

                if (frozen.platform != null)
                {
                    frozen.platform.TimeScale = frozen.unfrozenTimeScale;
                    frozenPlatforms.Remove(frozen);
                }
                else
                {
                    frozenPlatforms.Add(new FrozenPlatform { platform = platform, unfrozenTimeScale = platform.TimeScale });
                    platform.TimeScale = 0;
                    GameObject.FindWithTag("Player").GetComponentInChildren<TimeGun>().StartUnFreeze(platform, freezeTime);
                }
            }
        }
    }
}