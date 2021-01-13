using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPool : MonoBehaviour
{
    [SerializeField] private int damagePerSecond;
    [SerializeField] private string[] hittableTags = {"Enemy", "TargetPractice", "MovableTarget","Player"};
    
    void OnTriggerEnter(Collider col) {
        if (col.GetComponent<DPSAbility>() == null) {
            var dps = col.gameObject.AddComponent<DPSAbility>();
            dps.Damage = 10;
            dps.ApplyEveryNSeconds = 1;
        }
    }
}
