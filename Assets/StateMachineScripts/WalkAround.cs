using System;
using UnityEngine;

namespace StateMachineScripts
{
    public class WalkAround : BaseState
    {
        public MovableTarget movableTargetDemon;

        public Transform transform;

        private int destPoint;

        public WalkAround(MovableTarget movableTargetDemon)
        {
            this.movableTargetDemon = movableTargetDemon;
            transform = movableTargetDemon.transform;
        }
        public override Type Tick()
        {
            Patrol();
            return null;
        }
        private void Patrol()
        {
            if (!movableTargetDemon.agent.pathPending && movableTargetDemon.agent.remainingDistance < 1.0f)
                GotoNextPoint();
        }
        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (movableTargetDemon.walkpoints.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            movableTargetDemon.agent.destination = movableTargetDemon.walkpoints[destPoint].transform.position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % movableTargetDemon.walkpoints.Length;
        }
    }
}
