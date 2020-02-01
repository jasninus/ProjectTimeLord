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
        float radRot = Mathf.Atan2(diff.y, diff.x);
        transform.rotation = Quaternion.Euler(0, 0, radRot * Mathf.Rad2Deg);
    }
}