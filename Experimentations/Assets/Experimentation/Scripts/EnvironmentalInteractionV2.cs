using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


enum InteractorState2
{
    Search,
    Approach,
    Rise,
    Touch,
    Reset,
}

public class EnvironmentalInteractionV2 : MonoBehaviour
{
    [SerializeField] bool isRightDirection;

    float _wingspan, _armLength, _colliderCenterOffset;

    Collider _currentColliderTarget;
    Rigidbody _rb;

    Transform _rootTransform, _handTransform, _elbowTransform, _shoulderTransform, _ikTargetTransform;

    Vector3 _closestPointPosition = Vector3.positiveInfinity, _prepPointPosition;

    BoxCollider _boxCollider;

    float _approachingDistanceThreshold = 3, _approachWeight = 0.5f;

    private Coroutine _updateWeightsCoroutine, _resetStateCountdownCoroutine, _approachStateCountdownCoroutine;

    Vector3 _armForward;
    float _angle;

    [SerializeField] float _wallTouchHeight;

    MultiRotationConstraint _multiRotationConstraint;
    TwoBoneIKConstraint _ikConstraint;

    //State Machine
    InteractorState2 _currentState = InteractorState2.Reset;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _ikConstraint = GetComponent<TwoBoneIKConstraint>();
        _multiRotationConstraint = GetComponent<MultiRotationConstraint>();
        _rootTransform = transform.root;
        _wingspan = _rootTransform.GetComponent<CapsuleCollider>().height;
        _colliderCenterOffset = _wingspan / 3;


        _shoulderTransform = _ikConstraint.data.root.transform;
        _elbowTransform = _ikConstraint.data.mid.transform;
        _handTransform = _ikConstraint.data.tip.transform;

       

        Vector3 upperArm = _elbowTransform.position - _shoulderTransform.position;
        Vector3 forearm = _handTransform.position - _elbowTransform.position;

        _ikTargetTransform = _ikConstraint.data.target.transform;
        //ConstructColliders();

    }


    private void ConstructColliders()
    {
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.center = new Vector3(_elbowTransform.position.x*1.5f, _colliderCenterOffset, _wingspan/3);
        _boxCollider.size = new Vector3(.3f, _wingspan/4, _wingspan/2);
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArmTarget") && !_currentColliderTarget)// && _currentState != InteractorState2.Reset)
        {
            _currentColliderTarget = other;
            //_armForward = isRightDirection ? _rootTransform.right : -_rootTransform.right;
            //_angle = Vector3.Angle(_armForward, toClosestPoint);

            StartCoroutine(LerpFunction(1, 2, "touching Wall"));

            _closestPointPosition = other.ClosestPoint(new Vector3(_shoulderTransform.position.x, _shoulderTransform.position.y - _armLength - _wallTouchHeight, _shoulderTransform.position.z) + (_rb.velocity/10));
            _prepPointPosition = new Vector3(_closestPointPosition.x, 0, _closestPointPosition.z);
            _ikTargetTransform.position = _prepPointPosition;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == _currentColliderTarget)
        {
            _closestPointPosition = other.ClosestPoint(new Vector3(_shoulderTransform.position.x, _shoulderTransform.position.y - _wallTouchHeight, _shoulderTransform.position.z) + (_rb.velocity /10));
            _prepPointPosition = new Vector3(_closestPointPosition.x, _closestPointPosition.y, _closestPointPosition.z);
            _ikTargetTransform.position = _prepPointPosition;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _currentColliderTarget)
        {
            _currentColliderTarget = null;
            _closestPointPosition = Vector3.positiveInfinity;
            //ChangeState(InteractorState2.Reset);
            StartCoroutine(LerpFunction(0, 2, "letting go"));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_closestPointPosition != null)
        {
            Gizmos.DrawSphere(_closestPointPosition, .1f);
        }
    }

    IEnumerator LerpFunction(float endValue,float duration, string comment)
    {
        float time = 0;
        float startValue = _ikConstraint.weight;
        while (time < duration)
        {
            _ikConstraint.weight = Mathf.Lerp(startValue, endValue, time / duration);
            _multiRotationConstraint.weight = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
            Debug.Log(comment);
        }
        _ikConstraint.weight = endValue;
        _multiRotationConstraint.weight = endValue;
    }

}

