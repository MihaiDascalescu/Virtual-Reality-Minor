using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBarrel : MonoBehaviour
{
    [SerializeField] private GameObject magazineToInstantiate;
    public void SpawnAmmo()
    {
        var pos = new Vector3(0,transform.position.y,0) + transform.position;
        Instantiate(magazineToInstantiate, pos, Quaternion.identity);
    }
}
