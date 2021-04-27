using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_System_Player : MonoBehaviour
{
    public GameObject deathEffect;
    private GameObject enemy;
    public Detect_Shoot detect_Shoot;

    public float health;
    
    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");

        if(enemy != null)
        {
            detect_Shoot = enemy.GetComponent<Detect_Shoot>();
        }

    }

    void Update()
    {
        if(health <= 0)
        {
            detect_Shoot.allPlayersPos.Remove(gameObject.transform);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
