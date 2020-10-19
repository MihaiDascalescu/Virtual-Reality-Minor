using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DesertEagle : MonoBehaviour
{
    [SerializeField] private GameObject leftXrController;
    [SerializeField] private GameObject rightXrController;
    [SerializeField] private UnityEvent onPrimaryPressed;
    [SerializeField] private UnityEvent onPrimaryReleased;
    [SerializeField] private GameObject leftXrControllerParent;
    [SerializeField] private GameObject rightXrControllerParent;
    [SerializeField] private GameObject cartridge;
    [SerializeField] private Transform cartridgeExit;
    [SerializeField] private Transform barrelExit;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] public Transform cockBackPosition;

    public bool Fire { get; set; } = false;

    private Animator _animator;
    public bool CockBack { get; set; } = false;
    public bool Cocked { get; set; } = false;

    private bool _magazineInGun = false;
    [SerializeField] private int rounds = 0;
    [SerializeField] private Transform magazinePosition;
    public GameObject Magazine { get; set; }
    public bool ReleaseMagazine { get; set; } = false;

    private float _canShootAgainTime = 0.5f;
    private float _canShootAgain = 0;
    
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeftPrimaryPressed;
        leftXrControllerParent.GetComponent<XRController>().inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isLeftPrimaryPressed);
        if (isLeftPrimaryPressed)
        {
            var distanceToCockBackPosition = Vector3.Distance(cockBackPosition.position, leftXrController.transform.position);
            if (distanceToCockBackPosition < 0.1f && Cocked == false)
            {
                CockBack = true;
                leftXrController.transform.parent = cockBackPosition;
                Invoke("ResetLeftHandModel",0.5f);
            }
        }

        bool isRightPrimaryPressed;
        rightXrControllerParent.GetComponent<XRController>().inputDevice
            .TryGetFeatureValue(CommonUsages.primaryButton, out isRightPrimaryPressed);
        if (isRightPrimaryPressed)
        {
            onPrimaryPressed.Invoke();
        }
        else
        {
            onPrimaryReleased.Invoke();
        }
        
        if (ReleaseMagazine && _magazineInGun)
        {
            ReleaseMagazineFromGun();
        }

        if (CockBack && !Cocked)
        {
            CockBackSlider();
        }

        if (Fire)
        {
            Shoot();
        }
}


    void ResetLeftHandModel()
    {
        leftXrController.transform.parent = leftXrControllerParent.transform;
        leftXrController.transform.position = Vector3.zero;
        leftXrController.transform.rotation = Quaternion.Euler(leftXrController.transform.rotation.eulerAngles + new Vector3(0,0,90));
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
            else if(hitInfo.transform.CompareTag("otherTag"))
            {
                //Attack something else;
            }
        }

        var newImpact = Instantiate(bulletImpact, hitInfo.point, hitInfo.collider.transform.rotation);
        newImpact.transform.forward = hitInfo.normal;
        newImpact.transform.parent = hitInfo.transform;
    }
    public void Shoot()
    {
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
            var bullet = Magazine.transform.GetChild(0).gameObject;
            bullet.SetActive(false);
            CheckForHit();
            Invoke("KickBack",0.1f);
        }
        else if (Cocked && rounds <= 0 && _animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound") == false)
        {
            Cocked = false;
            _animator.SetTrigger("FireNoAmmo");
            
        }

        Fire = false;
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

    public void InstantiateMagazine()
    {
        Instantiate(this, transform.position, Quaternion.identity);
    }

    public void ReleaseMagazineFromGun()
    {
        ReleaseMagazine = false;
        if (Magazine != null)
        {
            Magazine.transform.parent = null;
            var rigidbody = Magazine.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(-Magazine.transform.up * 3, ForceMode.Impulse);

            if (rounds > 1)
            {
                rounds = 1;
            }

            _magazineInGun = false;
            Magazine = null;
        }
    }

    public void CockBackSlider()
    {
        CockBack = false;
        Cocked = true;
        _animator.SetTrigger("Cockback");
    }
    /*public void SendCartridge()
    {
        var newCartridge = Instantiate(cartridge, cartridgeExit.position, cartridgeExit.rotation);
        var newRigidBody = cartridge.GetComponent<Rigidbody>();
        newRigidBody.AddForce(newCartridge.transform.up * 5,ForceMode.Impulse);
        var random = UnityEngine.Random.Range(-90, 90);
        var randomTorque = new Vector3(random, random, random);
        newRigidBody.AddTorque(randomTorque,ForceMode.Impulse);
        Destroy(newCartridge,5f);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            if (!_magazineInGun)
            {
                Magazine = other.gameObject;
                var newRigidbody = Magazine.GetComponent<Rigidbody>();
                newRigidbody.isKinematic = true;
                Magazine.tag = "UsedMagazine";
                var collider = Magazine.GetComponent<Collider>();
                collider.enabled = false;
                Magazine.transform.parent = magazinePosition;
                Magazine.transform.position = magazinePosition.position;
                Magazine.transform.rotation = magazinePosition.rotation;
                _magazineInGun = true;
                rounds += 9;
/*                if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound"))
                {
                    _animator.SetTrigger("Loaded");
                }*/
            }
        }
    }
}
