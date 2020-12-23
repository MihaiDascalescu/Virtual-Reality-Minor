using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateMachineScripts
{
    public class WanderState : BaseState
    {
        
        private static readonly int IsInAttackRange = Animator.StringToHash("isInAttackRange");
        private static readonly int IsLookingForEnemies = Animator.StringToHash("isLookingForEnemies");
        private readonly LayerMask layerMask = LayerMask.NameToLayer("Walls");
        private static readonly int IsHit = Animator.StringToHash("isHit");

        private Quaternion desiredRotation;
        private Vector3 direction;
        private int destPoint;
        private Demon demon;
        private Transform demonTransform;
        private static readonly int IsInRangedRange = Animator.StringToHash("isInRangedRange");
        

        public WanderState(Demon demon)
        {
            this.demon = demon;
            this.demonTransform = demon.transform;
        }

       /* public WanderState(MonoBehaviour behaviour)
        {
            this.demon = behaviour;
            this.animator = behaviour.GetComponent<Animator>()
        }*/

       public override Type Tick()
        {
            
            if (demon.animator.GetBool(IsHit))
            {
                return null;
            }
            var chaseTarget = CheckForAggro();
            demon.animator.SetBool(IsLookingForEnemies,true);
            demon.animator.SetBool(IsInAttackRange,false);
            demon.animator.SetBool(IsInRangedRange,false);
            
            
            if (chaseTarget != null)
            {
                demon.SetTarget(chaseTarget);
                return typeof(ChaseState);
            }

            
            if (demon.wasAttacked)
            {
                demon.SetTarget(demon.player.transform);
                demon.wasAttacked = false;
                return typeof(ChaseState);
            }
            Patrol();
            return null;
        }
        private void Patrol()
        {
            if (!demon.agent.pathPending && demon.agent.remainingDistance < 1.0f)
                GotoNextPoint();
        }
        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (demon.walkpoints.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            demon.agent.destination = demon.walkpoints[destPoint].transform.position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % demon.walkpoints.Length;
        }
        private Transform CheckForAggro()
        {
            var pos = demonTransform.position;
            return Physics.CheckSphere(pos, GameSettings.AggroRadius, demon.whatIsPlayer) ? demon.player.transform : null;
        }
    }
}
