﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Management;


public class EnemyAiStateMachine : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public int health;
    
    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public GameObject[] walkPoints;
    
    public GameObject projectile;
    
    public Vector3 walkPoint;
    private bool isWalkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    public float sightRange, attackRange;
    private bool isPlayerInSightRange, isPlayerInAttackRange;

    

    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!player.gameObject.GetComponent<Player>().IsPlayerDead())
        {
            if (!animator.GetBool("isHit"))
            {
                
                if (!isPlayerInAttackRange && !isPlayerInSightRange)
                {
                    animator.SetBool("isLookingForEnemies", true);
                    animator.SetBool("isInAttackRange", false);
                    
                    Patrol();
                }

                if (!isPlayerInAttackRange && isPlayerInSightRange)
                {
                    animator.SetBool("isInAttackRange", false);

                    Chase();
                }

                if (isPlayerInAttackRange && isPlayerInSightRange)
                {
                    animator.SetBool("isLookingForEnemies", false);
                    animator.SetBool("isInAttackRange", true);

                    Attack();
                }
                else
                {
                    animator.SetBool("isInAttackRange", false);
                }
            }
        }
        else
        {
            animator.SetBool("isPlayerDead",true);
        }
    }
    /// <summary>
    ///Make Sepparate Component Patroler;
    ///Patrol Pattern 
    /// </summary>
    private void Patrol()
    {
        if (!isWalkPointSet)
        {
            SearchWalkPoint();
        }

        if (isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1)
        {
            isWalkPointSet = false;
        }
    }

    void Guard()
    {
        
    }

    private void SearchWalkPoint()
    {
        float random = UnityEngine.Random.Range(1, 3);
        float randomPointX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomPointZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomPointX,transform.position.y ,transform.position.z + randomPointZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            isWalkPointSet = true;
        } 
        
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        animator.SetBool("isHit",true);
        animator.SetBool("isLookingForEnemies",false);
        health -= damage;
        Invoke(nameof(DisableOnHit),1.0f);
        if (health <= 0)
        {
            //animator.SetBool("isHit",false);
            animator.SetBool("isDead",true);
            Invoke(nameof(DestroyEnemy),2.0f);
        }
        // animator.SetBool("isHit", false);
        animator.SetBool("isLookingForEnemies",true);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void DisableOnHit()
    {
        animator.SetBool("isHit", false);
    }

    public void AttackEnd()
    {
        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
        {
            player.gameObject.GetComponent<Player>().OnHit(2);
        }
        else
        {
            animator.SetBool("isInAttackRange",false);
        }
    }

    
}