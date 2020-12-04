using System;
using UnityEngine;

namespace StateMachineScripts
{
    public class IdleState : BaseState
    {
        private PracticeTargetDemon practiceTargetDemon;
        
        public IdleState(PracticeTargetDemon practiceTargetDemon)
        {
            this.practiceTargetDemon = practiceTargetDemon;
        }
        
        public override Type Tick()
        {
            return null;
        }
    }
}
