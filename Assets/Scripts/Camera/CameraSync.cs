using UnityEngine;

public class CameraSync : MonoBehaviour
{
    public Transform modelTransform;  // 캐릭터 모델의 Transform
    public Transform cameraTransform; // 메인 카메라의 Transform

    void Update()
    {
        if (cameraTransform != null && modelTransform != null)
        {
            // 카메라의 Y축 회전을 모델과 동기화
            Vector3 modelRotation = modelTransform.eulerAngles;
            modelRotation.y = cameraTransform.eulerAngles.y;
            modelTransform.eulerAngles = modelRotation;
        }
    }
}
