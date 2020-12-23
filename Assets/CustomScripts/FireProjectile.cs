using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private int projectileAmount = 10;
    [SerializeField] private float startAngle = 0, endAngle = 360;
    [SerializeField] private float yOffset;
    private Vector3 projectileMoveDirection;
    

    private void Start()
    {
        InvokeRepeating("FireInPattern",0f,2f);
    }

    void FireInPattern()
    {
        float angleStep = (endAngle - startAngle) / projectileAmount ;
        float angle = startAngle;
        for (int i = 0; i < projectileAmount + 1; i++)
        {
            var position = transform.position;
            float bulDirX = position.x + Mathf.Sin((angle * Mathf.PI)/180);
            float bulDirZ = position.z + Mathf.Sin((angle * Mathf.PI)/180);
            
            Vector3 projectileMoveVector = new Vector3(bulDirX,0,bulDirZ);
            Vector2 projectileDir = (projectileMoveVector - position).normalized;

            GameObject projectile = BossProjectilePool.bossProjectilePool.GetProjectile();
            projectile.transform.position = position;
            projectile.transform.rotation = transform.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<BossProjectile>().SetMoveDirection(projectileDir);

            angle += angleStep;
        }
    }
}
