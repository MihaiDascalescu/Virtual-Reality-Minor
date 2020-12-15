using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandedRifle : MonoBehaviour
{
   [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [SerializeField] private float spread = 0.02f;
    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
   


    public AudioSource source;
    public AudioClip fireSound;
    
    private RaycastHit rayHit;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private string[] hittableTags = {"Enemy", "TargetPractice", "MovableTarget"};
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        gunAnimator = GetComponent<Animator>();
    }


    public void PullTheTrigger()
    {
        gunAnimator.SetTrigger("Fire");

    }


//Called from animation event
    //This function creates the bullet behavior
    void Shoot()
    {
        source.PlayOneShot(fireSound);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject instantiatedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        Destroy(instantiatedBullet, 1.0f);
        
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 direction = barrelLocation.transform.forward + new Vector3(x, y, 0);
        
        if (Physics.Raycast(barrelLocation.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            foreach (string hittableTag in hittableTags)
            {
                if (!rayHit.collider.CompareTag(hittableTag))
                {
                    continue;
                }

                Health health = rayHit.collider.GetComponent<Health>();

                if (health == null)
                {
                    continue;
                }

                health.CurrentHealth -= damage;
            }
        }

    }
}
