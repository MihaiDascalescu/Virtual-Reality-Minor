using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Patrol), typeof(TargetDetector),typeof(Chase))]
public class Enemy : MonoBehaviour
{
    private Transform trackedTarget;
    private Patrol patrol;
    private TargetDetector targetDetector;
    private Chase chase;
    public Attack attack;
    private Animator animator;
    
    private void Awake()
    {
        patrol = GetComponent<Patrol>();
        targetDetector = GetComponent<TargetDetector>();
        chase = GetComponent<Chase>();
        attack = GetComponent<Attack>();
        animator = GetComponent<Animator>();
        attack.enabled = false;
        animator.SetBool("isLookingForEnemies",true);
    }

    private void OnEnable()
    {
        targetDetector.TargetDetected += OnTargetDetected;
        targetDetector.TargetLost += OnTargetLost;
        chase.TargetInAttackRange += OnTargetInAttackRange;
    }

    private void OnDisable()
    {
        targetDetector.TargetDetected -= OnTargetDetected;
        targetDetector.TargetLost -= OnTargetLost;
        chase.TargetInAttackRange -= OnTargetInAttackRange;
    }

    private void OnTargetDetected(Collider target)
    {
        if (!target.GetComponent<Player>())
        {
            return;
        }

        // Already chasing a target
        if (chase.trackedTarget != null)
        {
            return;
        }
        
        chase.trackedTarget = target.transform;

        patrol.enabled = false;
        chase.enabled = true;

        animator.SetBool("isLookingForEnemies", true);
        animator.SetBool("isInAttackRange",false);
        print("STOP BITCH!");
    }

    private void OnTargetLost(Collider target)
    {
        if (target != null && chase.trackedTarget != target.transform)
        {
            print("The target lost was null or different than the tracked target");
            return;
        }
        
        chase.enabled = false;
        attack.Target = null;
        attack.enabled = false;
        patrol.enabled = true;
        animator.SetBool("isLookingForEnemies",true);
        animator.SetBool("isInAttackRange",false);
        print("Lost current target...");
    }

    private void OnTargetInAttackRange(float attackRange)
    {
        patrol.enabled = false;
        chase.enabled = false;
        
        attack.Target = chase.trackedTarget.GetComponent<Health>();
       

        if (attack.Target != null)
        {
            animator.SetBool("isInAttackRange",true);
            animator.SetBool("isLookingForEnemies", false);
            animator.SetBool("isDead",false);
            attack.enabled = true;
        }

        

        print(attack.enabled);
    }
}
