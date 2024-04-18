using UnityEngine;

public class LookCamera : MonoBehaviour
{

    void LateUpdate()
    {
        // 카메라가 바라보는 방향을 구합니다.
        Vector3 lookDir = Camera.main.transform.forward;
        // 카메라가 바라보는 방향으로 오브젝트를 회전시킵니다.
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
