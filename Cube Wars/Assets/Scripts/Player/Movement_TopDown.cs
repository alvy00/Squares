using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_TopDown : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;

    public float runSpeed = 20.0f;

    void Start ()
    {
        body = GetComponent<Rigidbody2D>(); 
    }

    void Update ()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical"); 
    }

    void FixedUpdate()
    {  
        body.velocity = new Vector2(horizontal * runSpeed * Time.deltaTime * 100, vertical * runSpeed * Time.deltaTime * 100);
    }
}
