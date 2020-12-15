using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action Died;

    private SoundPlayer soundPlayer; // TODO: Remove me

    [FormerlySerializedAs("soundToPlay")] [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip bulletImpactSound;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value == currentHealth)
            {
                return;
            }

            int previousHealth = currentHealth;
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            if (previousHealth > currentHealth)
            {
                Damaged?.Invoke(currentHealth);
            }
            else if (previousHealth < currentHealth)
            {
                Healed?.Invoke(currentHealth);
            }

            if (currentHealth == 0)
            {
                Died?.Invoke();
            }
        }
    }

    public int MaxHealth => maxHealth;
    
    [SerializeField] private int maxHealth;

    private int currentHealth;
    
    private void Start()
    {
        // Set the property to the max health. This will call the HealthChanged event to update UI for example.
        // This happens on start so that everyone has a chance to register to the event onEnable or on awake first, otherwise there will be no listeners :)
        CurrentHealth = maxHealth;
        soundPlayer = GetComponent<SoundPlayer>();
    }

    // TODO: Remove me
    /*
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Damaged?.Invoke(damage);
        soundPlayer.PlaySoundOneShot(hurtSound);
        //soundPlayer.PlaySoundOneShot(bulletImpactSound);
    }
    */

    // TODO: Remove me
    /*
    public void HealDamage(int healAmount)
    {
        currentHealth += healAmount;
        Healed?.Invoke(healAmount);
    }
    */

    private bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void SetHealthToFull()
    {
        CurrentHealth = maxHealth;
    }

    // TODO: Remove me and handle in whatever needs to be destroyed when the died event is called
    /*
    public void DestroyIfDead()
    {
        if (IsDead())
        {
            Died?.Invoke();
        }
    }
    */
}
