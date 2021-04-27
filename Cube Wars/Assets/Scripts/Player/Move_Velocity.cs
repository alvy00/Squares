using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Velocity : MonoBehaviour, IMoveVelocity
{
    private Rigidbody2D rb;
    private Vector3 velocityVector;


    public float moveSpeed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocityVector * moveSpeed;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    
}
