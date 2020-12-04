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

        public GameObject[] walkpoints;

        public NavMeshAgent agent;

        public Animator animator;

        public LayerMask whatIsPlayer;

        public Health health;
        
        public Player player;
         

      
        

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
            health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            StateMachine.StateChanged -= OnStateChanged;
            health.Died -= OnDead;
            health.HealthChanged += OnHealthChanged;
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
                {typeof(AttackState), new AttackState(this)}
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
                Target.GetComponent<Player>().GetComponent<Health>().currentHealth -= 2;
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

        private void OnDead()
        {
            animator.SetBool(IsDead,true);
            Destroy(gameObject,2.0f);
        }

        private void OnHealthChanged(int damage)
        {
            StartCoroutine(IsHit());
        }

        private IEnumerator IsHit()
        {
            animator.SetBool(Hit,true);
            yield return new WaitForSeconds(1);
            animator.SetBool(Hit,false);
        }
    }
}
