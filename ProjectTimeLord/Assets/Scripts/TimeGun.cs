using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGun : MonoBehaviour
{
    private enum FiringMode
    {
        Slow,
        Freeze,
        Rewind
    }

    [SerializeField] private GameObject slowBullet, freezeBullet, rewindBullet;

    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private Transform wielderTransform;

    private void Update()
    {
        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(rewindBullet, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    private void LookAtMouse()
    {
        Vector2 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float degRot = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x);

        if (Math.Abs(wielderTransform.rotation.eulerAngles.y - 180) < 0.01f)
        {
            if (degRot >= 90)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(degRot, 90, 180));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(degRot, -180, -90));
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(degRot, -90, 90));
        }
    }
}