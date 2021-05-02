using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RTS_Movement : MonoBehaviour
{
    private List<PlayerRTS> selectedPlayersList;
    public Transform selectionAreaTransform;

    private Vector3 startPosition;
    

    float _threshold = 0.25f;

    public bool ignoreCollisionPlayers = true;
    public bool ignoreCollisionEnemies = true;


    //private bool doubleClicked = false;

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(11, 11, ignoreCollisionPlayers);
        Physics2D.IgnoreLayerCollision(9, 9, ignoreCollisionEnemies);

        selectedPlayersList = new List<PlayerRTS>();

        //StartCoroutine ("DoubleClick");
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Touch on the player
            startPosition = UtilsClass.GetMouseWorldPosition();

            selectionAreaTransform.gameObject.SetActive(true);
            selectionAreaTransform.position = startPosition;
        }

        if(Input.GetMouseButton(0))
        {
            // SELECTION AREA
            Vector3 selectionAreaSize = UtilsClass.GetMouseWorldPosition() - startPosition;
            selectionAreaTransform.localScale = selectionAreaSize;
        }

        if(Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);

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



       if(Input.GetMouseButton(0))
        {
            Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
            List<Vector3> targetPosList = GetPosListAround(moveToPos, new float[]{3f}, new int[]{selectedPlayersList.Count -1});

            int targetPosListIndex = 0;

            foreach(PlayerRTS playerRTS in selectedPlayersList)
            {
                playerRTS.MoveTo(targetPosList[targetPosListIndex]);
                targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
            }

            // if(doubleClicked)
            // {
            //     Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
            //     List<Vector3> targetPosList = GetPosListAround(moveToPos, new float[]{3f}, new int[]{selectedPlayersList.Count -1});

            //     int targetPosListIndex = 0;

            //     foreach(PlayerRTS playerRTS in selectedPlayersList)
            //     {
            //         playerRTS.MoveTo(targetPosList[targetPosListIndex]);
            //         targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
            //     }
            // }
        }

    }

    void Move()
    {
        // if(DoubleClick())
        //     {
        //         Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
        //         List<Vector3> targetPosList = GetPosListAround(moveToPos, new float[]{3f}, new int[]{selectedPlayersList.Count -1});

        //         int targetPosListIndex = 0;

        //         foreach(PlayerRTS playerRTS in selectedPlayersList)
        //         {
        //             playerRTS.MoveTo(targetPosList[targetPosListIndex]);
        //             targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
        //         }
        //     }

        Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
        List<Vector3> targetPosList = GetPosListAround(moveToPos, new float[]{3f}, new int[]{selectedPlayersList.Count -1});

        int targetPosListIndex = 0;

        foreach(PlayerRTS playerRTS in selectedPlayersList)
        {
            playerRTS.MoveTo(targetPosList[targetPosListIndex]);
            targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
        }
    }

    // IEnumerator DoubleClick()
    // {
    //     while (true) 
    //     {
    //         float duration = 0;
    //         if (Input.GetMouseButtonDown (0)) 
    //         {
    //             while (duration < _threshold) 
    //             {
    //                 duration += Time.deltaTime;
    //                 yield return new WaitForSeconds (0.005f);
    //                 if (Input.GetMouseButtonDown (0)) 
    //                 {
    //                     doubleClicked = true;
    //                     duration = _threshold;
    //                     // Double click/tap
    //                     Debug.Log("Double Click detected");
    //                 }
    //             }

    //             if (!doubleClicked) 
    //             {
    //                 // Single click/tap
    //                 Debug.Log("Single Click detected");
    //             }
    //         }
    //         yield return null;
    //     }
    // }

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
