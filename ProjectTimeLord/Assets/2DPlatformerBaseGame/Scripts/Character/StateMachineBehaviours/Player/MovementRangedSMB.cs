using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRangedSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.TeleportToColliderBottom();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.UpdateFacing();
        monoBehaviour.GroundHorizontalMovement(true);
        monoBehaviour.GroundVerticalMovement();
        monoBehaviour.CheckGrounded();
        monoBehaviour.CheckForPushing();
        monoBehaviour.CheckForRangedAttackOut();
        monoBehaviour.CheckAndFireGun();
        if (monoBehaviour.CheckForFallInput())
            monoBehaviour.MakePlatformFallThrough();
        else if (monoBehaviour.CheckForJumpInput())
            monoBehaviour.SetVerticalMovement(monoBehaviour.jumpSpeed);
        else if (monoBehaviour.CheckForMeleeAttackInput())
        {
            monoBehaviour.ForceNotRangedAttack();
            monoBehaviour.MeleeAttack();
        }
        
    }

    /*
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exiting Ranged");
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        if (!nextState.IsTag("RangedAttack"))
        {
            monoBehaviour.ForceNotRangedAttack();
            Debug.Log("Forced not ranged attack");
        }
    }
    */
}
