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
        

        private Quaternion desiredRotation;
        private Vector3 direction;
        private int destPoint;
        private Demon demon;
        private Transform demonTransform;
        

        public WanderState(Demon demon)
        {
            this.demon = demon;
            this.demonTransform = demon.transform;
        }

       

        public override Type Tick()
        {
            if (demon.animator.GetBool("isHit"))
            {
                return null;
            }
            var chaseTarget = CheckForAggro();
            demon.animator.SetBool(IsLookingForEnemies,true);
            demon.animator.SetBool(IsInAttackRange,false);
            
            
            if (chaseTarget != null)
            {
                demon.SetTarget(chaseTarget);
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
        private Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
        private Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
        

        private Transform CheckForAggro()
        {
            var pos = demonTransform.position;
            return Physics.CheckSphere(pos, GameSettings.AggroRadius, demon.whatIsPlayer) ? demon.player.transform : null;
        }
    }
}
/*RaycastHit hit;
           var angle = transform.rotation * startingAngle;
           var direction = angle * Vector3.forward;
           
           for (var i = 0; i < 24; i++)
           {
               if (Physics.Raycast(pos, direction, out hit, GameSettings.AggroRadius))
               {
                   var player = hit.collider.GetComponent<Player>();
                   if (player != null)
                   {
                       Debug.Log("player found");
                       Debug.DrawRay(pos, direction * hit.distance, Color.red);
                       return player.transform;
                   }
                   else
                   {
                       Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                   }
               }*/