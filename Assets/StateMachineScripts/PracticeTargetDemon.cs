using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineScripts
{
    [RequireComponent(typeof(Animator),typeof(Health),typeof(StateMachine))]
    public class PracticeTargetDemon : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("isHit");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        
        public Animator animator;

        public Health health;
        
        public StateMachine StateMachine => GetComponent<StateMachine>();

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
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
                {typeof(IdleState), new IdleState(this)},
            };
            GetComponent<StateMachine>().Init(typeof(IdleState), states);
        }
        
        private void OnHealthNegativelyChanged(int damage)
        {
            StartCoroutine(IsHit());
        }
        private IEnumerator IsHit()
        {
            animator.SetBool(Hit,true);
            yield return new WaitForSeconds(1.0f);
            animator.SetBool(Hit,false);
        }
    }
}
