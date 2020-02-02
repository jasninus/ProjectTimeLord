using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseBehaviour {

    [SerializeField]
    private float speed = 50f;
    public float runSpeedMultiplier = 2f;
    public GameObject dustEffectPrefab;
    public bool run = false;
    [SerializeField]
    private float dashTimeElapsed = 0;
    [SerializeField]
    private float dashInterval = 1.0f;
    private int movementClicks = 0;

    // Update is called once per frame
    void Update () {

        float movementInput = Input.GetAxis("Horizontal");
        bool right = (movementInput > 0);
        bool left = (movementInput < 0);

        if (right || left)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if (!run)
                {
                    movementClicks++;
                }
                else if (run && movementClicks == 2)
                {
                    movementClicks = 0;
                }

                if (movementClicks == 2)
                {
                    run = true;

                }
            }

            float tSpeed = speed;
            if(run && runSpeedMultiplier > 0)
            {
                tSpeed *= runSpeedMultiplier;
                dashTimeElapsed += Time.deltaTime;
                GameObject clone = Instantiate(dustEffectPrefab);
                clone.transform.position = new Vector2(transform.position.x -5 * movementInput, transform.position.y - 8);

            }
            if (run && (dashTimeElapsed >= dashInterval))
            {
                dashTimeElapsed = 0;
                movementClicks = 0;
                run = false;

            }
            var velX = tSpeed * (float)faceDirection.direction;
            body2d.velocity = new Vector2(velX, body2d.velocity.y);
        }
        else
        {
            run = false;
            dashTimeElapsed = 0;
            movementClicks = 0;
        }
    }
}


