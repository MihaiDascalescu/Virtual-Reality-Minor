using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StateMachineScripts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.WSA;

namespace StateMachineScripts
{
    public class Demon : MonoBehaviour
    {
        private static readonly int IsInAttackRange = Animator.StringToHash("isInAttackRange");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        
        public static readonly int Hit = Animator.StringToHash("isHit");
        public Transform Target { get; private set; }
        public StateMachine StateMachine => GetComponent<StateMachine>();

        private bool alreadyAttacked;

        private bool alreadyRangedAttack;

        [SerializeField]private GameObject demonRightHand;
        private GameObject projectile;

        public GameObject[] walkpoints;

        public NavMeshAgent agent;

        public Animator animator;

        public LayerMask whatIsPlayer;

        public Health health;
        
        public Player player;
        private static readonly int IsInRangedRange = Animator.StringToHash("IsInRangedRange");
        public bool wasAttacked = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
           
        }

        private void OnEnable()
        {
            StateMachine.StateChanged += OnStateChanged;
            health.Died += OnDead;
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            StateMachine.StateChanged -= OnStateChanged;
            health.Died -= OnDead;
            health.Damaged += OnDamaged;
        }

        private void Start()
        {
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            var states = new Dictionary<Type, BaseState>()
            {
                {typeof(WanderState), new WanderState(this)},
                {typeof(ChaseState), new ChaseState(this)},
                {typeof(AttackState), new AttackState(this)},
                {typeof(ThrowState), new ThrowState(this)}
            };
            GetComponent<StateMachine>().Init(typeof(WanderState), states);
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        public void Attack()
        {
            agent.SetDestination(Target.transform.position);
            transform.LookAt(Target.transform);
            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                StartCoroutine(ResetAttack());
            }
        }

        public void ThrowProjectile()
        {
            transform.LookAt(Target.transform);
            if (!alreadyRangedAttack)
            {
                alreadyRangedAttack = true;
                StartCoroutine(ResetRangedAttack());
            }
        }

        private IEnumerator ResetRangedAttack()
        {
            yield return new WaitForSeconds(GameSettings.RangedTimeBetweenAttacks);
            alreadyRangedAttack = false;
        }

        private IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(GameSettings.TimeBetweenAttacks);
            alreadyAttacked = false;
        }
        private void OnStateChanged(BaseState nextState)
        {
           //Debug.Log($"Demon state changed to {nextState}");
           print($"Demon state changed to {nextState}");
        }
        //This is called from animation event
        public void AttackEnd()
        {
            if (Physics.CheckSphere(transform.position, GameSettings.AttackRange, whatIsPlayer))
            {
                // TODO: Safety checks -> what if there is no player or health component
                Target.GetComponent<Player>().GetComponent<Health>().CurrentHealth -= 2;
                return;
            }
            animator.SetBool(IsInAttackRange, false);
        }
        //This is called from animation event
        public void AttackStart()
        {
            if (Physics.CheckSphere(transform.position, GameSettings.AttackRange, whatIsPlayer))
            {
                return;
            }
            animator.SetBool(IsInAttackRange, false);
        }
        //Animation Events
        public void FireProjectile()
        {
            transform.LookAt(Target);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            projectile.transform.parent = null;
            rb.AddForce((transform.forward - GameSettings.ProjectileOffset) * GameSettings.ShotPower ,ForceMode.Impulse);
            rb.AddForce(transform.up * GameSettings.VerticalShotPower,ForceMode.Impulse);
            agent.isStopped = false;
        }

        public void CreateProjectile()
        {
            agent.isStopped = true;
            projectile = Instantiate(GameSettings.DemonProjectilePrefab, demonRightHand.transform.position, Quaternion.identity,transform);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            if (animator.GetBool(IsInAttackRange))
            {
                Destroy(projectile);
                animator.SetBool(IsInRangedRange,false);
            }
            Destroy(projectile, 5.0f);
        }

        private void OnDead()
        {
            animator.SetBool(IsDead,true);
            Destroy(gameObject,2.0f);
        }

        private void OnDamaged(int damage)
        {
            StartCoroutine(IsHit());
            wasAttacked = true;
        }

        private IEnumerator IsHit()
        {
            animator.SetBool(Hit,true);
            yield return new WaitForSeconds(1);
            animator.SetBool(Hit,false);
        }
    }
}
