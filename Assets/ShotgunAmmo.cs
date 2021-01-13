using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunAmmo : MonoBehaviour
{
    [SerializeField] private int ammoCount;

    private void OnCollisionEnter(Collision other)
    {
        ShotgunTwoBarrel shotgun = other.gameObject.GetComponentInChildren<ShotgunTwoBarrel>();

        if (shotgun == null)
        {
            return;
        }
        shotgun.ammo += ammoCount;
        
        gameObject.GetComponent<XRGrabInteractable>().colliders.Clear();
        Destroy(gameObject);
    }
}
