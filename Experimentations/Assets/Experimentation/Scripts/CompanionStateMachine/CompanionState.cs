using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompanionState
{

    protected CompanionStateMachine stateMachine;

    public CompanionState (CompanionStateMachine localStateMachine)
    {
        this.stateMachine = localStateMachine;
    }

    public abstract void EnterState();

    public virtual void UpdateState()
    {
        //do nothing
    }
    public abstract void ExitState();
}
