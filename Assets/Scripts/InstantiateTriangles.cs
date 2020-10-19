using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTriangles : MonoBehaviour
{

     public GameObject block;
    public int width = 1;
    public int length = 1;
    public float xSpacing = 3;
    public float zSpacing = 3;
    Vector3 currentPosition = new Vector3(-7,0,0);
    Vector3 futurePosition = new Vector3(3,0,0);
  
    public void Spawn()
    {
        for (int z=-10; z<length; z++)
        {
            for (int x=-10; x<width; x++)
            {
                Instantiate(block, new Vector3(x * xSpacing,0,z * zSpacing), Quaternion.identity);
            }
        }       
    }

    public void SpawnAtPosition()
    {

        if (currentPosition.x < 30)
        {
            Instantiate(block, currentPosition, Quaternion.identity);
            currentPosition += futurePosition;
        }
        else
        {
            futurePosition = new Vector3(-3,0,-5);
            currentPosition += futurePosition;
            futurePosition = new Vector3(-3,0,0);
            Instantiate(block, currentPosition, Quaternion.identity);
            currentPosition += futurePosition;
        }


    }
}


