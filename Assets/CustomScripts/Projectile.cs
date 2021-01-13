using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private LayerMask playerLayer;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask wallLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        //00000000 00000001 10000001 00010000
        //00000000 00000000 00000000 00000001 << 16 -> 00000000 00000001 00000000 00000000
        
        //00000000 00000001 10000001 00010000 &
        //00000000 00000001 00000000 00000000
        //-----------------------------------
        //00000000 00000001 00000000 00000000
        int collidedWithLayer = 1 << other.gameObject.layer;
        if ((wallLayerMask.value & collidedWithLayer) == collidedWithLayer)
        {
            Destroy(gameObject);
        }
        
        if (other.GetComponent<Player>() == null)
        {
            return;
        }

        Health health = other.GetComponent<Health>();
        if (health == null)
        {
            return;
        }

        health.CurrentHealth -= damage;
    }
}
