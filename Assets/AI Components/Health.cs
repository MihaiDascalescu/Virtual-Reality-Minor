using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int> HealthChanged;
    public event Action Died;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value == currentHealth)
            {
                return;
            }

            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            
            HealthChanged?.Invoke(currentHealth);

            if (currentHealth == 0)
            {
                Died?.Invoke();
            }
        }
    }

    public int MaxHealth => maxHealth;
    
    [SerializeField] private int maxHealth;

    public int currentHealth;
    
    private void Start()
    {
        // Set the property to the max health. This will call the HealthChanged event to update UI for example.
        // This happens on start so that everyone has a chance to register to the event onEnable or on awake first, otherwise there will be no listeners :)
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        DestroyIfDead();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthChanged?.Invoke(damage);
    }

    private bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void SetHealthToFull()
    {
        currentHealth = maxHealth;
    }

    public void DestroyIfDead()
    {
        if (IsDead())
        {
            Died?.Invoke();
        }
    }
}
