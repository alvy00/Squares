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

    //private bool doubleClicked = false;

    public bool ignoreCollisionPlayers = true;
    public bool ignoreCollisionEnemies = true;

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

            Move();
        }

    }

    void Move()
    {
        Vector3 moveToPos = UtilsClass.GetMouseWorldPosition();
        //List<Vector3> targetPosList = CircleFormation(moveToPos, 3f, selectedPlayersList.Count -1);
        List<Vector3> targetPosList = SquareFormation(moveToPos, 1.5f,selectedPlayersList.Count);
        //List<Vector3> targetPosList = LineFormationHorizontal(moveToPos, 1.5f, selectedPlayersList.Count);
        //List<Vector3> targetPosList = LineFormationvertical(moveToPos, 1.5f, selectedPlayersList.Count);
        //List<Vector3> targetPosList = NoFormation(moveToPos, 0.5f, selectedPlayersList.Count);


        // MOVES THE PLAYERS TO ASSIGNED VECTOR3 POSITIONS
        int targetPosListIndex = 0;
        foreach(PlayerRTS playerRTS in selectedPlayersList)
        {
            playerRTS.MoveTo(targetPosList[targetPosListIndex]);
            targetPosListIndex = (targetPosListIndex + 1) % targetPosList.Count;
        }

        // for(int i=0; i<targetPosList.Count; i++)
        // {
        //     Debug.Log(i + " " + targetPosList[i]);
        // }
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


    // ...................................PLAYER FORMATIONS...................................


    List<Vector3> CircleFormation(Vector3 startPos, float distance, int posCount)
    {
        List<Vector3> posList = new List<Vector3>();
        posList.Add(startPos);

        for(int i=0; i<posCount; i++)
        {
            float angle = i * (360f / posCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1,0), angle);
            Vector3 pos = startPos + dir * distance;
            posList.Add(pos);
        }

        return posList;
    }

    List<Vector3> SquareFormation(Vector3 startPos,float distance,int playerSelectedCount)
    {
        List<Vector3> posList = new List<Vector3>();


        int counter = 1;
        int xoffset = 0;

        float sqrt = Mathf.Sqrt(playerSelectedCount);
        float startx = startPos.x;

        for (int i = 0; i < playerSelectedCount; i++)
        {

            if (xoffset > Mathf.Floor(sqrt)-1)
            {
                xoffset = 0;
            }

            if (counter > Mathf.Floor(sqrt))
            {
                counter = 1;
                
                startPos.x = startx;
                startPos.y -= 1.5f;
            }

            posList.Add(new Vector3(startPos.x + (xoffset*distance), startPos.y, 0f));

            counter++;
            xoffset++;

        }

        return posList;
    }

    List<Vector3> LineFormationHorizontal(Vector3 startPos, float distance,int playerSelectedCount)
    {
        List<Vector3> posList = new List<Vector3>();
        //posList.Add(startPos);


        for(int i=0; i<playerSelectedCount; i++)
        {
            posList.Add(new Vector3(startPos.x + (i*distance), startPos.y, 0));
        }

        return posList;
    }

    List<Vector3> LineFormationvertical(Vector3 startPos, float distance,int playerSelectedCount)
    {
        List<Vector3> posList = new List<Vector3>();
        //posList.Add(startPos);


        for(int i=0; i<playerSelectedCount; i++)
        {
            posList.Add(new Vector3(startPos.x, startPos.y  + (i*distance), 0));
        }

        return posList;
    }

    List<Vector3> NoFormation(Vector3 startPos, float distance,int playerSelectedCount)
    {
        List<Vector3> posList = new List<Vector3>();
        //posList.Add(startPos);


        for(int i=0; i<playerSelectedCount; i++)
        {
            posList.Add(new Vector3(startPos.x + (i * Random.Range(-distance, distance)), startPos.y  + (i * Random.Range(-distance, distance)), 0));
        }

        return posList;
    }


    //...................................PLAYER FORMATIONS...................................

    Vector3 ApplyRotationToVector(Vector3 vec,float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}
