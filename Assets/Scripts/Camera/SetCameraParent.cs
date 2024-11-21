using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    [SerializeField] private Transform cameraEmptyObject; // 카메라를 자식으로 만들 빈 오브젝트

    void Start()
    {
        if (Camera.main != null)
        {
            // 메인 카메라의 Transform을 가져와서 cameraEmptyObject의 자식으로 설정
            Camera.main.transform.SetParent(cameraEmptyObject);

            // 원하는 위치와 회전을 설정 (기본값을 유지하고 싶다면 생략 가능)
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }
}
