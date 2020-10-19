using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemySimpleAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public int health;
    
    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator; 
        
    public GameObject projectile;
    
    public Vector3 walkPoint;
    private bool _isWalkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool _alreadyAttacked;

    public float sightRange, attackRange;
    private bool _isPlayerInSightRange, _isPlayerInAttackRange;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {


        _isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        _isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!player.gameObject.GetComponent<Player>().IsPlayerDead())
        {
            if (!animator.GetBool("isHit"))
            {
                if (!_isPlayerInAttackRange && !_isPlayerInSightRange)
                {
                    animator.SetBool("isLookingForEnemies", true);
                    animator.SetBool("isInAttackRange", false);

                    Patrol();
                }

                if (!_isPlayerInAttackRange && _isPlayerInSightRange)
                {
                    animator.SetBool("isInAttackRange", false);

                    Chase();
                }

                if (_isPlayerInAttackRange && _isPlayerInSightRange)
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

    private void Patrol()
    {
        if (!_isWalkPointSet)
        {
            SearchWalkPoint();
        }

        if (_isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1)
        {
            _isWalkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float random = UnityEngine.Random.Range(1, 3);
        float randomPointX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomPointZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomPointX,transform.position.y ,transform.position.z + randomPointZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            _isWalkPointSet = true;
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
        if (!_alreadyAttacked)
        {
            
            /*Rigidbody rigidbody = Instantiate(projectile, transform.position, Quaternion.identity)
                .GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * 32f,ForceMode.Impulse);*/
            
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
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

    private void OnTriggerEnter(Collider other)
    {
        //if(other.Co)
    }
}
