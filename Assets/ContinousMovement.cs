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
    public float speed = 1;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;
    
    private float _fallingSpeed;
    private Vector2 _inputAxis;
    private XRRig _rig;

    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion _headYaw = Quaternion.Euler(0, _rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 _direction = _headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y);

        _characterController.Move(_direction * speed * Time.fixedDeltaTime);

        //gravity
        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
        {
            _fallingSpeed = 0;
        }
        else
        {
            _fallingSpeed += gravity * Time.fixedDeltaTime;
        }

        _characterController.Move(Vector3.up * _fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        _characterController.height = _rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(_rig.cameraGameObject.transform.position);
        _characterController.center = new Vector3(capsuleCenter.x,_characterController.height/2 + _characterController.skinWidth,capsuleCenter.z);
    }
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(_characterController.center);
        float rayLength = _characterController.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, _characterController.radius, Vector3.down, out RaycastHit hitInfo,
            rayLength, groundLayer);
        return hasHit;
    }
}
