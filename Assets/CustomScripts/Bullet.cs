using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int destroyTimer = 5;
   
    // Update is called once per frame
    void Update()
    {
        destroyTimer--;
        if (destroyTimer == 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
