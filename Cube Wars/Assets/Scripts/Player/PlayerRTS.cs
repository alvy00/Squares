using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRTS : MonoBehaviour
{
   public GameObject selectedGameObject;
   private IMovePosition movePosition;


   private void Awake()
   {
       movePosition = GetComponent<IMovePosition>();
   }

   public void SetSelectedVisible(bool visible)
   {
       if(selectedGameObject == null) {return;}
       selectedGameObject.SetActive(visible);
   }

   public void MoveTo(Vector3 targetPosition)
   {
       movePosition.SetMovePosition(targetPosition);
   }
}
