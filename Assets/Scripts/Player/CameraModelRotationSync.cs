using UnityEngine;

public class CameraModelRotationSync : MonoBehaviour
{
    [SerializeField] private Transform mainCameraTransform; // 메인 카메라의 Transform
    [SerializeField] private Transform modelTransform; // 최상위 모델링 오브젝트의 Transform
    [SerializeField] private float rotationSpeed = 10f; // 모델링이 카메라 회전을 따라가는 속도

    void Start()
    {
        if (mainCameraTransform == null)
        {
            // 메인 카메라가 할당되지 않았다면 기본적으로 메인 카메라를 참조
            mainCameraTransform = Camera.main.transform;
        }

        if (modelTransform == null)
        {
            modelTransform = transform; // 최상위 모델링 오브젝트의 Transform 사용
        }
    }

    void Update()
    {
        if (mainCameraTransform != null && modelTransform != null)
        {
            // 카메라의 Y축 회전 값을 모델에 즉각적으로 적용
            Vector3 targetRotation = new Vector3(0, mainCameraTransform.eulerAngles.y, 0);
            modelTransform.eulerAngles = targetRotation;
        }
    }



}
