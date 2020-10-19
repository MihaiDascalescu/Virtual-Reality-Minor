using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _healthpoints;
    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        print(_healthpoints);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnHit(int damage)
    {
       
        _healthpoints -= damage;
        print(_healthpoints);
    }

    public bool IsPlayerDead()
    {
        if (_healthpoints <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
