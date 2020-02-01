using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMeleeAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.ForceNotRangedAttack();
    }

    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.EnableMeleeDamager();
        if (monoBehaviour.dashWhileAirborne)
        {
            monoBehaviour.SetHorizontalMovement(monoBehaviour.meleeAttackDashSpeed * monoBehaviour.GetFacing());
        }
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.UpdateJump();
        monoBehaviour.AirHorizontalMovement();
        monoBehaviour.AirVerticalMovement();
        monoBehaviour.CheckGrounded();
        monoBehaviour.CheckWallSlide();
        
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.DisableMeleeDamager();
    }
}
