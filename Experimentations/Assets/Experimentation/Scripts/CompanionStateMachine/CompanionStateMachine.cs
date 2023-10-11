using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

public class CompanionStateMachine : MonoBehaviour
{
    [SerializeField] public Transform pointToEavesdrop, pointToWalk, playerPosition;
    [HideInInspector] public simpleNavFollow companionFollowScript;
    [HideInInspector] public NavMeshAgent companionAgent;

    #region States and properties
    private WaitingState _waitingState;
    private EavesdropState _eavesdropState;
    private FollowState _followState;

    public WaitingState WaitingState
    {
        get
        {
            if(_waitingState == null)
            {
                _waitingState = new WaitingState(this);
            }
            return _waitingState;
        }
    }

    public EavesdropState EavesdropState
    {
        get
        {
            if (_eavesdropState == null)
            {
                _eavesdropState = new EavesdropState(this);
            }
            return _eavesdropState;
        }
    }

    public FollowState FollowState
    {
        get
        {
            if (_followState == null)
            {
                _followState = new FollowState(this);
            }
            return _followState;
        }
    }
    #endregion

    private CompanionState currentState;

    public void SetState(CompanionState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }

    public void ExitState()
    {
        currentState.ExitState();
    }


    private void Start()
    {
        companionAgent = GetComponent<NavMeshAgent>();
        companionFollowScript = GetComponent<simpleNavFollow>();
        // Set initial state
        SetState(EavesdropState);
    }

    private void Update()
    {
        currentState.UpdateState();
    }
}
