using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class DesertEagle2 : MonoBehaviour
{
    /*private Animator animator;
    [SerializeField] private XRController rightHandController;
    [FormerlySerializedAs("Magazine")] [SerializeField] private GameObject magazine;
    [SerializeField] private GameObject cartridge;
    [SerializeField] private Transform cartridgeExit;
    [SerializeField] private Transform magazinePosition;
    [SerializeField] private Transform barrelExit;
    [SerializeField] private GameObject bulletImpact;
    private bool releaseMagazine;
    
    private float canShootAgainTime = 0.5f;
    private float canShootAgain = 0;
    
    [SerializeField] private int rounds;
    private bool magazineInGun;
    
    public bool Cocked { get; set; }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSecondaryPressed;
        rightHandController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out isSecondaryPressed);
        if (isSecondaryPressed)
        {
            //Add XRGRabInteractable to the Cockback
            
            animator.SetTrigger("Cockback");
            Cocked = true;
        }

        bool isPrimaryPressed;
        rightHandController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPrimaryPressed);
        if (isPrimaryPressed)
        {
            ReleaseMagazineFromGun();
        }

        
    }
    
    public void Shoot()
    {
        animator.SetTrigger("Fire");
        if (Cocked && rounds > 1 && canShootAgain < Time.time)
        {
            canShootAgain = Time.time + canShootAgainTime;
            rounds--;
            animator.SetTrigger("Fire");
            CheckForHit();
            Invoke("KickBack",0.1f);
            
        }
        else if(Cocked && rounds == 1 && canShootAgain < Time.time)
        {
            canShootAgain = Time.time + canShootAgainTime;
            rounds--;
            animator.SetTrigger("FireLastRound");
            if (magazine != null)
            {
                var bullet = magazine.transform.GetChild(0).gameObject;
                bullet.SetActive(false);
            }

            CheckForHit();
            Invoke("KickBack",0.1f);
        }
        else if (Cocked && rounds <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound") == false)
        {
            Cocked = false;
            animator.SetTrigger("FireNoAmmo");
            
        }
    }

    
    public void ReleaseMagazineFromGun()
    {
        releaseMagazine = false;
        if (magazine != null)
        {
            magazine.transform.parent = null;
            var rigidbody = magazine.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.AddForce(-magazine.transform.up * 3, ForceMode.Impulse);

            if (rounds > 1)
            {
                rounds = 1;
            }

            magazineInGun = false;
            magazine = null;
        }
    }
    public void SendCartridge()
    {
        var newCartridge = Instantiate(cartridge, cartridgeExit.position, cartridgeExit.rotation);
        var newRigidBody = cartridge.GetComponent<Rigidbody>();
        newRigidBody.AddForce(newCartridge.transform.up * 5,ForceMode.Impulse);
        var random = UnityEngine.Random.Range(-90, 90);
        var randomTorque = new Vector3(random, random, random);
        newRigidBody.AddTorque(randomTorque,ForceMode.Impulse);
        Destroy(newCartridge,5f);
    }
    private void KickBack()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 10);
        Invoke("ReturnKickBack",0.1f);
    }

    private void ReturnKickBack()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y, transform.eulerAngles.z + 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            if (magazineInGun == false)
            {
                magazine = other.gameObject;
                var newRigidbody = magazine.GetComponent<Rigidbody>();
                newRigidbody.isKinematic = true;
                magazine.tag = "UsedMagazine";
                var collider = magazine.GetComponent<Collider>();
                collider.enabled = false;
                magazine.transform.parent = magazinePosition.parent;
                magazine.transform.position = magazinePosition.position;
                magazine.transform.rotation = magazinePosition.rotation;
                magazineInGun = true;
                rounds += 9;
               
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound"))
                {
                    animator.SetTrigger("Loaded");
                }
            }
        }
    }
    void CheckForHit()
    {
        var forward = barrelExit.transform.TransformDirection(Vector3.forward);
        RaycastHit hitInfo;
        bool hittingSomething = Physics.Raycast(barrelExit.position, forward, out hitInfo, 1000);
        if (hittingSomething)
        {
            if (hitInfo.transform.CompareTag("Enemy"))
            {
                var targetScript = hitInfo.transform.gameObject.GetComponent<EnemySimpleAi>();
                targetScript.TakeDamage(2);
            }
            /*else if(hitInfo.transform.CompareTag("otherTag"))
            {
                //Attack something else;
            }
        }

        
            /*var newImpact = Instantiate(bulletImpact, hitInfo.point, hitInfo.collider.transform.rotation);
            newImpact.transform.forward = hitInfo.normal;
            newImpact.transform.parent = hitInfo.transform;
        
    }*/
}
