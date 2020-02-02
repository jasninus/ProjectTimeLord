using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour {

    public Directions direction = Directions.Right;
    public enum Directions
    {
        Right = 1,
        Left = -1
    }
    // Update is called once per frame
    void Update () {

        bool right = (Input.GetAxis("Horizontal") > 0);
        bool left = (Input.GetAxis("Horizontal") < 0);

        if (right)
        {
            direction = Directions.Right;

        }
        else if(left)
        {
            direction = Directions.Left;
        }
        transform.localScale = new Vector3((float)direction, 1, 1);
    }

    
}
