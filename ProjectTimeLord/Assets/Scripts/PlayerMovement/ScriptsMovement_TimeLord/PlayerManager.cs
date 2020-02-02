using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseBehaviour {

    private Animator animator;
    private Crouch duckBehaviour;
    private Walk walkBehaviour;
    private Slam slamBehaviour;
    // Use this for initialization
    void Start () {
        walkBehaviour= GetComponent<Walk>();
        animator = GetComponent<Animator>();
        duckBehaviour = GetComponent<Crouch>();
        slamBehaviour = GetComponent<Slam>();
    }
	
	// Update is called once per frame
	void Update () {
        if(collisionState.idle)
        {
            ChangeAnimationState(0);                         // 0 IDLE
        }

        if (AbsVelocityX > 0 )                                  // Number 1 ==> Running
        {
            ChangeAnimationState(1);
        }
  
        if(AbsVelocityY > 0 && !collisionState.idle)        
        {
            ChangeAnimationState(2);                        // Number 2 ==> Jump
        }

        animator.speed = walkBehaviour.run ? walkBehaviour.runSpeedMultiplier : 1; 

        if(duckBehaviour.crouching || slamBehaviour.slamming)
        {
            ChangeAnimationState(3);                            // 1 Ducking Animation used also for Slamming 
        }

        if(!collisionState.idle && collisionState.onWall)
        {
            ChangeAnimationState(4);                            // Animation Sliding on the wall
        }
    }

    void ChangeAnimationState(int value)                             // Pass values to the Animator
    {
        animator.SetInteger("AnimState", value);
    }
}
