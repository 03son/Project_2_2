using UnityEngine;
using System.Collections;

public class EnemyProximity : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    public float detectionRange = 20f; // 적이 플레이어에 접근하는 거리 기준
    public float shakeDuration = 0.1f; // 진동 지속 시간
    public float shakeMagnitude = 0.1f; // 진동 강도
    public float shakeInterval = 0.1f; // 진동 간격 (1초마다 진동)
    public AudioSource footstepSound; // 발소리 오디오 소스

    private CameraShake cameraShake;
    private bool isShaking = false;

    void Start()
    {
        // 플레이어를 "Player" 태그를 이용해 찾습니다. 프리팹에 Player 태그를 미리 설정해야 합니다.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            cameraShake = player.GetComponentInChildren<CameraShake>();

            if (cameraShake == null)
            {
                Debug.LogError("CameraShake 스크립트를 Main Camera에 추가해 주세요.");
            }
        }
        else
        {
            Debug.LogError("Player 태그를 가진 객체를 찾을 수 없습니다. 플레이어가 동적으로 생성되었는지 확인해 주세요.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position); // 적과 플레이어 사이 거리 계산

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
                footstepSound.Stop(); // 범위에서 벗어나면 발소리 멈춤
            }
        }
    }


    IEnumerator ShakeAndPlaySoundRepeatedly()
    {
        isShaking = true;

        while (isShaking)
        {
            // 진동 시작
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            // 발소리 재생
            if (footstepSound != null)
            {
                footstepSound.Play();
            }

            // 진동 간격 대기
            yield return new WaitForSeconds(shakeInterval);
        }
    }
}
