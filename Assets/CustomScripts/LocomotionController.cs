using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    // Start is called before the first frame update
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationTreshold = 0.1f;

    public XRRayInteractor rightRay; 
    public XRRayInteractor leftRay;
    
    public bool EnableLeftTeleport { get; set; }= true;
    public bool EnableRightTeleport { get; set; }= true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;
        if (leftTeleportRay)
        {
            bool isLeftInteractorHovering = leftRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftInteractorHovering);
        }

        if (rightTeleportRay)
        {
            bool isRightInteractorHovering = rightRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightInteractorHovering);
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice,teleportActivationButton,out bool isActivated,activationTreshold);
        return isActivated;
    }
}
