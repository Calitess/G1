using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPCRandomMove : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 ranPos;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        StartCoroutine(CheckValidDestination());

    }

    IEnumerator CheckValidDestination()
    {

        ranPos = RandomNavmeshLocation(Random.Range(2f, 10f));

        yield return new WaitUntil(()=>!agent.pathPending);


        if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            ranPos = RandomNavmeshLocation(Random.Range(2f, 10f));
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(ranPos);

        }

        if (Vector3.Distance(ranPos, this.transform.position) > agent.stoppingDistance)
        {
            animator.SetFloat("InputMagnitude", 0.2f);
        }
        else
        {
            animator.SetFloat("InputMagnitude", 0f);
        }

        yield return new WaitUntil(() => ReachedDestinationOrGaveUp());


        StartCoroutine(ChooseToWait(Random.Range(0f,3f)));


    }

    IEnumerator ChooseToWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StartCoroutine(CheckValidDestination());
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool ReachedDestinationOrGaveUp()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnDrawGizmos()
        {
            if (ranPos != null)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, ranPos);
            }
        }
   
}
