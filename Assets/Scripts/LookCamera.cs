using UnityEngine;

public class LookCamera : MonoBehaviour
{

    void LateUpdate()
    {
        // ī�޶� �ٶ󺸴� ������ ���մϴ�.
        Vector3 lookDir = Camera.main.transform.forward;
        // ī�޶� �ٶ󺸴� �������� ������Ʈ�� ȸ����ŵ�ϴ�.
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
