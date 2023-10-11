using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EavesdropState : CompanionState
{
    public EavesdropState(CompanionStateMachine localStateMachine) : base(localStateMachine)
    {
    }

    public override void EnterState()
    {
        stateMachine.companionFollowScript.targetPosition = stateMachine.pointToEavesdrop;
        stateMachine.companionFollowScript.distanceFromTarget = 1f;
    }

    public override void ExitState()
    {
        stateMachine.SetState(stateMachine.WaitingState);
    }
}
