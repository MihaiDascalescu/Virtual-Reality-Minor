using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private string rightHand;
    [SerializeField] private string leftHand;
    [SerializeField] private int healAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != rightHand && other.gameObject.name != leftHand) return;
        HealPlayer(other);
        Destroy(gameObject);
    }

    private void HealPlayer(Collider player)
    {
        var health = player.gameObject.GetComponent<Health>();
        health.HealDamage(healAmount);
    }
}
