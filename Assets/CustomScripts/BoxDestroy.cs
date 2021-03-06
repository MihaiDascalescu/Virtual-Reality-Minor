﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BoxDestroy : MonoBehaviour
{
    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private int cubesInRow = 5;
    [SerializeField] private Material pieceMaterial;
    
    private float cubesPivotDistance;
    private Vector3 cubesPivot;
    
    [SerializeField] private float explosionForce = 50f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionUpward = 0.4f;

    [SerializeField] private string rightHand,leftHand;

    [FormerlySerializedAs("magazinePrefab")] [SerializeField] private GameObject pistolMagazinePrefab;
    [SerializeField] private GameObject shotgunMagazinePrefab;
    [SerializeField] private GameObject medKitPrefab;

    [FormerlySerializedAs("dropMagazine")] [SerializeField] private bool dropPistolMagazine;
    [SerializeField] private bool dropHealthKit;
    [SerializeField] private bool dropShotgunMagazine;
    
    private AudioSource source;

    [SerializeField] private UnityEvent playSound;

    private void Start()
    {
        cubesPivotDistance = cubeSize * cubesInRow * 0.5f;
        
        cubesPivot = new Vector3(cubesPivotDistance,cubesPivotDistance,cubesPivotDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != rightHand && other.gameObject.name != leftHand) return;
        if(dropPistolMagazine)
        {
            Instantiate(pistolMagazinePrefab, transform.position, Quaternion.identity);
        }

        if (dropHealthKit)
        {
            Instantiate(medKitPrefab, transform.position, Quaternion.identity);
        }
        
        if (dropShotgunMagazine)
        {
            Instantiate(shotgunMagazinePrefab, transform.position, Quaternion.identity);
        }
        playSound?.Invoke();
        Explode();
    }

    private void Explode()
    {
        
        gameObject.SetActive(false);
        
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    CreatePiece(x,y,z);
                }
            }
        }

        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,explosionRadius,explosionUpward);
            }
        }
    }

    private void CreatePiece(int x, int y, int z)
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize,cubeSize,cubeSize);
        
        piece.GetComponent<Renderer>().material = pieceMaterial;
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        Destroy(piece,3.0f);
    }
}
