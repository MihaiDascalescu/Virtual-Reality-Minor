using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectilePool : MonoBehaviour
{
    public static BossProjectilePool bossProjectilePool;

    [SerializeField] private GameObject pooledProjectile;

    private bool notEnoughProjectilesInPool = true;

    private List<GameObject> projectiles = new List<GameObject>();

    private void Awake()
    {
        bossProjectilePool = this;
    }

    public GameObject GetProjectile()
    {
        if (projectiles.Count > 0)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].activeInHierarchy)
                {
                    return projectiles[i];
                }
            }
        }

        if (notEnoughProjectilesInPool)
        {
            GameObject projectile = Instantiate(pooledProjectile);
            projectile.SetActive(false);
            projectiles.Add(projectile);
            return projectile;
        }

        return null;
    }
}
