using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
   
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
   
    public float activationTreshold = 0.1f;

    public XRRayInteractor rightRay; 
    public XRRayInteractor leftRay;
    
    public bool EnableLeftTeleport { get; set; }= true;
    public bool EnableRightTeleport { get; set; }= true;
    
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;
        if (leftTeleportRay)
        {
            bool isLeftInteractorHovering = leftRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfTeleportActivated(leftTeleportRay) && !isLeftInteractorHovering);
        }

        if (rightTeleportRay)
        {
            bool isRightInteractorHovering = rightRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfTeleportActivated(rightTeleportRay) && !isRightInteractorHovering);
        }

    }

    public bool CheckIfTeleportActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice,teleportActivationButton,out bool isActivated,activationTreshold);
        return isActivated;
    }
}
