using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public XRNode inputJumpSource;
    public float speed = 1;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;
    
    [SerializeField] private bool checkForGroundOnJump = true;
    [SerializeField] private float jumpingForce;
    private float fallingSpeed;
    private Vector2 inputAxis;
    private XRRig rig;
    private bool buttonPressed = false;

    private Vector3 moveDirection = Vector3.zero; 

    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(inputJumpSource);
        UpdateJump(rightHand);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        characterController.Move(direction * (speed * Time.fixedDeltaTime));

        //gravity
        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }

        characterController.Move(Vector3.up * (fallingSpeed * Time.fixedDeltaTime));
    }

    void CapsuleFollowHeadset()
    {
        characterController.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        characterController.center = new Vector3(capsuleCenter.x,characterController.height/2 + characterController.skinWidth,capsuleCenter.z);
    }
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(characterController.center);
        float rayLength = characterController.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, characterController.radius, Vector3.down, out RaycastHit hitInfo,
            rayLength, groundLayer);
        return hasHit;
    }

    void UpdateJump(InputDevice inputSource)
    {
        if (checkForGroundOnJump && !CheckIfGrounded())
        {
            return;
        }

        bool buttonValue;
        if(inputSource.TryGetFeatureValue(CommonUsages.primaryButton, out buttonValue) && buttonValue)
        {
            moveDirection.y = jumpingForce;
            if (!buttonPressed)
            {
                buttonPressed = true;
                moveDirection.y -= gravity * Time.fixedDeltaTime;
                characterController.Move(moveDirection * Time.fixedDeltaTime);
                
            }
        }
        else if (buttonPressed)
        {
            buttonPressed = false;
        }
    }
}
