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
    public MonsterAI mstate;

    private CameraShake cameraShake;
    public bool isShaking = false;

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
            //cameraShake = player.GetComponentInChildren<CameraShake>();
            cameraShake = CameraInfo.MainCam.gameObject.GetComponent<CameraShake>();

            if (cameraShake == null)
            {
                Debug.LogError("CameraShake ��ũ��Ʈ�� Main Camera�� �߰��� �ּ���.");
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ��ü�� ã�� �� �����ϴ�. �÷��̾ �������� �����Ǿ����� Ȯ���� �ּ���.");
        }
        mstate = GetComponent<MonsterAI>();
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position); // ���� �÷��̾� ���� �Ÿ� ���
        //Debug.Log(mstate.currentState);
        // Idle ������ ��� ������ ���� �ߴ�
        if (mstate.currentState == MonsterAI.State.Idle)
        {
            StopShakingAndSound();
            return;
        }
        if (mstate.currentState != MonsterAI.State.Idle) //id��e ���°� �ƴ��� Ȯ��
        {
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
                CameraInfo.MainCam.gameObject.GetComponent<CameraShake>().SetShakeMagnitude(currentShakeMagnitude);
                // cameraShake.SetShakeMagnitude(currentShakeMagnitude);
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
    }

    IEnumerator ShakeAndPlaySoundRepeatedly()
    {

        isShaking = true;

        while (isShaking)
        {
            // ���� ����
            if (cameraShake != null)
            {
                StartCoroutine(CameraInfo.MainCam.gameObject.GetComponent<CameraShake>().Shake(shakeDuration));
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
    private void StopShakingAndSound()
    {
        // ���� �� ���� �ߴ�
        if (isShaking)
        {
            StopAllCoroutines();
            isShaking = false;
        }

        if (footstepSound != null && footstepSound.isPlaying)
        {
            footstepSound.Stop();
        }

        if (cameraShake != null)
        {
            cameraShake.SetShakeMagnitude(0); // ���� ���� �ʱ�ȭ
        }
    }
}
