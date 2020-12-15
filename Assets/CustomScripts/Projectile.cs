using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private LayerMask playerLayer;
    [SerializeField] private int damage;
    
    private void OnTriggerEnter(Collider other)
    {
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
