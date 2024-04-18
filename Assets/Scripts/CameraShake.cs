using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public Camera maincamera;
    Vector3 cameraPos;

    public bool isShake = false;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    [Range(0.1f, 0.5f)]
    float ShakeRange = 0.5f;
    [SerializeField]
    [Range(0.1f, 1f)]
    float duration = 0.1f;

    public void Shake()
    {
      
        cameraPos = maincamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
       
    }

    void StartShake()
    {
        float CameraPosX = Random.value * ShakeRange * 2 - ShakeRange;
        float CameraPosY = Random.value * ShakeRange * 2 - ShakeRange;

        Vector3 newCameraPos = cameraPos;
        newCameraPos.x += CameraPosX;
        newCameraPos.y += CameraPosY;

        maincamera.transform.position = newCameraPos;

        isShake = true;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        maincamera.transform.position = cameraPos;

        isShake = false;

    }
}
