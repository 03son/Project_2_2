using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnemyJumpScare : MonoBehaviourPun
{
    public Transform enemyFacePosition; // 적의 얼굴 위치를 바라보는 Transform
    public float zoomInDuration = 0.5f; // 줌인 시간
    public AudioClip jumpScareSound;    // 점프스케어 사운드
    public float soundVolume = 1f;      // 사운드 볼륨 (0~1)

    private Camera mainCamera;          // 로컬 플레이어의 카메라
    private AudioSource audioSource;    // AudioSource 컴포넌트
    private Vector3 originalPosition;   // 원래 위치 저장
    private Quaternion originalRotation; // 원래 회전 저장

    void Start()
    {
        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = soundVolume;
        audioSource.playOnAwake = false;

        // jumpScareSound 확인
        if (jumpScareSound == null)
        {
            Debug.LogError("JumpScare 사운드가 설정되지 않았습니다. Inspector에서 추가하세요.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // 로컬 플레이어에게만 점프스케어 트리거
                TriggerJumpScare();
            }
        }
    }

    void TriggerJumpScare()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다.");
            return;
        }

        // 점프스케어 사운드와 카메라 효과 실행
        PlayJumpScareSound();
        StartCoroutine(ZoomInOnEnemy());
    }

    private void PlayJumpScareSound()
    {
        if (jumpScareSound != null)
        {
            audioSource.PlayOneShot(jumpScareSound);
        }
        else
        {
            Debug.LogWarning("점프스케어 사운드가 설정되지 않았습니다.");
        }
    }

    IEnumerator ZoomInOnEnemy()
    {
        Transform originalParent = mainCamera.transform.parent;

        // 원래 위치와 회전 저장
        originalPosition = mainCamera.transform.localPosition;
        originalRotation = mainCamera.transform.localRotation;

        // 카메라를 적 얼굴로 이동
        mainCamera.transform.SetParent(enemyFacePosition);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;

        // 줌인 유지
        yield return new WaitForSeconds(zoomInDuration);

        // 원래 부모 및 위치/회전으로 복구
        mainCamera.transform.SetParent(originalParent);
        mainCamera.transform.localPosition = originalPosition;
        mainCamera.transform.localRotation = originalRotation;
    }
}
