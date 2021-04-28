using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_System_Player : MonoBehaviour
{
    public GameObject deathEffect;
    private GameObject[] enemies;
    public List<Detect_Shoot> detect_Shoot;

    public float health;
    
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies != null)
        {
            foreach(GameObject go in enemies)
            {
                detect_Shoot.Add(go.GetComponent<Detect_Shoot>());
            }
        }

        Debug.Log(enemies.Length);

    }

    void Update()
    {
        if(health <= 0)
        {
            foreach(Detect_Shoot ds in detect_Shoot)
            {
                ds.allPlayersPos.Remove(gameObject.transform);
            }


            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
