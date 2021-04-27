using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Position : MonoBehaviour, IMovePosition
{
    private Vector3 movePosition;


    private bool isMovingToPosition;

    private void Update()
    {
        if(isMovingToPosition){
            Vector3 moveDir = (movePosition - transform.position).normalized;

            float reachedDistance = 0.3f;


            if(Vector3.Distance(movePosition, transform.position) < reachedDistance) 
            {
                moveDir = Vector3.zero;
                isMovingToPosition = false;
            }
            GetComponent<IMoveVelocity>().SetVelocity(moveDir);
        }else
        {
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

    
    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;
        isMovingToPosition = true;
    }

}

