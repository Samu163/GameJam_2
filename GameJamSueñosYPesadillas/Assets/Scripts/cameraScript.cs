using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform targetObject; 
    public float zoomDuration = 2f;
    public float zoomAmount = 2f; 
    public float zoomOutDelay = 2f;

    private Vector3 originalPosition;
    private float originalScale;

    private Camera myCamera;

    private bool isZoomingIn = false;
    private bool isZoomingOut = false;
    private float zoomTimer = 0f;
    private float zoomOutTimer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        myCamera = GetComponent<Camera>();
        originalScale = myCamera.orthographicSize;
    }

    void Update()
    {

        if (isZoomingIn)
        {
            zoomTimer += Time.deltaTime;
            float t = zoomTimer / zoomDuration;
            transform.position = Vector3.Lerp(originalPosition, targetObject.position, t);
            myCamera.orthographicSize = Mathf.Lerp(originalScale, 250, t);

            if (zoomTimer >= zoomDuration)
            {
                isZoomingIn = false;
                zoomOutTimer = 0f; 
                isZoomingOut = true;
            }
        }

        if (isZoomingOut)
        {
            zoomOutTimer += Time.deltaTime;

            if (zoomOutTimer >= zoomOutDelay)
            {
                float t = (zoomOutTimer - zoomOutDelay) / zoomDuration;
                transform.position = Vector3.Lerp(targetObject.position, originalPosition, t);
                myCamera.orthographicSize = Mathf.Lerp(250, originalScale, t);

                if (zoomOutTimer >= zoomDuration + zoomOutDelay)
                {
                    isZoomingOut = false;
                }
            }
        }
    }

    public void ZoomIn(Transform traget)
    {
        targetObject = traget;
        isZoomingIn = true;
        zoomTimer = 0f;
    }

}
