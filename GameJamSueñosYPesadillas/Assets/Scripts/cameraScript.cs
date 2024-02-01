using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform targetObject; // The object to zoom in on
    public float zoomDuration = 0.2f; // Time taken to zoom in
    public float zoomAmount = 0.2f; // How much to zoom in
    public float minDistance = 0.2f; // Minimum distance between camera and target object

    private Vector3 originalPosition;
    private float originalScale;
    private float originalZPosition;

    private Camera myCamera;

    private bool isZoomingIn = false;
    private bool isZoomingOut = false;
    private float zoomTimer = 0f;
    private float zoomOutTimer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        originalZPosition = originalPosition.z;
        myCamera = GetComponent<Camera>();
        originalScale = myCamera.orthographicSize;
    }

    void Update()
    {
       

        if (isZoomingIn)
        {
            zoomTimer += Time.deltaTime;
            float t = zoomTimer / zoomDuration;
            Vector3 targetPosition = targetObject.position - (transform.position - targetObject.position).normalized * minDistance;
            transform.position = new Vector3(Mathf.Lerp(originalPosition.x, targetPosition.x, t), Mathf.Lerp(originalPosition.y, targetPosition.y, t), originalZPosition);
            myCamera.orthographicSize = Mathf.Lerp(originalScale, 250, t);

            if (zoomTimer >= zoomDuration)
            {
                isZoomingIn = false;
                zoomOutTimer = 0f; // Reset zoom out timer
                isZoomingOut = true;
            }
        }

        if (isZoomingOut)
        {
            zoomOutTimer += Time.deltaTime;

            if (zoomOutTimer >= zoomDuration)
            {
                float t = zoomOutTimer / zoomDuration;
                Vector3 newPosition = new Vector3(Mathf.Lerp(targetObject.position.x - ((transform.position - targetObject.position).normalized * minDistance).x, originalPosition.x, t), Mathf.Lerp(targetObject.position.y - ((transform.position - targetObject.position).normalized * minDistance).y, originalPosition.y, t), originalZPosition);
                transform.position = newPosition;
                myCamera.orthographicSize = Mathf.Lerp(250, originalScale, t);

                if (zoomOutTimer >= zoomDuration * 2f) // Wait for zoomDuration * 2f before ending zoom out phase
                {
                    isZoomingOut = false;
                }
            }
        }
    }

    public void ZoomIn(Transform traget)
    {
        targetObject = traget;
        traget.position = new Vector3(targetObject.position.x , targetObject.position.y, targetObject.position.z -60);
        isZoomingIn = true;
        zoomTimer = 0f;
    }

}
