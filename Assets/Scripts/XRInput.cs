using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XrInput : MonoBehaviour
{
    [FormerlySerializedAs("_controller")] [SerializeField] private XRController controller;
    [FormerlySerializedAs("OnPrimaryPressed")] [SerializeField] private UnityEvent onPrimaryPressed;

    // Update is called once per frame
    void Update()
    {
        bool isPressed;
        controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);
        
        if (isPressed) onPrimaryPressed.Invoke();
        
        
    }
}