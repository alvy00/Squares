using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 touchStart;


    public float zoomOutMin = 2;
    public float zoomOutMax = 15;

    public float cameraLimitX = 2;
    public float cameraNegLimitX = 15;
    public float cameraLimitY = 2;
    public float cameraNegLimitY = 15;

    void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(1))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            CameraLimiting();
        }


        Zoom(Input.GetAxis("Mouse ScrollWheel"));

        
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    void CameraLimiting()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, cameraNegLimitX, cameraLimitX);
        pos.y = Mathf.Clamp(pos.y, cameraNegLimitY, cameraLimitY);

        transform.position = pos;
    }
}
