using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxGrabber : MonoBehaviour
{
    [SerializeField] private Transform grabbedBoxPos;

    [SerializeField] private float throwForce;

    private GrabbyBox grabbing;

    public bool IsGrabbing => grabbing != null;

    public void GrabBox(GrabbyBox toGrab)
    {
        toGrab.transform.parent = grabbedBoxPos;
        toGrab.transform.localPosition = Vector3.zero;
        grabbing = toGrab;
    }

    public void Throw()
    {
        Vector2 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        grabbing.GetComponent<Rigidbody2D>().velocity = lookDir.normalized * throwForce;
        grabbing = null;
    }

    public void Drop()
    {
        grabbing = null;
    }
}