using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private float finalMovement;

    [SerializeField]
    private float jumpSpeed;

    private Rigidbody2D rb;
    public bool facingDirection = true;
    public bool isGrounded;

    //public Transform groundCollision;
    public int extraJumps;

    public float jumpForce;

    public LayerMask layerMask;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = 1;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        //else if(Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded)
        //{
        //    rb.velocity = Vector2.up * jumpForce;
        //}

        float movement = Input.GetAxis("Horizontal");
        finalMovement = movement * speed;

        FlipTowardsMouse();
        //if (facingDirection == false && movement > 0)
        //{
        //    Flip();
        //}
        //else if (facingDirection == true && movement < 0)
        //{
        //    Flip();
        //}
    }

    private void FlipTowardsMouse()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, diff.x >= 0 ? 0 : 180, transform.rotation.eulerAngles.z);
    }

    private void Flip()
    {
        facingDirection = !facingDirection;
        transform.rotation = Quaternion.Euler(transform.rotation.x, Math.Abs(transform.rotation.y) < 0.01f ? 180 : 0, transform.rotation.z);
        //Vector3 scalar = transform.localScale;
        //scalar.x *= -1;
        //transform.localScale = scalar;
    }

    private void FixedUpdate()
    {
        isGrounded = castRays(0.08f);
        // isGrounded = Physics2D.BoxCast(groundCollision.position, new Vector2(1.0f,0.2f), Vector2.down, 0.5f);
        rb.velocity = new Vector2(finalMovement * Time.deltaTime, rb.velocity.y);
    }

    private bool castRays(float distanceFromCenter)
    {
        return castRay(new Vector2(-distanceFromCenter, 0f)) || castRay(new Vector2(distanceFromCenter, 0f));
    }

    private bool castRay(Vector2 offset)
    {
        RaycastHit2D hit; // Stores the result of the raycast

        // Cast the ray and store the result in hit
        Vector2 positionToCheck = (Vector2)transform.position + offset;
        hit = Physics2D.Raycast(positionToCheck, -Vector2.up, 0.61f, layerMask);
        Debug.DrawRay(positionToCheck, -Vector2.up, Color.red);
        // If the ray hit a collider...
        if (hit.collider != null)
        {
            // Destroy it
            //Destroy(hit.collider.gameObject);
            //Debug.Log("Hitting Something");
            // Return true
            return true;
        }
        // Else, return false
        return false;
    }
}