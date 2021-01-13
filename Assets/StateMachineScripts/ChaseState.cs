using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineScripts
{
    public class ChaseState : BaseState
    {
        private static readonly int IsInAttackRange = Animator.StringToHash("isInAttackRange");
        private static readonly int IsLookingForEnemies = Animator.StringToHash("isLookingForEnemies");
        private static readonly int IsHit = Animator.StringToHash("isHit");
        private Demon demon;
        private Transform transform;
        private static readonly int IsInRangedRange = Animator.StringToHash("IsInRangedRange");


        public ChaseState(Demon demon)
        {
            this.demon = demon;
            this.transform = demon.transform;
        }
        NavMeshPath path = new NavMeshPath();
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

            Vector3 toTarget = demon.Target.transform.position - transform.position;
            float distance = toTarget.magnitude;
            
            toTarget.y = 0.0f;
            Quaternion rotationTowardsTarget = Quaternion.LookRotation(toTarget);
            transform.rotation = rotationTowardsTarget;

            if (demon.agent.CalculatePath(demon.Target.transform.position, path))
            {
                demon.agent.SetPath(path);
            }

            // Agent can reach player
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                if (distance <= GameSettings.AttackRange)
                {
                    demon.animator.SetBool(IsInAttackRange, true);
                    demon.animator.SetBool(IsLookingForEnemies, false);
                    return typeof(AttackState);
                }

                if (distance <= GameSettings.RangedAttackRange)
                {
                    demon.animator.SetBool(IsInRangedRange, true);
                    demon.animator.SetBool(IsLookingForEnemies, false);
                    return typeof(ThrowState);
                }
            }
            else
            {
                if (demon.agent.remainingDistance <= 1.0f)
                {
                    // TODO!~
                    //return typeof(IdleState);
                }
            }

            return typeof(ChaseState);
        }

        public override void OnExitState()
        {
            demon.agent.isStopped = true;
        }

        public override void OnEnterState()
        {
            demon.agent.isStopped = false;
        }
    }
}