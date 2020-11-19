using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Patrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject[] walkPoints;
    
    private int destPoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
    }
    private void Update()
    {
        PatrolToWalkPoints();
    }
    private void PatrolToWalkPoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 1.0f)
            GotoNextPoint();
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
