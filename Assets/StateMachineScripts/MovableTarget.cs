using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineScripts
{
    [RequireComponent(typeof(Animator),typeof(Health),typeof(StateMachine))]
    public class MovableTarget : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("IsHit");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        
        public Animator animator;

        public Health health;

        public NavMeshAgent agent;

        public GameObject[] walkpoints;
        
        public StateMachine StateMachine => GetComponent<StateMachine>();
        // Start is called before the first frame update
        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            health.HealthNegativelyChanged += OnHealthNegativelyChanged;
            health.Died += OnDead;
        }
        
        private void OnDisable()
        {
            health.HealthNegativelyChanged -= OnHealthNegativelyChanged;
            health.Died -= OnDead;
        }
        private void OnDead()
        {
            animator.SetBool(IsDead,true);
            StartCoroutine(IfIsDead());
        }

        private void Start()
        {
            InitializeStateMachine();
        }

        private IEnumerator IfIsDead()
        {
            yield return new WaitForSeconds(2);
            animator.SetBool(IsDead,false);
            gameObject.SetActive(false);
        }

        private void InitializeStateMachine()
        {
            var states = new Dictionary<Type, BaseState>()
            {
                {typeof(WalkAround), new WalkAround(this)}
            };
            GetComponent<StateMachine>().Init(typeof(WalkAround), states);
        }
        
        private void OnHealthNegativelyChanged(int damage)
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
