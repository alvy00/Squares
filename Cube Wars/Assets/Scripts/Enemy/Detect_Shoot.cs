using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Detect_Shoot : MonoBehaviour
{
    public GameObject gun;
    public Transform shotPoint;
    public GameObject bullet;
    public SpriteRenderer gunSpriteRenderer;
    
    public Transform target;
    public List<Transform> allPlayersPos;
    public List<Transform> visibleTargets;
    public List<Transform> targetsFound;
    private Vector2 direction;

    public LayerMask interactableColliders;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private AIPath aiPath;

    public float range;

    public int weapon;
    private float timeBetweenEachBullet = 0.1f;
    public float bulletsToShoot;
    public float spread;
    public float bulletForce;

    private float pistol = 1f;
    private float smg = 1.8f;
    private float mg = 4.5f;

    public float shotPointLocalPosX;
    public float shotPointLocalPosY;


    private bool detected = false;

    private bool canShoot = true;


    void Start()
    {
        aiPath = GetComponent<AIPath>();

        GameObject[] allPlayerGameObject = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in allPlayerGameObject)
        {
            allPlayersPos.Add(go.transform);
        }

        //Debug.Log(allPlayersPos.Count);
    }


    void Update()
    {
        // FINDS THE CLOSEST PLAYER TO MOVE TO
        if(allPlayersPos != null)
        {
            //MoveTowardsClosestPlayer(GetClosestEnemy(allPlayersPos));
            MoveTowardsClosestPlayer(FindClosestEnemy());
        }


        // FINDS CLOSEST TARGET
        targetsFound = FindVisibleTargets();
        if(targetsFound == null) { return; }
        target = GetClosestEnemy(targetsFound);
        //target = FindClosestEnemy();
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
            if(rayInfo.collider.gameObject.CompareTag("Player"))
            {
                detected = true;
                
            }else{

                detected = false;
            }
        } //------------------------------------------------------------------

        if(detected)
        {
            // DISABLE MOVING TOWARDS PLAYERS
            aiPath.enabled = false;
            
            // SHOOT    
            if(canShoot)
            {
                switch(weapon)
                {
                    case 1:
                        Pistol();
                        canShoot = false;
                        Invoke("Reload", Random.Range(pistol, pistol+0.3f));
                        break;
                    case 2:
                        SMG();
                        canShoot = false;
                        Invoke("Reload", Random.Range(smg, smg+0.3f));
                        break;
                    case 3:
                        MG();
                        canShoot = false;
                        Invoke("Reload", Random.Range(mg, mg+0.3f));
                        break;
                    default:
                        break;
                }
            }

        }

        aiPath.enabled = true;
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

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range, playerMask);

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

    Transform GetClosestEnemy(List<Transform> players)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in players)
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

    Transform FindClosestEnemy() 
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) 
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) 
            {
                closest = go;
                distance = curDistance;
            }
        }
        
        return closest.transform;
    }

    void MoveTowardsClosestPlayer(Transform transform)
    {
        aiPath.target = transform;
    }

    // SHOWS THE CIRCLE AROUND WHICH ENEMY USES TO DETECT
    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
