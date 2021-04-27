using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveToPosAstarPath : MonoBehaviour, IMovePosition
{
    private AIPath aiPath;


    private void Awake() 
    {
        aiPath = GetComponent<AIPath>();
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        aiPath.destination = movePosition;
    }
}
