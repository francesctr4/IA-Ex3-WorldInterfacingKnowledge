using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NavMeshWanderAndSeek : MonoBehaviour
{
    // Animation parameters
    private Animator animator;

    // NavMesh parameters
    public GameObject target;
    private NavMeshAgent agent;
    public LayerMask navMeshLayer;

    // Wandering parameters
    private Vector3 destinationPoint;
    private bool walkpointSet;
    public float range;

    // Time parameters
    private float waitingTime; 
    private float actualTime;

    private float waitingTimeStuck;
    private float actualTimeStuck;

    public WolfStates state;

    public enum WolfStates
    {
        WANDER,
        SEEK
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        waitingTime = 2f;
        actualTime = 0f;

        waitingTimeStuck = 5f;
        actualTimeStuck = 0f;

        animator.SetBool("IsWalking", true);

        state = WolfStates.WANDER;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case WolfStates.WANDER:
            { 
                Wander();
                break;
            }
            case WolfStates.SEEK:
            {
                Seek();
                break;
            }
        }
    }

    public void Wander()
    {
        animator.SetBool("IsSeeking", false);
        agent.speed = 1;

        if (!walkpointSet)
        {
            SearchForDestination();
        }

        if (walkpointSet)
        {
            agent.SetDestination(destinationPoint);
        }

        actualTimeStuck += Time.deltaTime;

        if (Vector3.Distance(transform.position, destinationPoint) < 0.1 || actualTimeStuck > waitingTimeStuck)
        {
            actualTime += Time.deltaTime;

            animator.SetBool("IsWalking", false);

            if (actualTime > waitingTime)
            {
                animator.SetBool("IsWalking", true);
                walkpointSet = false;
                
                actualTime = 0f;
                actualTimeStuck = 0f;
            }
        }

    }

    private void SearchForDestination()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destinationPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destinationPoint, Vector3.down, navMeshLayer))
        {
            walkpointSet = true;
        }
    }

    public void Seek()
    {
        animator.SetBool("IsSeeking", true);
        agent.SetDestination(target.transform.position);
        agent.speed = 4;
    }

}
