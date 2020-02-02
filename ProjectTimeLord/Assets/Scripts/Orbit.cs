using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject cube1;
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        //
    }

    // Update is called once per frame
    private void Update()
    {
        OrbitAround();

        //transform.Rotate(0, 0, 50 * Time.deltaTime); //rotates 50 degrees per second
    }

    private void OrbitAround()
    {
        transform.RotateAround(cube1.transform.position, Vector3.forward, speed * Time.deltaTime);

        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}