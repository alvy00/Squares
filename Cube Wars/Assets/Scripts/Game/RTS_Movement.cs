using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RTS_Movement : MonoBehaviour
{
    private List<PlayerRTS> selectedPlayersList;
    public Transform selectionAreaTransform;

    private Vector3 startPosition;
    

    private float doubleClickTime = 0.4f;
    private float lastClickTime;

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(11, 11, true);

        selectedPlayersList = new List<PlayerRTS>();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Touch on the player
            startPosition = UtilsClass.GetMouseWorldPosition();

            selectionAreaTransform.gameObject.SetActive(true);

            // Collider2D[] collider2DArray =  Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());
            // // Deselect
            // foreach(PlayerRTS playerRTS in selectedPlayersList)
            // {
            //     playerRTS.SetSelectedVisible(false);
            // }

            // // Select
            // selectedPlayersList.Clear();
            // foreach(Collider2D collider2D in collider2DArray)
            // {
            //     PlayerRTS playerRTS = collider2D.GetComponent<PlayerRTS>();
            //     if(playerRTS != null)
            //     {
            //         playerRTS.SetSelectedVisible(true);
            //         selectedPlayersList.Add(playerRTS);
            //     }
            // } 

            //Debug.Log(selectedPlayersList.Count);
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMousePos.x), Mathf.Min(startPosition.y, currentMousePos.y));
            Vector3 upperRight = new Vector3(Mathf.Max(startPosition.x, currentMousePos.x), Mathf.Max(startPosition.y, currentMousePos.y));
        
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;

            if(DoubleClick())
            {
                Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
                List<Vector3> targetPosList = GetPosListAround(moveToPos, new float[]{3f}, new int[]{selectedPlayersList.Count -1});

                int targetPosListIndex = 0;

                foreach(PlayerRTS playerRTS in selectedPlayersList)
                {
                    playerRTS.MoveTo(targetPosList[targetPosListIndex]);
                    targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
                }
            }
            
            // Vector3 currentMousePos = UtilsClass.GetMouseWorldPosition();
            // Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMousePos.x), Mathf.Min(startPosition.y, currentMousePos.y));
            // Vector3 upperRight = new Vector3(Mathf.Max(startPosition.x, currentMousePos.x), Mathf.Max(startPosition.y, currentMousePos.y));
       
            // selectionAreaTransform.position = lowerLeft;
            // selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if(Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);

            // Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            // // Touch lifted
            // foreach(PlayerRTS playerRTS in selectedPlayersList)
            // {
            //     playerRTS.SetSelectedVisible(false);
            //     playerRTS.MoveTo(moveToPosition);
            // }

            Collider2D[] collider2DArray =  Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());
            
            // DESLECT ALL PLAYERS
            foreach(PlayerRTS playerRTS in selectedPlayersList)
            {
                playerRTS.SetSelectedVisible(false);
            }

            // SELECT ALL PLAYERS WITHIN SELECTION AREA
            selectedPlayersList.Clear();
            foreach(Collider2D col2D in collider2DArray)
            {
                PlayerRTS playerRTS = col2D.GetComponent<PlayerRTS>();
                if(playerRTS != null)
                {
                    playerRTS.SetSelectedVisible(true);
                    selectedPlayersList.Add(playerRTS);
                }
            }

            //Debug.Log(selectedPlayersList.Count);

            //Debug.Log(UtilsClass.GetMouseWorldPosition() + " "  + startPosition); 
        }

    }

    bool DoubleClick()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if(timeSinceLastClick <= doubleClickTime){ return true; }

        lastClickTime = Time.time;

        return false;
    }

    List<Vector3> GetPosListAround(Vector3 startPos, float[] ringDistanceArray, int[] ringPosCountArray)
    {
        List<Vector3> posList = new List<Vector3>();
        posList.Add(startPos);
        for(int i=0; i<ringDistanceArray.Length; i++)
        {
            posList.AddRange(GetPosListAround(startPos, ringDistanceArray[i], ringPosCountArray[i]));
        }

        return posList;
    }

    List<Vector3> GetPosListAround(Vector3 startPos, float distance, int posCount)
    {
        List<Vector3> posList = new List<Vector3>();
        for(int i=0; i<posCount; i++)
        {
            float angle = i * (360f / posCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1,0), angle);
            Vector3 pos = startPos + dir * distance;
            posList.Add(pos);
        }

        return posList;
    }

    Vector3 ApplyRotationToVector(Vector3 vec,float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}
