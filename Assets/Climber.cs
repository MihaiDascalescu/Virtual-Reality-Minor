using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    private CharacterController _characterController;
    public static XRController climbingHand;
    private ContinousMovement _continousMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _continousMovement = GetComponent<ContinousMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(climbingHand)
        {
            _continousMovement.enabled = false;
            Climb();
        }
        else
        {
            _continousMovement.enabled = true;
        }
        
    }

    void Climb()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        _characterController.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("FinalHandle"))
        {
            _characterController.SimpleMove(Vector3.up);
        }
        
    }
}
