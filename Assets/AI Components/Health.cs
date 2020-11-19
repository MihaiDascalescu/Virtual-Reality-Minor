using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> HealthChanged;
    public event Action Died;

    public float CurrentHealth
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

    public float MaxHealth => maxHealth;
    
    [SerializeField] private float maxHealth;

    public float currentHealth;
    
    private void Start()
    {
        // Set the property to the max health. This will call the HealthChanged event to update UI for example.
        // This happens on start so that everyone has a chance to register to the event onEnable or on awake first, otherwise there will be no listeners :)
        CurrentHealth = maxHealth;
    }
}
