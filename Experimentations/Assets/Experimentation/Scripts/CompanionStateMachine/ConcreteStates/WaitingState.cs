using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : CompanionState
{
    public WaitingState(CompanionStateMachine localStateMachine) : base(localStateMachine)
    {
    }

    public override void EnterState()
    {
        stateMachine.companionFollowScript.targetPosition = stateMachine.pointToWalk;
        stateMachine.companionFollowScript.distanceFromTarget = 1f;
    }

    public override void ExitState()
    {
        stateMachine.SetState(stateMachine.FollowState);
    }
}
