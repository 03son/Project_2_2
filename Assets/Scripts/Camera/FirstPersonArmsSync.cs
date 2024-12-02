using UnityEngine;

public class ArmsRotationSync : MonoBehaviour
{
    [SerializeField] private Transform mainCamera; // ���� ī�޶� Transform
    [SerializeField] private Transform armsTransform; // 1��Ī �� �� Transform

    [SerializeField] private Vector3 rotationOffset = Vector3.zero; // �� �� ȸ�� ����

    void LateUpdate()
    {
        if (mainCamera == null || armsTransform == null)
        {
            Debug.LogWarning("Main Camera or Arms Transform is not assigned.");
            return;
        }

        // ī�޶��� ȸ���� �� �𵨿� ����ȭ
        armsTransform.rotation = mainCamera.rotation * Quaternion.Euler(rotationOffset);
    }
}
