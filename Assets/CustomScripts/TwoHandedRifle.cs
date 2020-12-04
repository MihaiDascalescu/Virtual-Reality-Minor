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
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.gameObject.GetComponent<Demon>().health.TakeDamage(damage);
            }
            else if (rayHit.collider.CompareTag("TargetPractice"))
            {
                rayHit.collider.gameObject.GetComponent<PracticeTargetDemon>().health.TakeDamage(damage);
            }
            else if (rayHit.collider.CompareTag("MovableTarget"))
            {
                rayHit.collider.gameObject.GetComponent<MovableTarget>().health.TakeDamage(damage);
            }
            else
            {
                return;
            }
        }

    }
    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        /*if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);*/
    }
}
