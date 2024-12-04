using UnityEngine;
using System.Collections;

public class EnemyProximity : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    public float detectionRange = 20f; // ���� �÷��̾ �����ϴ� �Ÿ� ����
    public float maxShakeMagnitude = 0.3f; // �ִ� ���� ����
    public float shakeDuration = 0.1f; // ���� ���� �ð�
    public float shakeInterval = 0.5f; // ���� ����
    public AudioSource footstepSound; // �߼Ҹ� ����� �ҽ�

    private CameraShake cameraShake;
    private bool isShaking = false;

    void Start()
    {
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        yield return new WaitForSecondsRealtime(5);
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

            // �Ÿ� ����Ͽ� ���� ���� �� �߼Ҹ� ���� ���
            float proximity = 1 - (distance / detectionRange); // �������� 1�� ������, �ּ��� 0�� �����
            float currentShakeMagnitude = Mathf.Lerp(0, maxShakeMagnitude, proximity);
            float currentVolume = Mathf.Lerp(0, 1, proximity);

            if (footstepSound != null)
            {
                footstepSound.volume = currentVolume;
            }

            // ���� ������ ī�޶� ���� �Լ��� ����
            cameraShake.SetShakeMagnitude(currentShakeMagnitude);
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
            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(shakeDuration));
            }

            // �߼Ҹ� ���
            if (footstepSound != null && !footstepSound.isPlaying)
            {
                footstepSound.Play();
            }

            // ���� ���� ���
            yield return new WaitForSeconds(shakeInterval);
        }
    }
}
