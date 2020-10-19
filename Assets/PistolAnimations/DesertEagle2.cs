using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class DesertEagle2 : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private XRController rightHandController;
    [SerializeField] private GameObject Magazine;
    [SerializeField] private GameObject cartridge;
    [SerializeField] private Transform cartridgeExit;
    [SerializeField] private Transform magazinePosition;
    [SerializeField] private Transform barrelExit;
    [SerializeField] private GameObject bulletImpact;
    private bool ReleaseMagazine;
    
    private float _canShootAgainTime = 0.5f;
    private float _canShootAgain = 0;
    
    [SerializeField] private int rounds;
    private bool _magazineInGun;
    
    public bool Cocked { get; set; }
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSecondaryPressed;
        rightHandController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out isSecondaryPressed);
        if (isSecondaryPressed)
        {
            _animator.SetTrigger("Cockback");
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
        _animator.SetTrigger("Fire");
        if (Cocked && rounds > 1 && _canShootAgain < Time.time)
        {
            _canShootAgain = Time.time + _canShootAgainTime;
            rounds--;
            _animator.SetTrigger("Fire");
            CheckForHit();
            Invoke("KickBack",0.1f);
            
        }
        else if(Cocked && rounds == 1 && _canShootAgain < Time.time)
        {
            _canShootAgain = Time.time + _canShootAgainTime;
            rounds--;
            _animator.SetTrigger("FireLastRound");
            if (Magazine != null)
            {
                var bullet = Magazine.transform.GetChild(0).gameObject;
                bullet.SetActive(false);
            }

            CheckForHit();
            Invoke("KickBack",0.1f);
        }
        else if (Cocked && rounds <= 0 && _animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound") == false)
        {
            Cocked = false;
            _animator.SetTrigger("FireNoAmmo");
            
        }
    }

    
    public void ReleaseMagazineFromGun()
    {
        ReleaseMagazine = false;
        if (Magazine != null)
        {
            Magazine.transform.parent = null;
            var rigidbody = Magazine.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.AddForce(-Magazine.transform.up * 3, ForceMode.Impulse);

            if (rounds > 1)
            {
                rounds = 1;
            }

            _magazineInGun = false;
            Magazine = null;
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
            if (_magazineInGun == false)
            {
                Magazine = other.gameObject;
                var newRigidbody = Magazine.GetComponent<Rigidbody>();
                newRigidbody.isKinematic = true;
                Magazine.tag = "UsedMagazine";
                var collider = Magazine.GetComponent<Collider>();
                collider.enabled = false;
                Magazine.transform.parent = magazinePosition.parent;
                Magazine.transform.position = magazinePosition.position;
                Magazine.transform.rotation = magazinePosition.rotation;
                _magazineInGun = true;
                rounds += 9;
               
                if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound"))
                {
                    _animator.SetTrigger("Loaded");
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
                var targetScript = hitInfo.transform.gameObject.GetComponent<EnemySimpleAI>();
                targetScript.TakeDamage(2);
            }
            /*else if(hitInfo.transform.CompareTag("otherTag"))
            {
                //Attack something else;
            }*/
        }

        
            /*var newImpact = Instantiate(bulletImpact, hitInfo.point, hitInfo.collider.transform.rotation);
            newImpact.transform.forward = hitInfo.normal;
            newImpact.transform.parent = hitInfo.transform;*/
        
    }
}
