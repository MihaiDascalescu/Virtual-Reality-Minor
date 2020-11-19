using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public event Action<float> TargetInAttackRange;
    public Transform trackedTarget;
    public float attackRange = 1.0f;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(trackedTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!trackedTarget)
        {
            return;
        }
        
        float distance = Vector3.Distance(transform.position, trackedTarget.position);
        
        if (!(distance < attackRange)) return;
        
        TargetInAttackRange?.Invoke(attackRange);
        
        print("attackRange");
    }
}
