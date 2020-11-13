using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private int healthpoints;
    private bool isDead = false;
    public UnityEvent isPlayerDead;
    
    // Start is called before the first frame update
    void Start()
    {
        print(healthpoints);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnHit(int damage)
    {
        healthpoints -= damage;
        print(healthpoints);
    }

    public bool IsPlayerDead()
    {
        if (healthpoints <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
