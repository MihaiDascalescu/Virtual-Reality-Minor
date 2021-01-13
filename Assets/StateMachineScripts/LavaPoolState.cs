using System;

namespace StateMachineScripts
{
    public class LavaPoolState : BaseState
    {
        private Boss boss;

        public LavaPoolState(Boss boss)
        {
            this.boss = boss;
        }
        
        public override Type Tick()
        {
            boss.EnableLavaPool();

            if (!boss.lavaPool.activeInHierarchy)
            {
                return typeof(FireProjectilesState);
            }

            return null;
        }
    }
}
