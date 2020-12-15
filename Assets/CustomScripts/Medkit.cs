using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] private int healAmount;
    
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player == null)
        {
            return;
        }

        Health health = other.GetComponent<Health>();

        if (health == null)
        {
            return;
        }

        health.CurrentHealth += healAmount;
        Destroy(gameObject);
    }
}
