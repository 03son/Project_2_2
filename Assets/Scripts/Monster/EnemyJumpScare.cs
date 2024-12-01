using UnityEngine;
using System.Collections;

public class EnemyJumpScare : MonoBehaviour
{
    public Transform enemyFacePosition; // 적의 얼굴 위치를 바라보는 Transform
    public float zoomInDuration = 0.5f; // 줌인 시간
    public float zoomInFieldOfView = 30f; // 줌인 시 카메라 FOV
    private Camera mainCamera;
    private float originalFieldOfView;

    void Start()
    {
        mainCamera = Camera.main;
        originalFieldOfView = mainCamera.fieldOfView;
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(ZoomInOnEnemy());
    }

    IEnumerator ZoomInOnEnemy()
    {
        // 초기 카메라 위치 저장
        Vector3 originalPosition = mainCamera.transform.position;
        Quaternion originalRotation = mainCamera.transform.rotation;

        // 카메라의 위치와 회전을 적의 얼굴로 변경
        mainCamera.transform.position = enemyFacePosition.position;
        mainCamera.transform.LookAt(enemyFacePosition);

        float elapsedTime = 0f;

        while (elapsedTime < zoomInDuration)
        {
            elapsedTime += Time.deltaTime;

            // 카메라의 FOV를 서서히 줄여줌
            mainCamera.fieldOfView = Mathf.Lerp(originalFieldOfView, zoomInFieldOfView, elapsedTime / zoomInDuration);

            yield return null;
        }

        yield return new WaitForSeconds(1f); // 1초 동안 유지

        // 원래의 카메라 위치와 FOV로 복귀
        mainCamera.transform.position = originalPosition;
        mainCamera.transform.rotation = originalRotation;
        mainCamera.fieldOfView = originalFieldOfView;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyJumpScare jumpScareScript = GetComponent<EnemyJumpScare>();
            if (jumpScareScript != null)
            {
                jumpScareScript.TriggerJumpScare();
            }
        }
    }

}
