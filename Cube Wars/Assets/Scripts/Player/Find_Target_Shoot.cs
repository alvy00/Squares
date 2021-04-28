using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Target_Shoot : MonoBehaviour
{
    public GameObject gun;
    public Transform shotPoint;
    public GameObject bullet;
    public SpriteRenderer gunSpriteRenderer;
    
    public Transform target;
    public List<Transform> visibleTargets;
    public List<Transform> targetsFound;
    private Vector2 direction;

    public LayerMask interactableColliders;
    public LayerMask EnemyMask;
    public LayerMask obstacleMask;


    public float range;

    public int weapon;
    public float timeBetweenEachBurst = 1.8f;
    private float timeBetweenEachBullet = 0.1f;
    public float bulletsToShoot;
    public float spread;
    public float bulletForce;

    public float shotPointLocalPosX;
    public float shotPointLocalPosY;


    private bool detected = false;

    private bool canShoot = true;

    void Update()
    {
        // FINDS CLOSEST TARGET
        targetsFound = FindVisibleTargets();
        if(targetsFound == null) { return; }
        target = GetClosestEnemy(targetsFound);
        if(target == null) { return; }

        // DETERMINES THE SPREAD
        float disToEnemy = Vector2.Distance(transform.position, target.position);
        spread = disToEnemy/10;
        
        // FINDS THE TARGET AND SHOOTS
        Vector2 targetPos = target.position;
        direction = targetPos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, range, interactableColliders);
        if(rayInfo)
        {
            if(rayInfo.collider.gameObject.CompareTag("Enemy"))
            {
                detected = true;
                
            }else
            {
                detected = false;
            }
        } //------------------------------------------------------------------

        if(detected)
        {
            // SHOOT    
            if(canShoot)
            {
                switch(weapon)
                {
                    case 1:
                        Pistol();
                        canShoot = false;
                        Invoke("Reload", 1f); 
                        break;
                    case 2:
                        SMG();
                        canShoot = false;
                        Invoke("Reload", 1.8f);
                        break;
                    case 3:
                        MG();
                        canShoot = false;
                        Invoke("Reload", 4.5f);
                        break;
                    default:
                        break;
                }
            }     
        }
    }

    void AimShoot()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // GUN FLIP AND ROTATION
            gun.transform.right = direction;
            if(gun.transform.rotation.z < 0.7 && gun.transform.rotation.z > -0.7)
            {
                shotPoint.localPosition = new Vector3(shotPointLocalPosX, shotPointLocalPosY, 0);
                gunSpriteRenderer.flipY = false;
            }else
            {
                shotPoint.localPosition = new Vector3(shotPointLocalPosX, -shotPointLocalPosY, 0);
                gunSpriteRenderer.flipY = true;
            }

        GameObject bulletIns = Instantiate(bullet, shotPoint.position, Quaternion.identity);
        Vector2 norDir = direction + new Vector2(x, y);
        norDir.Normalize();
        bulletIns.GetComponent<Rigidbody2D>().AddForce(norDir * bulletForce * 100);
    }

    //........................... GUNS ...........................
    void Pistol()
    {
        Invoke("AimShoot", 0.2f);
    }
    void SMG()
    {
        Invoke("AimShoot", 0.2f);
        Invoke("AimShoot", 0.35f);
        Invoke("AimShoot", 0.55f);
        Invoke("AimShoot", 0.75f);
    }
    void MG()
    {
        Invoke("AimShoot", 0.2f);
        Invoke("AimShoot", 0.35f);
        Invoke("AimShoot", 0.55f);
        Invoke("AimShoot", 0.75f);
        Invoke("AimShoot", 0.95f);
        Invoke("AimShoot", 1.1f);
        Invoke("AimShoot", 1.35f);
        Invoke("AimShoot", 1.55f);
        Invoke("AimShoot", 1.75f);
    }
    //........................... GUNS ...........................

    void Reload()
    {
        canShoot = true;
    }

    List<Transform> FindVisibleTargets()
    {
        visibleTargets = new List<Transform>();

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range, EnemyMask);

        for(int i=0; i<targetsInRange.Length; i++)
        {
            Transform target = targetsInRange[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            float disToTarget = Vector2.Distance(transform.position, target.position);

            if(!Physics2D.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
            {
                visibleTargets.Add(target);
            }
        }

        return visibleTargets;
    }

    Transform GetClosestEnemy (List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }

    // SHOWS THE CIRCLE AROUND WHICH ENEMY USES TO DETECT
    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
