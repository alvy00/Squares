using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Player_Move_Towards_Mouse : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            GetComponent<Move_Position>().SetMovePosition(UtilsClass.GetMouseWorldPosition());
        }
    }
}
