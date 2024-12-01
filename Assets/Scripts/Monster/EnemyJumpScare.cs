using UnityEngine;
using System.Collections;

public class EnemyJumpScare : MonoBehaviour
{
    public Transform enemyFacePosition; // ���� �� ��ġ�� �ٶ󺸴� Transform
    public float zoomInDuration = 0.5f; // ���� �ð�
    public float zoomInFieldOfView = 30f; // ���� �� ī�޶� FOV
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
        // �ʱ� ī�޶� ��ġ ����
        Vector3 originalPosition = mainCamera.transform.position;
        Quaternion originalRotation = mainCamera.transform.rotation;

        // ī�޶��� ��ġ�� ȸ���� ���� �󱼷� ����
        mainCamera.transform.position = enemyFacePosition.position;
        mainCamera.transform.LookAt(enemyFacePosition);

        float elapsedTime = 0f;

        while (elapsedTime < zoomInDuration)
        {
            elapsedTime += Time.deltaTime;

            // ī�޶��� FOV�� ������ �ٿ���
            mainCamera.fieldOfView = Mathf.Lerp(originalFieldOfView, zoomInFieldOfView, elapsedTime / zoomInDuration);

            yield return null;
        }

        yield return new WaitForSeconds(1f); // 1�� ���� ����

        // ������ ī�޶� ��ġ�� FOV�� ����
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
