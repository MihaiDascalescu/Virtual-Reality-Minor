using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolAmmo : MonoBehaviour
{
    
    [SerializeField] private int ammoCount;

    private void OnCollisionEnter(Collision other)
    {
        GunSystem gunSystem = other.gameObject.GetComponentInChildren<GunSystem>();

        if (gunSystem == null)
        {
            return;
        }
        gunSystem.ammo += ammoCount;

        gameObject.GetComponent<XRGrabInteractable>().colliders.Clear();
        Destroy(gameObject);
    }

}
