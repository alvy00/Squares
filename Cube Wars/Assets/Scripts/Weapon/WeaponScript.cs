using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform shotPoint;
    private SpriteRenderer spriteRenderer;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public float spread;
    public float bulletForce;

    public float offset = -90f;
    public float shotPointLocalPosX;
    public float shotPointLocalPosY;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {

        Vector3 difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, (rotZ + offset));


        // Flips the gun
        if(rotZ < 89 && rotZ > -89)
        {
            shotPoint.localPosition = new Vector3(shotPointLocalPosX, shotPointLocalPosY, 0);
            spriteRenderer.flipY = false;
        }else
        {
            shotPoint.localPosition = new Vector3(shotPointLocalPosX, -shotPointLocalPosY, 0);
            spriteRenderer.flipY = true;
        }


        // Time between bullets fired
        if(timeBtwShots <= 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Shoot();
                timeBtwShots = startTimeBtwShots;
            }
        }else
        {
            timeBtwShots -= Time.deltaTime;
        }


    }

    void Shoot()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) + new Vector3(x, y, 0);
        direction.Normalize();

        GameObject bulletIns = Instantiate(bullet, shotPoint.position, transform.rotation);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * bulletForce * 100);
    }
}
