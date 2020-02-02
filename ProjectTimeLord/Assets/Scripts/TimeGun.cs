using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeGun : MonoBehaviour
{
    [SerializeField] private GameObject freezeBullet, rewindBullet;

    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private Transform wielderTransform;

    [SerializeField] private BoxGrabber grabber;

    private void Update()
    {
        if (grabber.IsGrabbing)
        {
            return;
        }

        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(rewindBullet, bulletSpawn.position, bulletSpawn.rotation);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Instantiate(freezeBullet, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    private void LookAtMouse()
    {
        Vector2 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float degRot = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x);

        if (Math.Abs(wielderTransform.rotation.eulerAngles.y - 180) < 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, 0, degRot >= 0 ? Mathf.Clamp(degRot, 100, 180) :
                                                                       Mathf.Clamp(degRot, -180, -100));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(degRot, -80, 80));
        }
    }

    public void StartUnFreeze(Rigidbody2D rb, float freezeTime)
    {
        StartCoroutine(UnFreezing(rb, freezeTime));
    }

    public void StartUnFreeze(Movingplatform platform, float freezeTime)
    {
        StartCoroutine(UnFreezing(platform, freezeTime));
    }

    public void StartUnFreeze(Orbit o, float freezeTime)
    {
        StartCoroutine(UnFreezing(o, freezeTime));
    }

    private IEnumerator UnFreezing(Rigidbody2D rb, float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);

        if (rb.gameObject != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.constraints = RigidbodyConstraints2D.None;
            FreezeBullet.frozenObjects.Remove(rb);
        }
    }

    private IEnumerator UnFreezing(Movingplatform platform, float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);

        if (platform != null)
        {
            FreezeBullet.FrozenPlatform frozen = FreezeBullet.frozenPlatforms.FirstOrDefault(p => p.platform == platform);

            if (frozen.platform != null)
            {
                frozen.platform.TimeScale = frozen.unfrozenTimeScale;
                FreezeBullet.frozenPlatforms.Remove(frozen);
            }
        }
    }

    private IEnumerator UnFreezing(Orbit o, float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);

        if (o != null)
        {
            o.enabled = true;
            FreezeBullet.frozenOrbits.Remove(o);
        }
    }
}