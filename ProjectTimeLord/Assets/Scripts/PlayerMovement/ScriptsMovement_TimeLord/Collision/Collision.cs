using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public LayerMask collisionLayer;
    public Vector2 feetPosition = Vector2.zero;
    public Vector2 rightPosition = Vector2.zero;
    public Vector2 leftPosition = Vector2.zero;
    public float collisionRadius = 10f;
    private FaceDirection faceDirection;
    public bool isGrounded;
    public bool idle;
    public bool onWall;
    public Color debugCollisionColor = Color.blue;
    // Use this for initialization
    void Awake () {
        faceDirection = GetComponent<FaceDirection>();
	}
	
    private void FixedUpdate()
    {
        var pos = feetPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;

       // isGrounded = castRay(pos);
        idle = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);

        ///////////////////////////////////////////////
        Debug.Log(idle);
        pos = faceDirection.direction == FaceDirection.Directions.Right ? rightPosition : leftPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;

        onWall = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugCollisionColor;

        var positions = new Vector2[] { rightPosition, feetPosition, leftPosition };
        foreach( var position in positions)
        {
            var pos = position;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            Gizmos.DrawWireSphere(pos, collisionRadius);
        }

        
    }
    //private bool castRays(float distanceFromCenter)
    //{
    //    // Return if the ray on the left hit something
    //    if (castRay(new Vector2(-distanceFromCenter, 0f)))
    //    { return true; }
    //    // Return if the ray on the right hit something
    //    else if (castRay(new Vector2(distanceFromCenter, 0f)))
    //    { return true; }

    //    return false;
    //}


    //private bool castRay(Vector2 offset)
    //{
    //    RaycastHit2D hit; // Stores the result of the raycast

    //    // Cast the ray and store the result in hit

    //    Vector2 positionToCheck = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
    //    hit = Physics2D.Raycast(positionToCheck, -Vector2.up, 0.2f, collisionLayer);
    //    Debug.DrawRay(positionToCheck, -Vector2.up, Color.red);
    //    // If the ray hit a collider...
    //    if (hit.collider != null)
    //    {
    //        // Destroy it
    //        //Destroy(hit.collider.gameObject); // Destroy Object When Landing on It
    //        Debug.Log("Hitting Something");
    //        // Return true      
    //        return true;
    //    }
    //    // Else, return false
    //    return false;
    //}
}
