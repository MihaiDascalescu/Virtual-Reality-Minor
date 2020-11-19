using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public event Action<Collider> TargetDetected;
    public event Action<Collider> TargetLost;

    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float range;
    [SerializeField] private float fieldOfView;

    private float halfFieldOfView;

    private List<Collider> detectedTargets = new List<Collider>();
    
    private void Awake()
    {
        halfFieldOfView = fieldOfView * 0.5f;
    }

    private void Update()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, range, detectionLayer, QueryTriggerInteraction.Ignore);
        List<Collider> collidersInFieldOfView = GetTargetsInFieldOfView(targetsInRange);
        
        AddNewlyDetectedTargets(collidersInFieldOfView);
        RemoveUndetectedTargets(collidersInFieldOfView);
    }

    private List<Collider> GetTargetsInFieldOfView(Collider[] targets)
    {
        List<Collider> collidersInFieldOfView = new List<Collider>(targets.Length);
        
        Vector3 forwardDirection = transform.forward;
        foreach (Collider collider in targets)
        {
            Vector3 vectorToCollider = collider.transform.position - transform.position;
            float angleToCollider = Mathf.Abs(Vector3.Angle(forwardDirection, vectorToCollider));

            if (angleToCollider <= halfFieldOfView)
            {
                collidersInFieldOfView.Add(collider);
            }
        }

        return collidersInFieldOfView;
    }

    private void AddNewlyDetectedTargets(List<Collider> targetsInFieldOfView)
    {
        foreach (Collider collider in targetsInFieldOfView)
        {
            if (!detectedTargets.Contains(collider))
            {
                detectedTargets.Add(collider);
                TargetDetected?.Invoke(collider);
            }
        }
    }

    private void RemoveUndetectedTargets(List<Collider> targetsInFieldOfView)
    {
        for(int i = detectedTargets.Count - 1; i >= 0; --i)
        {
            Collider collider = detectedTargets[i];
            if (!targetsInFieldOfView.Contains(collider))
            {
                detectedTargets.RemoveAt(i);
                TargetLost?.Invoke(collider);
            }
            
        }
    }
}

