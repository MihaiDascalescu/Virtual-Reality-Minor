using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInput : MonoBehaviour
{
    [SerializeField] private XRController _controller;
    [SerializeField] private UnityEvent OnPrimaryPressed;

    // Update is called once per frame
    void Update()
    {
        bool isPressed;
        _controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);
        
        if (isPressed) OnPrimaryPressed.Invoke();
        
        
    }
}