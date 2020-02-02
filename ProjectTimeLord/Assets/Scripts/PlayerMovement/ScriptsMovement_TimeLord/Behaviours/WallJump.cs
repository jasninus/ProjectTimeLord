using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : BaseBehaviour {


    Vector2 jumpVelocity = new Vector2(50, 200);
    public bool jumpingOffWall;
    public float resetDelay = 0.21f;

    private float timeElapsed = 0;
	// Update is called once per frame
	void Update () {

        if(collisionState.onWall && !collisionState.idle)
        {
            var canJump = Input.GetKeyDown(KeyCode.Space);
            if (canJump && !jumpingOffWall)
            {
                faceDirection.direction = faceDirection.direction == FaceDirection.Directions.Right ? FaceDirection.Directions.Left : FaceDirection.Directions.Right;
                body2d.velocity = new Vector2(jumpVelocity.x * (float)faceDirection.direction, jumpVelocity.y);
                ToggleScripts(false);
                jumpingOffWall = true;
            }
        }

        if(jumpingOffWall)
        {
            timeElapsed += Time.deltaTime;
             if(timeElapsed > resetDelay)
            {
                ToggleScripts(true);
                jumpingOffWall = false;
                timeElapsed = 0;
            }
        }

       

	}
}
