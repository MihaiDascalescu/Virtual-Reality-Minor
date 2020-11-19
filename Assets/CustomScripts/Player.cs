using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{

    private Health health;

    private bool isDead = false;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.Died += Die;
    }

    private void OnDisable()
    {
        health.Died -= Die;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public bool IsPlayerDead()
    {
        if (health.currentHealth <= 0)
        {
            isDead = true;
        }

        return isDead;
    }


}
