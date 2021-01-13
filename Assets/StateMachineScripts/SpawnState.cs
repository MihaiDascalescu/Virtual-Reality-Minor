using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineScripts
{
    public class SpawnState : BaseState
    {
        private readonly Boss boss;
        
        private readonly int maxEnemiesSpawned;

        private Demon demon;

        private bool IsSpawning()
        {
            return boss.demonAmount < maxEnemiesSpawned;
        }

        private float timeBetweenSpawns;
        
        
        public SpawnState(Boss boss)
        {
            this.boss = boss;
            timeBetweenSpawns = GameSettings.TimeBetweenEnemiesSpawned;
            maxEnemiesSpawned = GameSettings.MaxEnemiesSpawned;
            
        }
        public override Type Tick()
        {
            if (IsSpawning())
            {
                SpawnEnemies();
                boss.smokeEffect.SetActive(true);
            }
            else
            {
                boss.smokeEffect.SetActive(false);
                return typeof(FireProjectilesState);
            }
            timeBetweenSpawns-= Time.deltaTime;
            boss.transform.LookAt(boss.player.transform);
            if (boss.demonAmount == maxEnemiesSpawned)
            {
                
            }
            return null;
            
        }

        private void SpawnEnemies()
        {
            if(timeBetweenSpawns <= 0)
            {
                for (var i = 0; i < maxEnemiesSpawned; i++)
                {
                    boss.SpawnEnemies(i);
                    boss.demonAmount += 1;
                    timeBetweenSpawns = GameSettings.TimeBetweenEnemiesSpawned;
                }
            }
        }
        
    }
}
