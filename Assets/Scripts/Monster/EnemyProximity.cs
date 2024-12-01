using UnityEngine;
using System.Collections;

public class EnemyProximity : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    public float detectionRange = 20f; // ���� �÷��̾ �����ϴ� �Ÿ� ����
    public float shakeDuration = 0.1f; // ���� ���� �ð�
    public float shakeMagnitude = 0.1f; // ���� ����
    public float shakeInterval = 0.1f; // ���� ���� (1�ʸ��� ����)
    public AudioSource footstepSound; // �߼Ҹ� ����� �ҽ�

    private CameraShake cameraShake;
    private bool isShaking = false;

    void Start()
    {
        // �÷��̾ "Player" �±׸� �̿��� ã���ϴ�. �����տ� Player �±׸� �̸� �����ؾ� �մϴ�.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            cameraShake = player.GetComponentInChildren<CameraShake>();

            if (cameraShake == null)
            {
                Debug.LogError("CameraShake ��ũ��Ʈ�� Main Camera�� �߰��� �ּ���.");
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ��ü�� ã�� �� �����ϴ�. �÷��̾ �������� �����Ǿ����� Ȯ���� �ּ���.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position); // ���� �÷��̾� ���� �Ÿ� ���

        if (distance <= detectionRange)
        {
            if (!isShaking)
            {
                StartCoroutine(ShakeAndPlaySoundRepeatedly());
            }
        }
        else
        {
            StopAllCoroutines();
            isShaking = false;
            if (footstepSound != null && footstepSound.isPlaying)
            {
                footstepSound.Stop(); // �������� ����� �߼Ҹ� ����
            }
        }
    }


    IEnumerator ShakeAndPlaySoundRepeatedly()
    {
        isShaking = true;

        while (isShaking)
        {
            // ���� ����
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            // �߼Ҹ� ���
            if (footstepSound != null)
            {
                footstepSound.Play();
            }

            // ���� ���� ���
            yield return new WaitForSeconds(shakeInterval);
        }
    }
}
