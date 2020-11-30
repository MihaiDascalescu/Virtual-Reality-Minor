﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            health.HealthChanged += OnHealthChanged;
            health.Died += OnDead;
        }

        private void OnDead()
        {
            animator.SetBool(IsDead,true);
            StartCoroutine(IfIsDead());
        }

        private void OnDisable()
        {
            health.HealthChanged -= OnHealthChanged;
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
                {typeof(IdleState), new IdleState(this)}
            };
            GetComponent<StateMachine>().Init(typeof(IdleState), states);
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