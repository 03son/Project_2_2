using UnityEngine;
using System.Collections;

public class EnemyProximity : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    public float detectionRange = 20f; // 적이 플레이어에 접근하는 거리 기준
    public float maxShakeMagnitude = 0.3f; // 최대 진동 강도
    public float shakeDuration = 0.1f; // 진동 지속 시간
    public float shakeInterval = 0.5f; // 진동 간격
    public AudioSource footstepSound; // 발소리 오디오 소스
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
        // 플레이어를 "Player" 태그를 이용해 찾습니다. 프리팹에 Player 태그를 미리 설정해야 합니다.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            //cameraShake = player.GetComponentInChildren<CameraShake>();
            cameraShake = CameraInfo.MainCam.gameObject.GetComponent<CameraShake>();

            if (cameraShake == null)
            {
                Debug.LogError("CameraShake 스크립트를 Main Camera에 추가해 주세요.");
            }
        }
        else
        {
            Debug.LogError("Player 태그를 가진 객체를 찾을 수 없습니다. 플레이어가 동적으로 생성되었는지 확인해 주세요.");
        }
        mstate = GetComponent<MonsterAI>();
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position); // 적과 플레이어 사이 거리 계산
        //Debug.Log(mstate.currentState);
        // Idle 상태일 경우 진동과 사운드 중단
        if (mstate.currentState == MonsterAI.State.Idle)
        {
            StopShakingAndSound();
            return;
        }
        if (mstate.currentState != MonsterAI.State.Idle) //idㅣe 상태가 아닌지 확인
        {
            if (distance <= detectionRange)
            {
                if (!isShaking)
                {
                   StartCoroutine(ShakeAndPlaySoundRepeatedly());
                }
                // 거리 비례하여 진동 강도 및 발소리 볼륨 계산
                float proximity = 1 - (distance / detectionRange); // 가까울수록 1에 가깝고, 멀수록 0에 가까움
                float currentShakeMagnitude = Mathf.Lerp(0, maxShakeMagnitude, proximity);
                float currentVolume = Mathf.Lerp(0, 1, proximity);

                if (footstepSound != null)
                {
                    footstepSound.volume = currentVolume;
                }

                // 진동 강도를 카메라 흔들기 함수에 전달
                CameraInfo.MainCam.gameObject.GetComponent<CameraShake>().SetShakeMagnitude(currentShakeMagnitude);
                // cameraShake.SetShakeMagnitude(currentShakeMagnitude);
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
    }

    IEnumerator ShakeAndPlaySoundRepeatedly()
    {

        isShaking = true;

        while (isShaking)
        {
            // 진동 시작
            if (cameraShake != null)
            {
                StartCoroutine(CameraInfo.MainCam.gameObject.GetComponent<CameraShake>().Shake(shakeDuration));
            }

            // 발소리 재생
            if (footstepSound != null && !footstepSound.isPlaying)
            {
                footstepSound.Play();
            }

            // 진동 간격 대기
            yield return new WaitForSeconds(shakeInterval);
        }
    }
    private void StopShakingAndSound()
    {
        // 진동 및 사운드 중단
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
            cameraShake.SetShakeMagnitude(0); // 진동 강도 초기화
        }
    }
}
