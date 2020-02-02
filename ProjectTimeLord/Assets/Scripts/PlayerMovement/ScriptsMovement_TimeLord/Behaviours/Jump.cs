using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseBehaviour {

    public float jumpSpeed = 200f;
    public float jumpDelay = .1f;
    public int jumpCount = 2;
    public GameObject dustEffectPrefab;
    protected float lastJumpTime = 0;
    protected int jumpRemaining = 0;
    protected float holdTime = 0;


	// Update is called once per frame
	protected virtual void Update () {

        bool canJump = Input.GetKeyDown(KeyCode.Space);
        if(canJump)
        {
             holdTime += Time.deltaTime;
        }

		if(collisionState.idle)
        {
            if(canJump && holdTime < 0.1f)
            {
                jumpRemaining = jumpCount - 1;
                OnJump();
            }

        }
        else
        {
            if (canJump && holdTime < 0.1f && Time.time - lastJumpTime > jumpDelay)
            {
                if(jumpRemaining > 0)
                {
                    OnJump();
                    jumpRemaining--;
                    GameObject clone = Instantiate(dustEffectPrefab);
                    clone.transform.position = transform.position;
                }
               
            }
            else
            {
                holdTime = 0;
            }
        }
	}

    protected virtual void OnJump()
    {
        Vector2 vel = body2d.velocity;
        lastJumpTime = Time.time;
        body2d.velocity = new Vector2(vel.x, jumpSpeed);
    }
}
