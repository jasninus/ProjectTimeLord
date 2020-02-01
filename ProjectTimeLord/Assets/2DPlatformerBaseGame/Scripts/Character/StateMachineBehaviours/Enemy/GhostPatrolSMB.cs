using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPatrolSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //We do this explicitly here instead of in the enemy class, that allow to handle obstacle differently according to state
        //monoBehaviour.CheckGrounded();

        float dist = monoBehaviour.speed;

        monoBehaviour.CheckTargetStillVisible();
        monoBehaviour.CheckMeleeAttack();

        float amount = monoBehaviour.speed * 2.0f;

        if (monoBehaviour.CheckForObstacle(dist) || (monoBehaviour.usePatrolBorders && !monoBehaviour.CheckWithinPatrolBorders()))
        { monoBehaviour.TurnAround(dist); } 
        else { monoBehaviour.SetHorizontalSpeed(dist); }


        monoBehaviour.ScanForPlayer();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.NoMovement();
    }
}
