using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrabbyBox : MonoBehaviour
{
    [SerializeField] private float grabDis;

    private bool grabbed;

    private Transform playerTransform;

    private BoxGrabber grabber;

    private Rigidbody2D rb;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        grabber = playerTransform.GetComponent<BoxGrabber>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (CanGrabBox())
        {
            GrabBox();
        }

        if (grabbed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Throw();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Drop();
            }
        }
    }

    private void GrabBox()
    {
        grabbed = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        grabber.GrabBox(this);
    }

    private void Throw()
    {
        Ungrab();
        grabber.Throw();
    }

    private void Drop()
    {
        Ungrab();
        grabber.Drop();
    }

    private void Ungrab()
    {
        grabbed = false;
        rb.isKinematic = false;
        transform.parent = null;
    }

    private bool CanGrabBox()
    {
        return Input.GetKeyDown(KeyCode.E) &&
            Vector2.Distance(transform.position, playerTransform.position) <= grabDis &&
            FacingBox() &&
            !rb.isKinematic;
    }

    private bool FacingBox()
    {
        float xDiff = transform.position.x - playerTransform.position.x;
        return xDiff >= 0 ? playerTransform.right.x > 0 : playerTransform.right.x < 0;
    }
}