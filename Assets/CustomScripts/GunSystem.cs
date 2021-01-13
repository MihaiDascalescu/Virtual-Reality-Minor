using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineScripts;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class GunSystem : MonoBehaviour
{
    [Header("Location References")]
    [SerializeField]private Transform barrelLocation;
    [SerializeField]private Transform casingExitLocation;
    
    [Header("Settings")]
    [SerializeField]private int damage;
    [SerializeField]private float spread, range, reloadTime, timeBetweenShots;
    [SerializeField]private int magazineSize, bulletsPerTap;
    [SerializeField]public int ammo;
    [SerializeField]private Animator animator;
    [SerializeField]private LayerMask whatIsEnemy;
    public bool Shooting { get; set; }
    
    [Header("Effects")]
    [SerializeField]private GameObject bulletPrefab; 
    [SerializeField]private GameObject casingPrefab;
    [SerializeField]private GameObject muzzleFlashPrefab;
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    
    [Header("User Interface")]
    public Text showBullets;
    public GameObject panel;
    public bool ActivatePanel { get; set; }
    [Header("Sound")]
    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
     
    private int bulletsLeft, bulletsShot;
    private bool hasAmmo;
    private bool readyToShoot, reloading;
    private GunCast gunCast;
    private RaycastHit rayHit;
    public bool OnReload { get; set; } = false;
    
    private void Start()
    {
        gunCast = GetComponent<GunCast>();
    }

    private void Awake()
    {
        bulletsLeft = ammo;
        readyToShoot = true;
    }
    private void Update()
    {
        showBullets.text = bulletsLeft + "/" + ammo;
        if (ActivatePanel)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
        if (Vector3.Angle(barrelLocation.up, Vector3.up) > 100 && bulletsLeft < magazineSize && hasAmmo)
        {
            StartCoroutine(Reload());
            source.PlayOneShot(reloadSound);
        }

        if (ammo > 0)
        {
            hasAmmo = true;
        }
        else
        {
            hasAmmo = false;
        }
        
        if (!readyToShoot || !Shooting || reloading || bulletsLeft <= 0 ||!hasAmmo) return;
        bulletsShot = bulletsPerTap;
        PullTheTrigger();

    }
    private void PullTheTrigger()
    {
        animator.SetTrigger("Fire");
    }

    private void Shoot()
    {
        readyToShoot = false;
        
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
        
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        var barrelLocationTransform = barrelLocation.transform;
        Vector3 direction = barrelLocationTransform.forward + new Vector3(x, y, 0);

        //RayCast
        gunCast.CheckForHit(barrelLocationTransform.position,direction,rayHit,range,whatIsEnemy,damage);
        bulletsLeft--;
        bulletsShot--;
        ammo--;

        StartCoroutine(ResetShot());
    }
    
    private IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        readyToShoot = true;
    }

    private IEnumerator ShootAgain()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        Shoot();
    }
    

    private IEnumerator Reload()
    {
        reloading = true;
        yield return  new WaitForSeconds(reloadTime);
        ReloadFinished();
    }
    private void ReloadFinished()
    {
        if (ammo >= magazineSize)
        {
            bulletsLeft = magazineSize;
        }
        else
        {
            bulletsLeft = ammo;
        }

        reloading = false;
    }
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}

