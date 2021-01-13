using System;
using System.Collections;
using UnityEngine;

namespace StateMachineScripts
{
    public class FireProjectilesState : BaseState
    {
        private Boss boss;
        private int iteration = 5;

        private Coroutine fireCoroutineHandle;

        public FireProjectilesState(Boss boss)
        {
            this.boss = boss;
        }

        public override void OnEnterState()
        {
            if (fireCoroutineHandle != null)
            {
                Debug.LogWarning("Fire coroutine was not stopped when entering the fire state! This should never happen!");
                boss.StopCoroutine(fireCoroutineHandle);
            }

            fireCoroutineHandle = boss.StartCoroutine(FireCoroutine());
        }

        public override Type Tick()
        {
            if (boss.demonAmount < boss.spawnStateStartThreshold)
            {
                return typeof(SpawnState);
            }
            
            return null;
        }

        public override void OnExitState()
        {
            boss.StopCoroutine(fireCoroutineHandle);
            fireCoroutineHandle = null;
        }

        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                boss.FireInPattern();
                yield return new WaitForSeconds(2.0f);
            }
        }
    }
}
