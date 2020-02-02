using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : StickToWall {


    public GameObject dustPrefab;

    public float slideVelocity = -5;
    public float slideMultiplier = 5;
    public float dustSpawnDelay = 0.5f;
    private float timeElapsed = 0f;
	// Update is called once per frame
	override protected void Update () {

        base.Update();

        if(onWallDetected && !collisionState.idle)
        {
            float velY = slideVelocity;
            if((Input.GetAxis("Horizontal") > 0) || (Input.GetAxis("Horizontal") < 0))   ///(Input.GetAxis("Horizontal") > 0))
            {
                velY *= slideMultiplier;
            }
            body2d.velocity = new Vector2(body2d.velocity.x, velY);

            if(timeElapsed > dustSpawnDelay)
            {
                GameObject dust = Instantiate(dustPrefab);
                Vector3 pos = transform.position;
                pos.y += 2;
                dust.transform.position = pos;
                dust.transform.localScale = transform.localScale;
                timeElapsed = 0;
            }
            timeElapsed += Time.deltaTime;
        }

    }

    override protected void OnStick()
    {
        body2d.velocity = Vector2.zero;
    }

    override protected void OffWall()
    {
        base.OffWall();
        timeElapsed = 0;
    }
}
