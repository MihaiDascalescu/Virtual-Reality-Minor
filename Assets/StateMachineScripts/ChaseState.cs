using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;

namespace StateMachineScripts
{
    public class ChaseState : BaseState
    {
        private Demon demon;
        private Transform transform;
        private float distance;

        public ChaseState(Demon demon)
        {
            this.demon = demon;
            this.transform = demon.transform;
        }

        public override Type Tick()
        {
            if (demon.animator.GetBool("isHit"))
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
                demon.animator.SetBool("isInAttackRange",true);
                demon.animator.SetBool("isLookingForEnemies",false);
                return typeof(AttackState);
            }
            else
            {
                demon.agent.isStopped = false;
                return typeof(ChaseState);
            }

            return null;
        }
    }
}