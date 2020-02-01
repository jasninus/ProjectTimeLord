using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    int hashAirMeleeAttackState = Animator.StringToHash("AirMeleeAttack");

    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateIfno, int layerIndex)
    {
        monoBehaviour.ForceNotRangedAttack();
        monoBehaviour.EnableMeleeDamager();
        monoBehaviour.SetHorizontalMovement(monoBehaviour.meleeAttackDashSpeed * monoBehaviour.GetFacing());
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!monoBehaviour.CheckGrounded())
        {
            animator.Play(hashAirMeleeAttackState, layerIndex, stateInfo.normalizedTime);
        }

        monoBehaviour.GroundHorizontalMovement(true);

       
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.DisableMeleeDamager();
    }
}
