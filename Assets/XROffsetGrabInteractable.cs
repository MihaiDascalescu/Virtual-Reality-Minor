using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrOffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPose;

    private Quaternion initialAttachLocalRot;
    // Start is called before the first frame update
    void Start()
    {
        if (!attachTransform)
        {
            GameObject grab = new GameObject("GrabPivot");
            grab.transform.SetParent(transform,false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPose = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;
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
            attachTransform.localPosition = initialAttachLocalPose;
            attachTransform.localRotation = initialAttachLocalRot;
        }
        
        base.OnSelectEnter(interactor);
    }
}
