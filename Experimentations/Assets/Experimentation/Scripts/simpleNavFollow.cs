using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class simpleNavFollow : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;
    [HideInInspector] public Transform targetPosition;
    [HideInInspector] public float distanceFromTarget;
    [SerializeField] private Animator npcAnimator;


    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        if(Vector3.Distance(targetPosition.position, this.transform.position) > distanceFromTarget)
        {
            npcAnimator.SetFloat("InputMagnitude",0.4f);
            agent.SetDestination(targetPosition.position);
        }
        else
        {
            StopFollowing();
        }
    }

    private void StopFollowing()
    {
        npcAnimator.SetFloat("InputMagnitude", 0f);
        agent.stoppingDistance = distanceFromTarget;
    }


}
