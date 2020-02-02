using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform toFollow;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float lerpFactor;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, toFollow.position + offset, lerpFactor);
    }
}
