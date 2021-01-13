using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    private CharacterController characterController;
    public static XRController climbingHand;
    private ContinousMovement continousMovement;
    [SerializeField] private float ledgePush = 100.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        continousMovement = GetComponent<ContinousMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(climbingHand)
        {
            continousMovement.enabled = false;
            Climb();
        }
        else
        {
            continousMovement.enabled = true;
        }
        
    }

    void Climb()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        characterController.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }

  
}
