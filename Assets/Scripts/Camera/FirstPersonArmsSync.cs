using UnityEngine;

public class ArmsRotationSync : MonoBehaviour
{
    [SerializeField] private Transform mainCamera; // 메인 카메라 Transform
    [SerializeField] private Transform armsTransform; // 1인칭 팔 모델 Transform

    [SerializeField] private Vector3 rotationOffset = Vector3.zero; // 팔 모델 회전 보정

    void LateUpdate()
    {
        if (mainCamera == null || armsTransform == null)
        {
            Debug.LogWarning("Main Camera or Arms Transform is not assigned.");
            return;
        }

        // 카메라의 회전을 팔 모델에 동기화
        armsTransform.rotation = mainCamera.rotation * Quaternion.Euler(rotationOffset);
    }
}
