using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f; // 줌 속도
    public float minFOV = 10f; // 최소 시야
    public float maxFOV = 60f; // 최대 시야

    void Update()
    {
        // 마우스 휠 스크롤 입력을 받아서 시야를 변경
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 시야를 변경하여 줌 인 및 줌 아웃을 구현
        Camera.main.fieldOfView += scroll * zoomSpeed * -1;

        // 시야를 최소값과 최대값 사이로 제한
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
    }
}
