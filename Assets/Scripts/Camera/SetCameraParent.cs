using System.Collections;
using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    private Transform cameraEmptyObject;

    // Start 메서드를 IEnumerator로 수정하여 코루틴으로 사용
    IEnumerator Start()
    {
        // 약간의 지연을 준 후 빈 오브젝트 할당 시도
        yield return new WaitForSeconds(0.1f);

        // 빈 오브젝트의 이름을 기준으로 할당 시도 (이름이 "Camera"인 오브젝트를 찾음)
        cameraEmptyObject = GameObject.Find("Camera")?.transform;

        if (cameraEmptyObject != null && Camera.main != null)
        {
            // 메인 카메라의 Transform을 가져와서 cameraEmptyObject의 자식으로 설정
            Camera.main.transform.SetParent(cameraEmptyObject, false); // Keep world position false

            // 카메라의 위치와 회전을 빈 오브젝트의 로컬 위치와 동일하게 설정
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;

            Debug.Log("Camera successfully attached to the empty object after delay.");
        }
        else
        {
            Debug.LogError("Camera or Camera Empty Object not found after delay.");
        }
    }
}
