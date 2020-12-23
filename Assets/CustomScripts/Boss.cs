using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public StateMachine StateMachine => GetComponent<StateMachine>();
    
    public NavMeshAgent agent;

    public Animator animator;

    public LayerMask whatIsPlayer;

    public Health health;
        
    public Player player;

    [SerializeField] private Projectile projectile;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(IdleState), new IdleState(this)}
        };
        GetComponent<StateMachine>().Init(typeof(IdleState), states);
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

    private void OnDead()
    {
        throw new NotImplementedException();
    }

    private void OnStateChanged(BaseState obj)
    {
        throw new NotImplementedException();
    }
    private void OnDamaged(int obj)
    {
        throw new NotImplementedException();
    }
    
}
