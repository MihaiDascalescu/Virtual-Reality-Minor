using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineScripts
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private float demonSpeed = 2.0f;
        public static float DemonSpeed => Instance.demonSpeed;

        [SerializeField] private float aggroRadius = 4.0f;
        public static float AggroRadius => Instance.aggroRadius;

        [SerializeField] private float attackRange = 5.0f;
        public static float AttackRange => Instance.attackRange;

        [SerializeField] private float rangedAttackRange = 10.0f;
        public static float RangedAttackRange => Instance.rangedAttackRange;

        [SerializeField] private float rangedTimeBetweenAttacks;
        public static float RangedTimeBetweenAttacks => Instance.rangedTimeBetweenAttacks;

        [SerializeField] private float shotPower;
        public static float ShotPower => Instance.shotPower;
        [SerializeField] private float verticalShotPower;
        public static float VerticalShotPower => Instance.verticalShotPower;
        [SerializeField] private Vector3 projectileOffset;
        public static Vector3 ProjectileOffset => Instance.projectileOffset;

        [SerializeField] private GameObject demonProjectilePrefab;
        public static GameObject DemonProjectilePrefab => Instance.demonProjectilePrefab;

        [SerializeField] private float timeBetweenAttacks;
        public static float TimeBetweenAttacks => Instance.timeBetweenAttacks;


        public static GameSettings Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}