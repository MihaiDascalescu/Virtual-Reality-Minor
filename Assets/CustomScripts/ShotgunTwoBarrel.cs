using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ShotgunTwoBarrel : MonoBehaviour
{
    [Header("Location Refrences")]
    [SerializeField] private Transform barrelLocationOne;
    [SerializeField] private Transform casingExitLocationOne;
    [SerializeField] private Transform casingExitLocationTwo;
    [SerializeField] private Transform shotGunTransform;

    [Header("Settings")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private float spread = 0.02f;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int magazineSize;
    [SerializeField] private float reloadTime;
    [SerializeField] public int ammo;
    
    [Header("Effects")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    
    [Header("Sounds")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip fireNoAmmo;
    
    private RaycastHit rayHit;
    private bool reloading;
    private float bulletsLeft;
    private bool hasAmmo = false;
    private GunCast gunCast;
    private bool barrelOne = true;
    private bool barrelTwo = true;
    void Start()
    {
        if (barrelLocationOne == null)
            barrelLocationOne = transform;

        gunAnimator = GetComponent<Animator>();
        gunCast = GetComponent<GunCast>();
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
    }

    public void PullTheTrigger()
    {
        if(bulletsLeft > 0 && !reloading && hasAmmo)
        {
            gunAnimator.SetTrigger("Fire");
            ammo--;
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
        if (ammo > 0)
        {
            hasAmmo = true;
        }
        else
        {
            hasAmmo = false;
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
        var barrelLocationTransform = barrelLocationOne.transform;
        Vector3 direction = barrelLocationTransform.forward + new Vector3(x, y, 0);

        //RayCast
        
        gunCast.CheckForHit(barrelLocationTransform.position,direction,rayHit,range,whatIsEnemy,damage);
        bulletsLeft--;
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
