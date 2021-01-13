using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorAttachTag :MonoBehaviour
{
   [SerializeField] private Transform attachTransform;
   [SerializeField] private Transform[] transforms;

   private void Start()
   {
     
   }

   public void CheckForObject()
   {
      if (gameObject.CompareTag("Shotgun"))
      {
         print("facut");
         attachTransform.position = transforms[0].position;
      }

      if (gameObject.CompareTag("Pistol"))
      {
         attachTransform.position = transforms[1].position;
      }
   }

   public void Update()
   {
      
   }
}
