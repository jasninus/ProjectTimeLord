using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneRangedSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.UpdateFacing();
        monoBehaviour.UpdateJump();
        monoBehaviour.AirHorizontalMovement();
        monoBehaviour.AirVerticalMovement();
        monoBehaviour.CheckGrounded();
        monoBehaviour.CheckWallSlide();
        monoBehaviour.CheckForRangedAttackOut();
        if (monoBehaviour.CheckForMeleeAttackInput())
        {
            monoBehaviour.MeleeAttack();
        }
        monoBehaviour.CheckAndFireGun();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        if (!nextState.IsTag("RangedAttack"))
        {
            monoBehaviour.ForceNotRangedAttack();
        }
    }
}
