using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Vector3 moveDirection;

    private float moveSpeed;
    [SerializeField] private int damage; 

    void Start()
    {
        moveSpeed = 5.0f;
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(DisableBullet());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
        {
            return;
        }

        Health health = other.GetComponent<Health>();
        if (health == null)
        {
            return;
        }

        if (other.gameObject.layer == 16)
        {
            Destroy(gameObject);
        }
        
        health.CurrentHealth -= damage;
    }
}
