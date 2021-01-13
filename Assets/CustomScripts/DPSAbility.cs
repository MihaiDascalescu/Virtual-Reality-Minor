using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSAbility : MonoBehaviour
{
    public int Damage { get; set; }
    public float Seconds { get; set; }
    public float Delay { get; set; }
    public float ApplyDamageNTimes { get; set; }
    public float ApplyEveryNSeconds { get; set; }

    private int appliedTimes = 0;

    void Start() {
        StartCoroutine(Dps());
    }

    IEnumerator Dps() 
    {
        yield return new WaitForSeconds(Delay);
        while(appliedTimes < ApplyDamageNTimes)
        {
            Health health = GetComponent<Health>();
            if (health != null) health.CurrentHealth -= Damage;
            yield return new WaitForSeconds(ApplyEveryNSeconds);
            appliedTimes++;
        }

        Destroy(this);
    }
}
