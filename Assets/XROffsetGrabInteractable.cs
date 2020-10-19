using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 InitialAttachLocalPose;

    private Quaternion InitialAttachLocalRot;
    // Start is called before the first frame update
    void Start()
    {
        if (!attachTransform)
        {
            GameObject grab = new GameObject("GrabPivot");
            grab.transform.SetParent(transform,false);
            attachTransform = grab.transform;
        }

        InitialAttachLocalPose = attachTransform.localPosition;
        InitialAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        if (interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = InitialAttachLocalPose;
            attachTransform.localRotation = InitialAttachLocalRot;
        }
        
        base.OnSelectEnter(interactor);
    }
}
