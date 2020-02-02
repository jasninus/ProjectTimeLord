using System;
using System.Collections;
using System.Collections.Generic;
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
}