using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Velocity_Transform : MonoBehaviour, IMoveVelocity
{
    private Vector3 velocityVector;


    public float moveSpeed;


    private void Update()
    {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }
}
