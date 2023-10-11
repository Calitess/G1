using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : CompanionState
{
    public FollowState(CompanionStateMachine localStateMachine) : base(localStateMachine)
    {
    }

    public override void EnterState()
    {
        stateMachine.companionFollowScript.targetPosition = stateMachine.playerPosition;
        stateMachine.companionFollowScript.distanceFromTarget = 2f;

    }

    public override void ExitState()
    {
    }
}
