using UnityEngine;
using System.Collections;

public class EnemyJumpScare : MonoBehaviour
{
    public Transform enemyFacePosition; // 적의 얼굴 위치를 바라보는 Transform
    public float zoomInDuration = 0.5f; // 줌인 시간
    private Camera mainCamera;
    private float originalFieldOfView;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isJumpScareActive = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다. 카메라를 확인해주세요.");
        }
        else
        {
            originalFieldOfView = mainCamera.fieldOfView;
            Debug.Log("Main Camera와 FOV 초기화 완료");
        }
    }

    void Update()
    {
        if (isJumpScareActive && mainCamera.transform.parent == enemyFacePosition)
        {
            // enemyFacePosition의 자식으로 있는 동안 카메라의 위치와 회전을 지속적으로 설정
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;
        }
    }

    public void TriggerJumpScare()
    {
        if (mainCamera != null)
        {
            Debug.Log("JumpScare 시작");
            StartCoroutine(ZoomInOnEnemy());
        }
        else
        {
            Debug.LogError("Main Camera가 초기화되지 않았습니다. JumpScare를 시작할 수 없습니다.");
        }
    }

    IEnumerator ZoomInOnEnemy()
    {
        // 현재 카메라의 부모를 원래 부모로부터 분리하고, enemyFacePosition의 자식으로 설정합니다.
        Transform originalParent = mainCamera.transform.parent;
        CameraRot cameraRotScript = mainCamera.GetComponent<CameraRot>();

        if (cameraRotScript != null)
        {
            cameraRotScript.isControlledExternally = true;  // CameraRot 업데이트 멈추기
        }

        mainCamera.transform.SetParent(enemyFacePosition);
        Debug.Log("카메라가 enemyFacePosition의 자식으로 설정되었습니다.");

        // 카메라의 위치 및 회전을 enemyFacePosition에 맞춰 강제로 설정
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
        Debug.Log("카메라의 위치와 회전을 enemyFacePosition에 맞춰 설정되었습니다.");

        // 줌인 시간 동안 유지 (줌인이 필요 없으므로 바로 대기)
        yield return new WaitForSeconds(zoomInDuration);

        yield return new WaitForSeconds(1f); // 1초 동안 유지

        // 원래 부모로 다시 연결하고 위치와 회전을 복귀시킵니다.
        mainCamera.transform.SetParent(originalParent);
        mainCamera.transform.position = originalPosition;
        mainCamera.transform.rotation = originalRotation;
        Debug.Log("카메라가 원래 부모로 다시 연결되었습니다.");

        if (cameraRotScript != null)
        {
            cameraRotScript.isControlledExternally = false;  // CameraRot 업데이트 재개
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌 감지. JumpScare 트리거 호출");
            TriggerJumpScare();
        }
    }
}
