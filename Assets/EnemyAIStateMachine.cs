using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Management;



public class EnemyAIStateMachine : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public int health;
    
    public LayerMask whatIsGround, whatIsPlayer,whatIsWalkPoint;

    public Animator animator;

    public GameObject[] walkPoints;
    
    public GameObject projectile;
    private bool walkPointReached = false;
    public Vector3 walkPoint;
    private bool isWalkPointSet;
    public float walkPointRange;
    private GameObject walkPointer;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    public float sightRange, attackRange;
    private bool isPlayerInSightRange, isPlayerInAttackRange;
    private int destPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
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

    

    /*void Patrol()
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
    }*/
    /*Vector3 distanceToWalkPoint = new Vector3();
      if (walkPoints == null) return;
      int randomIndex = UnityEngine.Random.Range(0, walkPoints.Length - 1);
      if (!isWalkPointSet)
      {
          agent.SetDestination(walkPoints[randomIndex].transform.position);
      }
      if (isWalkPointSet == true)
      {
          isWalkPointSet = false;
      }
      if (Physics.CheckSphere(transform.position,1,whatIsWalkPoint))
      {
          isWalkPointSet = false;
      }
      if (!isWalkPointSet)
      {
          for (int i = 0; i < walkPoints.Length; i++)
          {
              walkPointer = walkPoints[i];
              agent.SetDestination(walkPointer.transform.position);
              
              
              if (i == walkPoints.Length)
              {
                  i = 0;
              }
          }
          isWalkPointSet = true;
      }

      distanceToWalkPoint = transform.position - walkPointer.transform.position;
      print(isWalkPointSet);*/
}
