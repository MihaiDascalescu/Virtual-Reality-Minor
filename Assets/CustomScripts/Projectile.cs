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
        if (other.gameObject.layer == playerLayer)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
