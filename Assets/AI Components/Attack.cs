using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Health Target { get; set; }

    [SerializeField] private int damage = 5;

    private Health target;
    private Coroutine dealDamageCoroutineHandle;

    private void OnEnable() => StartAttacking();

    private void OnDisable() => StopAttacking();

    private void StartAttacking()
    {
        if (Target == null)
        {
            return;
        }
        transform.LookAt(Target.transform);
        StopAttacking();
        dealDamageCoroutineHandle = StartCoroutine(DealDamageCoroutine());
    }

    private void StopAttacking()
    {
        if (dealDamageCoroutineHandle == null)
        {
            return;
        }
        
        StopCoroutine(dealDamageCoroutineHandle);
        dealDamageCoroutineHandle = null;
    }
    
    private IEnumerator DealDamageCoroutine()
    {
        while (true)
        {
            Target.CurrentHealth -= damage;

            print($"Dealt {damage} damage to {Target.name}");
            yield return new WaitForSeconds(2.0f);
        }
    }
}
