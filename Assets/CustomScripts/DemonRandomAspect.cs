using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonRandomAspect : MonoBehaviour
{
    private Transform demonTransform; 
    [SerializeField]private Material[] demonMaterials;
    private SkinnedMeshRenderer renderer; 
    
    void Start()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        demonTransform = GetComponent<Transform>();
        
        float randomValue = Random.Range(8.0f, 10.5f);
        demonTransform.localScale = new Vector3(randomValue, randomValue,randomValue); 
        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        renderer.material = SelectRandomMat();
    }

    private Material SelectRandomMat()
    {
        return demonMaterials[Random.Range(0, demonMaterials.Length)];
    }

}
