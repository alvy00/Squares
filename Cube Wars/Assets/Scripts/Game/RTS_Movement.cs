using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RTS_Movement : MonoBehaviour
{
    private List<PlayerRTS> selectedPlayersList;


    private Vector3 startPosition;
    

    void Awake()
    {
        selectedPlayersList = new List<PlayerRTS>();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Touch on the player
            startPosition = UtilsClass.GetMouseWorldPosition();


            Collider2D[] collider2DArray =  Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());
            
            // Deselect
            foreach(PlayerRTS playerRTS in selectedPlayersList)
            {
                playerRTS.SetSelectedVisible(false);
            }

            // Select
            selectedPlayersList.Clear();
            foreach(Collider2D collider2D in collider2DArray)
            {
                PlayerRTS playerRTS = collider2D.GetComponent<PlayerRTS>();
                if(playerRTS != null)
                {
                    playerRTS.SetSelectedVisible(true);
                    selectedPlayersList.Add(playerRTS);
                }
            } 

            //Debug.Log(selectedPlayersList.Count);
        }

        if(Input.GetMouseButtonUp(0))
        {
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            // Touch lifted
            foreach(PlayerRTS playerRTS in selectedPlayersList)
            {
                playerRTS.SetSelectedVisible(false);
                playerRTS.MoveTo(moveToPosition);
            }

            Debug.Log(UtilsClass.GetMouseWorldPosition() + " "  + startPosition); 
        }
    }

}
