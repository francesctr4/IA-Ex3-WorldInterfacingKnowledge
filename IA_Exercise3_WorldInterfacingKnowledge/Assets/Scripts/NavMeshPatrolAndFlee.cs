using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static NavMeshWanderAndSeek;

public class NavMeshPatrolAndFlee : MonoBehaviour
{
    // NavMesh parameters
    public WolfSpawner wolfSpawner;

    public Transform[] points;
    public Transform[] pointsReversed;

    private int destPoint = 0;
    private int destPointReversed = 0;

    private NavMeshAgent agent;
    private Animator animator;

    public LiamStates state;
    public bool liamDetected = false;

    public enum LiamStates
    {
        PATROL,
        FLEE
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).

        agent.autoBraking = false;

        // Random initial start point:
        //destPoint = Random.Range(0, points.Length - 1);

        // To unrandomize and start from the first point:
        GotoNextPoint();

        state = LiamStates.PATROL;
        animator.SetBool("IsDetected", false);
    }

    void Update()
    {

        if (wolfSpawner.detection)
        {
            state = LiamStates.FLEE;
        }
        else
        {
            state = LiamStates.PATROL;
        }

        switch (state)
        {
            case LiamStates.PATROL:
            {
                Patrol();
                break;
            }
            case LiamStates.FLEE:
            {
                Flee();
                break;
            }

        } 
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.

        destPoint = (destPoint + 1) % points.Length;
        
    }

    void GotoLastPoint()
    {
        // Returns if no points have been set up
        if (pointsReversed.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = pointsReversed[destPointReversed].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //destPointReversed = (destPointReversed + 1) % pointsReversed.Length;

        if (destPointReversed != pointsReversed.Length - 1)
        {
            destPointReversed++;
        }

    }

    public void Patrol()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        agent.speed = 3;
        animator.SetBool("IsDetected", false);
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }

    }

    public void Flee()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        agent.speed = 4.2f;
        animator.SetBool("IsDetected", true);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoLastPoint();
        }
    }


}
