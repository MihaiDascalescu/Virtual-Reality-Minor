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
    //Gun stats
    [SerializeField]private int damage;
    [SerializeField]private float spread, range, reloadTime, timeBetweenShots;
    [SerializeField]private int magazineSize, bulletsPerTap;
    
    private int bulletsLeft, bulletsShot;

    //bools 
    public bool Shooting { get; set; }
    private bool readyToShoot, reloading;
    
    [SerializeField]private Animator animator;
    [SerializeField]private Transform barrelLocation;
    
    [SerializeField]private GameObject bulletPrefab; 
    [SerializeField]private GameObject casingPrefab;
    [SerializeField]private GameObject muzzleFlashPrefab;
    //Reference
    public Transform fpsCam;
    
    private RaycastHit rayHit;
    //Enemy Detection
    [SerializeField]private LayerMask whatIsEnemy;
    [SerializeField]private Transform gunTransform;
    //Effects
    [SerializeField]private Transform casingExitLocation;
    //Effects
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    //UI
    public Text showBullets;
    public GameObject panel;
    //Sound
    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    
    public bool OnReload { get; set; } = false;
    public bool ActivatePanel { get; set; }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        showBullets.text = bulletsLeft + "/" + magazineSize;
        if (ActivatePanel)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
        if (Vector3.Angle(gunTransform.up, Vector3.up) > 100 && bulletsLeft < magazineSize)
        {
            StartCoroutine(Reload());
            source.PlayOneShot(reloadSound);
        }

        if (!readyToShoot || !Shooting || reloading || bulletsLeft <= 0) return;
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
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
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
        bulletsLeft--;
        bulletsShot--;

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
        bulletsLeft = magazineSize;
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

