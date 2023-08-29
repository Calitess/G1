using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class simpleNavFollow : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
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

    private void FollowPlayer()
    {
        if(Vector3.Distance(playerTransform.position, this.transform.position) > 2f)
        {
            npcAnimator.SetFloat("InputMagnitude",0.4f);
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            StopFollowing();
        }
    }

    private void StopFollowing()
    {
        npcAnimator.SetFloat("InputMagnitude", 0f);
        agent.stoppingDistance = 2;
    }
}
