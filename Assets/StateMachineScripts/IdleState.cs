using System;
using UnityEngine;

namespace StateMachineScripts
{
    public class IdleState : BaseState
    {
        private MonoBehaviour demon;
        
        public IdleState(MonoBehaviour demon)
        {
            this.demon = demon;
        }
        
        public override Type Tick()
        {
            return null;
        }
    }
}
