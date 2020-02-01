using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class TimeBullet : MonoBehaviour
{
    [SerializeField] private float speed, maxLifetime;

    private void Awake()
    {
        Invoke("DestroyBullet", maxLifetime);
        GetComponent<Rigidbody2D>().velocity = transform.rotation * new Vector3(speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Collide(other);
        DestroyBullet();
    }

    protected abstract void Collide(Collision2D other);

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}