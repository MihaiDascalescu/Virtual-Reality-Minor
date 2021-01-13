using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCast : MonoBehaviour
{
    [SerializeField] private string[] hittableTags = {"Enemy", "TargetPractice", "MovableTarget","EnemyProjectile"};

    public void CheckForHit(Vector3 origin, Vector3 direction, RaycastHit rayHit, float range, LayerMask whatIsEnemy, int damage)
    {
        if (Physics.Raycast(origin, direction, out rayHit, range, whatIsEnemy))
        {
            if (rayHit.collider.gameObject.layer == 16)
            {
                return;
            }
            foreach (string hittableTag in hittableTags)
            {
                if (!rayHit.collider.CompareTag(hittableTag))
                {
                    continue;
                }

                if (hittableTag == "EnemyProjectile")
                {
                    GameObject projectile = rayHit.collider.gameObject;
                    Destroy(projectile);
                }
                Health health = rayHit.collider.GetComponent<Health>();

                if (health == null)
                {
                    continue;
                }

                health.CurrentHealth -= damage;
            }
        }
    }
}
