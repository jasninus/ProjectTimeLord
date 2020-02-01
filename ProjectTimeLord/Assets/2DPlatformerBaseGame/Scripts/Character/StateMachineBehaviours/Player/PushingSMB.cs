using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.ForceNotRangedAttack();
        monoBehaviour.StartPushing();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.UpdateFacing();
        monoBehaviour.GroundHorizontalMovement(true, monoBehaviour.pushingSpeedProportion);
        monoBehaviour.GroundVerticalMovement();
        monoBehaviour.CheckGrounded();
        monoBehaviour.CheckForPushing();
        monoBehaviour.MovePushable();
        monoBehaviour.CheckForJumpInput();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
