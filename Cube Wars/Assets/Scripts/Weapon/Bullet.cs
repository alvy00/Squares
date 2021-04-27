using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject destroyEffect;
    public LayerMask whatIsSolid;

    public float lifeTime;
    public float damage;
    
    void Start()
    {
        Invoke("DestroyBullet", lifeTime);
    }

    
    void Update()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0f, whatIsSolid);
        if(hitInfo.collider != null)
        {
            if(hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Health_System>().TakeDamage(damage);
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
