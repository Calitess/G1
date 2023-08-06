using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR;

enum InteractorState
{
    Search,
    Approach,
    Rise,
    Touch,
    Reset,
}


public class EnvironmentInteractor : MonoBehaviour
{
    [SerializeField] bool isRightDirection;

    float _wingspan, _armLength,_colliderCenterOffset;

    Collider _currentColliderTarget;
    Rigidbody _rb;

    Transform _rootTransform, _handTransform, _elbowTransform, _shoulderTransform, _ikTargetTransform;

    Vector3 _closestPointPosition = Vector3.positiveInfinity, _prepPointPosition;

    BoxCollider _boxCollider;

    float _approachingDistanceThreshold = 3, _approachWeight = 0.5f;

    private Coroutine _updateWeightsCoroutine, _resetStateCountdownCoroutine, _approachStateCountdownCoroutine;

    Vector3 _armForward;
    float _angle;

    MultiRotationConstraint _multiRotationConstraint;
    TwoBoneIKConstraint _ikConstraint;

    //State Machine
    InteractorState _currentState = InteractorState.Reset;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _ikConstraint = GetComponent<TwoBoneIKConstraint>();
        _multiRotationConstraint = GetComponent<MultiRotationConstraint>();
        _rootTransform = transform.root;
        _wingspan = _rootTransform.GetComponent<CapsuleCollider>().height;
        _colliderCenterOffset = _wingspan / 8;
        _shoulderTransform = _ikConstraint.data.root.transform;
        _elbowTransform = _ikConstraint.data.mid.transform;
        _handTransform = _ikConstraint.data.tip.transform;

        Vector3 upperArm = _elbowTransform.position - _shoulderTransform.position;
        Vector3 forearm = _handTransform.position - _elbowTransform.position;

        _ikTargetTransform = _ikConstraint.data.target.transform;
        ConstructColliders();

    }

    private void Start()
    {
        EnterState(_currentState);
    }

    private void Update()
    {
        UpdateState();
        CheckChangeState();
    }

    private void ConstructColliders()
    {
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.center = new Vector3(_elbowTransform.position.x, _colliderCenterOffset, _wingspan / 2);
        _boxCollider.size = new Vector3(1f, _wingspan, _wingspan);
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player") && !_currentColliderTarget && _currentState!=InteractorState.Reset)
        {
            _currentColliderTarget = other;
            //_armForward = isRightDirection ? _rootTransform.right : -_rootTransform.right;
            //_angle = Vector3.Angle(_armForward, toClosestPoint);

            _closestPointPosition = other.ClosestPoint(new Vector3(_shoulderTransform.position.x, _shoulderTransform.position.y - _armLength, _shoulderTransform.position.z) + _rb.velocity);
            _prepPointPosition = new Vector3(_closestPointPosition.x, 0, _closestPointPosition.z);
            _ikTargetTransform.position = _prepPointPosition;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == _currentColliderTarget)
        {
            _closestPointPosition = other.ClosestPoint(new Vector3(_shoulderTransform.position.x, _shoulderTransform.position.y, _shoulderTransform.position.z) + _rb.velocity);
            _prepPointPosition = new Vector3(_closestPointPosition.x, _closestPointPosition.y, _closestPointPosition.z);
            _ikTargetTransform.position= _prepPointPosition;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == _currentColliderTarget)
        {
            _currentColliderTarget = null;
            _closestPointPosition = Vector3.positiveInfinity;
            ChangeState(InteractorState.Reset);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_closestPointPosition != null)
        {
            Gizmos.DrawSphere(_closestPointPosition, .1f);
        }
    }

    void UpdateState()
    {
        switch(_currentState)
        {
            case InteractorState.Search:
                break;
            case InteractorState.Approach:
                break;
            case InteractorState.Rise:
                break;
            case InteractorState.Touch:
                break;
        }
    }

    void CheckChangeState()
    {
        bool shouldReset = CheckShouldReset();
        if(shouldReset)
        {
            ChangeState(InteractorState.Reset);
            return;
        }

        switch(_currentState)
        {
            case InteractorState.Search:
                if(_closestPointPosition != Vector3.positiveInfinity && Vector3.Distance(_closestPointPosition, _rootTransform.position) < _approachingDistanceThreshold)
                {
                    ChangeState(InteractorState.Approach);

                }
                break;

            case InteractorState.Approach:
                // float newDistance = Vector3.Distance(_closestPointPosition, _rootTransform.position);
                // if(currentDistance)
                break;

            case InteractorState.Rise:
                break;

            case InteractorState.Touch:
                break;



        }
    }

    void ChangeState(InteractorState nextState)
    {
        //Do nothing if we are already in new state
        if(nextState == _currentState)
        {
            return;
        }

        //perform the 'exit state' functionality if the current state is different from the next state
        ExitState();

        //perform the 'enter state' functionality for hte next state
        EnterState(nextState);

        _currentState = nextState;
    }

    bool CheckShouldReset()
    {
        switch (_currentState)
        {
            case InteractorState.Touch:
            case InteractorState.Approach:
            case InteractorState.Rise:

                if(_rb.velocity == Vector3.zero || Mathf.Round(_rb.velocity.y) >= 1)
                {
                    Debug.Log("HIT RESET");
                    return true;
                }

                return false;

            case InteractorState.Search:
            case InteractorState.Reset:
            default:
                return false;

        }
    }

    void ExitState()
    {
        switch(_currentState)
        {
            case InteractorState.Search:
                break;
            case InteractorState.Approach:
                if (_approachStateCountdownCoroutine !=null)
                {
                    StopCoroutine( _approachStateCountdownCoroutine );
                }
                break;
            case InteractorState.Rise:
                break;
            case InteractorState.Touch:
                break;
            case InteractorState.Reset:
                break;
        }
            
    }

    void EnterState(InteractorState state)
    {
        switch(state)
        {
            case InteractorState.Search:
                break;
            case InteractorState.Approach:
                _updateWeightsCoroutine = StartCoroutine(UpdateWeightsCoroutine(_approachWeight, 2.0f));
                _approachStateCountdownCoroutine = StartCoroutine(ApproachStateCountdown(0));
                break;
            case InteractorState.Rise:
                break;
            case InteractorState.Touch:
                break;
            case InteractorState.Reset:
                if(_updateWeightsCoroutine !=null)
                {
                    StopCoroutine(_updateWeightsCoroutine);
                }
                _updateWeightsCoroutine = StartCoroutine(UpdateWeightsCoroutine(0, 10.0f));
                _resetStateCountdownCoroutine = StartCoroutine(ResetStateCountdownCoroutine(10));
                break;
        }
    }

    IEnumerator ResetStateCountdownCoroutine(int countdownTime)
    {
        yield return new WaitForSeconds(countdownTime);
        ChangeState(InteractorState.Search);
        yield return null;
    }

    IEnumerator UpdateWeightsCoroutine(float targetWeight, float duration)
    {
        float elapsed = 0.0f;
        float speedAdjustedDuration = duration / Mathf.Max(1.0f, Mathf.Min(4.0f, _rb.velocity.magnitude));

        while (elapsed <duration)
        {
            _ikConstraint.weight = Mathf.Lerp(_ikConstraint.weight, targetWeight, elapsed / duration);
            _multiRotationConstraint.weight = Mathf.Lerp(_multiRotationConstraint.weight, targetWeight, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator ApproachStateCountdown(int countdownTime)
    {
        yield return new WaitForSeconds(countdownTime);
        ChangeState(InteractorState.Reset);
        yield return null;
    }
}
