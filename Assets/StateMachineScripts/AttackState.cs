using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;

namespace StateMachineScripts
{
    public class AttackState : BaseState
    {
        private const float TimeBetweenAttacks = 5.0f;
        
        private static readonly int IsInAttackRange = Animator.StringToHash("isInAttackRange");
        
        private float attackReadyTimer = TimeBetweenAttacks;
        
        private readonly Demon demon;
        
        private Transform transform;

        public AttackState(Demon demon)
        {
            this.demon = demon;
            this.transform = demon.transform;
        }

        public override Type Tick()
        {
            if (demon.Target == null)
            {
                return typeof(WanderState);
            }

            attackReadyTimer -= Time.deltaTime;
            var distance = Vector3.Distance(demon.transform.position, demon.Target.transform.position);
            if (distance > GameSettings.AttackRange)
            {
                return typeof(ChaseState);
            }
            if (attackReadyTimer <= 0)
            {
                demon.Attack();
                demon.animator.SetBool(IsInAttackRange,true);
                attackReadyTimer = TimeBetweenAttacks;
            }

            if (distance > GameSettings.AttackRange && distance < GameSettings.RangedAttackRange)
            {
                return typeof(ThrowState);
            }

            return null;
        }
       
    }
}