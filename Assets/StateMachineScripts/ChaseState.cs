using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;

namespace StateMachineScripts
{
    public class ChaseState : BaseState
    {
        private static readonly int IsInAttackRange = Animator.StringToHash("isInAttackRange");
        private static readonly int IsLookingForEnemies = Animator.StringToHash("isLookingForEnemies");
        private static readonly int IsHit = Animator.StringToHash("isHit");
        private Demon demon;
        private Transform transform;
        private float distance;
        private static readonly int IsInRangedRange = Animator.StringToHash("IsInRangedRange");


        public ChaseState(Demon demon)
        {
            this.demon = demon;
            this.transform = demon.transform;
        }

        public override Type Tick()
        {
            if (demon.animator.GetBool(IsHit))
            {
                return null;
            }
            if (demon.Target == null)
            {
                return typeof(WanderState);
            }
            
            transform.LookAt(demon.Target);
            var position = demon.Target.transform.position;
            demon.agent.SetDestination(position);
            distance = Vector3.Distance(demon.transform.position, position);
            
            if (distance <= GameSettings.AttackRange)
            {
                demon.agent.isStopped = true;
                demon.animator.SetBool(IsInAttackRange,true);
                demon.animator.SetBool(IsLookingForEnemies,false);
                return typeof(AttackState);
            }

            if (distance <= GameSettings.RangedAttackRange)
            {
                demon.agent.isStopped = true;
                demon.animator.SetBool(IsInRangedRange,true);
                demon.animator.SetBool(IsLookingForEnemies,false);
                return typeof(ThrowState);
            }

            if (demon.Target.transform.position.y > 2)
            {
                return typeof(ThrowState);
            }
            demon.agent.isStopped = false;
            return typeof(ChaseState);

            return null;
        }
    }
}