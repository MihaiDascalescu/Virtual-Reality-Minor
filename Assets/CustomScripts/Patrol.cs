using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Patrol : MonoBehaviour
{
    public NavMeshAgent agent;
    
    public UnityEvent playerInAttackRange;
    public UnityEvent playerInSight;
    
    public Animator animator;

    public GameObject[] walkPoints;
    
    private int destPoint;

    public int sightRange, attackRange;

    public LayerMask whatIsPlayer;

    private bool isPlayerInAttackRange, isPlayerInSightRange;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GotoNextPoint();
    }

    private void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(position: transform.position, radius: sightRange, layerMask: whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(position: transform.position, radius: attackRange, layerMask: whatIsPlayer);
        if (!isPlayerInAttackRange && !isPlayerInSightRange)
        {
            PatrolToWalkPoints();
            SetPatrollingAnimation();
        }
        if (!isPlayerInAttackRange && isPlayerInSightRange)
        {
            playerInSight.Invoke();
        }
        if (isPlayerInAttackRange && isPlayerInSightRange)
        {
            playerInAttackRange.Invoke();
        }
    }

    private void PatrolToWalkPoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 1.0f)
            GotoNextPoint();
    }

    private void SetPatrollingAnimation()
    {
        animator.SetBool(name: "isLookingForEnemies", value: true);
        animator.SetBool(name: "isInAttackRange", value: false);
    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (walkPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = walkPoints[destPoint].transform.position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % walkPoints.Length;
    }
}
