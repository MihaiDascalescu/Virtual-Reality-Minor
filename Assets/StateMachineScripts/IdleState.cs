using System;

namespace StateMachineScripts
{
    public class IdleState : BaseState
    {
        private PracticeTargetDemon practiceTargetDemon;
        // Start is called before the first frame update
        

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
