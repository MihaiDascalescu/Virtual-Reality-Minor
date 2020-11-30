using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class PressableButton : XRBaseInteractable
{
    private float yMin = 0.0f;
    [SerializeField]private float yMax = 0.0f;
    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor;
    private bool previousPress = false;
    public UnityEvent onPress = null;
    
    protected override void Awake()
    {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    private void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = GetLocalYPos(hoverInteractor.transform.position);
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax);
    }

    private void Start()
    {
        SetMinMax();
    }

    private void SetMinMax()
    {
        var position = transform.localPosition;
        Collider collider = GetComponent<Collider>();
        yMin = position.y - (collider.bounds.size.y * 0.5f);
        yMax = position.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (!hoverInteractor)
        {
            return;
        }

        float newHandHeight = GetLocalYPos(hoverInteractor.transform.position);
        float handDifference = previousHandHeight - newHandHeight;
        previousHandHeight = newHandHeight;

        float newPosition = transform.localPosition.y - handDifference;
        SetYPosition(newPosition);
        
        CheckPress();
    }

    private float GetLocalYPos(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    private void SetYPosition(float positionY)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(positionY, yMin, yMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool isInPosition = InPosition();
        if (isInPosition && isInPosition != previousPress)
        {
            onPress?.Invoke();
        }

        previousPress = isInPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
}
