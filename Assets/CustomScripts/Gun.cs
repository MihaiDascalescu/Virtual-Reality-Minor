using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 40;
    public GameObject bullet;
    public Transform barrel;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Fire()
    {
        GameObject spawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;
        
        Destroy(spawnedBullet,2);
    }

    
}
