using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_AI : MonoBehaviour
{
    public GameObject destroyEffect;
    public LayerMask whatIsSolid;

    public float lifetime;
    public float damage;

    void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0f, whatIsSolid);
        if(hitInfo.collider != null)
        {
            if(hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<Health_System_Player>().TakeDamage(damage);
            }

            DestroyBullet();
        } 
    }

    void DestroyBullet()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

