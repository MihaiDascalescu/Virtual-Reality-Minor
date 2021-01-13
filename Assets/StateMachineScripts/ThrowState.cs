using System;
using Data.Util;
using UnityEngine;

namespace StateMachineScripts
{
    public class ThrowState : BaseState
    {
        private readonly Demon demon;

        private readonly Transform transform;

        private float attackReadyTimer = GameSettings.RangedTimeBetweenAttacks;
        private static readonly int IsInRangedRange = Animator.StringToHash("IsInRangedRange");


        public ThrowState(Demon demon)
        {
            this.demon = demon;
            this.transform = demon.transform;
        } 
        // Start is called before the first frame update
        
        public override Type Tick()
        {
            if (demon.Target == null)
            {
                return typeof(WanderState);
            }

            attackReadyTimer -= Time.deltaTime;
            var distance = Vector3.Distance(transform.position, demon.Target.transform.position);
            if (distance > GameSettings.RangedAttackRange)
            {
                return typeof(ChaseState);
            }
            if (distance <= GameSettings.AttackRange)
            {
                return typeof(AttackState);
            }
            if (attackReadyTimer <= 0)
            {
                demon.ThrowProjectile();
                demon.animator.SetBool(IsInRangedRange,true);
                attackReadyTimer = GameSettings.RangedTimeBetweenAttacks;
            }

           

           

            return null;
        }
    }
}
