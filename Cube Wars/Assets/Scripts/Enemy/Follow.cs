using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;


    void Update()
    {
        if(target == null) { return; }
        
        if(Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        
        }else if(Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance){
            transform.position = this.transform.position;
        }else if(Vector2.Distance(transform.position, target.position) < retreatDistance){
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }
        
    }
}
