using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    [SerializeField] private Transform cameraEmptyObject; // 카메라를 자식으로 만들 빈 오브젝트

    void Start()
    {
        if (Camera.main != null)
        {
            // 메인 카메라의 Transform을 가져와서 cameraEmptyObject의 자식으로 설정
            Camera.main.transform.SetParent(cameraEmptyObject, false); // Keep world position false

            // 카메라의 위치와 회전을 빈 오브젝트의 로컬 위치와 동일하게 설정
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;

            Debug.Log("Camera successfully attached to the empty object.");
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }
}
