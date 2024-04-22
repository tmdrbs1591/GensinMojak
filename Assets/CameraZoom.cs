using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f; // �� �ӵ�
    public float minFOV = 10f; // �ּ� �þ�
    public float maxFOV = 60f; // �ִ� �þ�

    void Update()
    {
        // ���콺 �� ��ũ�� �Է��� �޾Ƽ� �þ߸� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // �þ߸� �����Ͽ� �� �� �� �� �ƿ��� ����
        Camera.main.fieldOfView += scroll * zoomSpeed * -1;

        // �þ߸� �ּҰ��� �ִ밪 ���̷� ����
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
    }
}
