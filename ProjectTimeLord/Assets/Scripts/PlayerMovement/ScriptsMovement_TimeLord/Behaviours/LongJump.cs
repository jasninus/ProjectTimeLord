using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJump : Jump {

    [SerializeField]
    public float longJumpDelay = 0.15f;
    [SerializeField]
    public float longJumpMultiplier = 1.5f;

    public bool canLongJump;
    public bool isLongJumping;

    protected override void Update()
    {
        bool canJump = Input.GetKeyDown(KeyCode.Space);
        if(canJump)
        {
            holdTime += Time.deltaTime;
        }

        if (!canJump)
        {
            canLongJump = false;
        }

        if (collisionState.idle && isLongJumping)
            isLongJumping = false;

        base.Update();

        if (canLongJump && !collisionState.idle && holdTime > longJumpDelay)
        {
            Vector3 velocity = body2d.velocity;
            body2d.velocity = new Vector2(velocity.x, jumpSpeed * longJumpMultiplier);
            canLongJump = false;
            isLongJumping = true;
        }
    }

    protected override void OnJump()
    {
        base.OnJump();
        canLongJump = true;
    }
       

}
