using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ShotgunTwoBarrel : MonoBehaviour
{
   [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocationOne;
    [SerializeField] private Transform barrelLocationTwo;
    [SerializeField] private Transform casingExitLocationOne;
    [SerializeField] private Transform casingExitLocationTwo;
    

    [SerializeField] private float spread = 0.02f;
    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    [SerializeField] private Transform shotGunTransform;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip fireNoAmmo;
    

    private bool barrelOne = true;
    private bool barrelTwo = true;
    
    private RaycastHit rayHit;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int magazineSize;
    
    private bool reloading;
    [SerializeField]private float reloadTime;
    private float bulletsLeft;
    
    void Start()
    {
        if (barrelLocationOne == null)
            barrelLocationOne = transform;

        gunAnimator = GetComponent<Animator>();
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
    }

    public void PullTheTrigger()
    {
        if(bulletsLeft > 0 && !reloading)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(fireNoAmmo);
        }
    }

    private void Update()
    {
        if (Vector3.Angle(shotGunTransform.up, Vector3.up) > 100 && bulletsLeft < magazineSize)
        {
            gunAnimator.SetBool("Reload 0",true);
            StartCoroutine(Reload());
            
        }
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
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocationOne.position, barrelLocationOne.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }
        
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = barrelLocationOne.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(barrelLocationOne.transform.position, direction, out rayHit, range, whatIsEnemy))
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
        bulletsLeft--;
    }

   /* void ShootSecondBarrel()
    {
        if(barrelTwo && !barrelOne)
        {
            ChooseBarrel(barrelLocationTwo);
            bulletsLeft--;
            casingPrefabTwo.SetActive(false);
            barrelTwo = false;
        }
    }*/
    

    private void ChooseBarrel(Transform barrelToUse)
    {
        source.PlayOneShot(fireSound);
        
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelToUse.position, barrelToUse.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject instantiatedBullet = Instantiate(bulletPrefab, barrelToUse.position, barrelToUse.rotation);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(barrelToUse.forward * shotPower);
        Destroy(instantiatedBullet, 1.0f);
        
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 direction = barrelToUse.transform.forward + new Vector3(x, y, 0);
        
        if (Physics.Raycast(barrelToUse.transform.position, direction, out rayHit, range, whatIsEnemy))
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
    private IEnumerator Reload()
    {
        
        reloading = true;
        yield return  new WaitForSeconds(reloadTime);
        ReloadFinished();
    }
    private void ReloadFinished()
    {
        source.PlayOneShot(reloadSound);
        barrelOne = true;
        barrelTwo = true;
        bulletsLeft = magazineSize;
        reloading = false;
        gunAnimator.SetBool("Reload 0",false);
       
    }
    
    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        InstantiateCasingAtTransform(casingExitLocationOne);
        InstantiateCasingAtTransform(casingExitLocationTwo);
    }

    void InstantiateCasingAtTransform(Transform casingReleaseLocation)
    {
        if (!casingReleaseLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingReleaseLocation.position, casingReleaseLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingReleaseLocation.position - casingReleaseLocation.right * 0.3f - casingReleaseLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}
