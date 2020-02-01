using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.AirVerticalMovement();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.AirVerticalMovement();
        monoBehaviour.UpdateFacing();
        monoBehaviour.UpdateJump();
        monoBehaviour.AirHorizontalMovement();
        monoBehaviour.CheckGrounded();
        //if (monoBehaviour.CheckGrounded())
        //    monoBehaviour.AddLandLocation();
        monoBehaviour.CheckWallSlide();
        monoBehaviour.CheckForRangedAttackOut();
        
        if (monoBehaviour.CheckForMeleeAttackInput())
            monoBehaviour.MeleeAttack();
        monoBehaviour.CheckAndFireGun();
    }
}
