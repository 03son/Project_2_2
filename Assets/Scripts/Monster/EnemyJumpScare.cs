using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnemyJumpScare : MonoBehaviourPun
{
    public Transform enemyFacePosition; // ���� �� ��ġ�� �ٶ󺸴� Transform
    public float zoomInDuration = 2f; // ���� �ð�
    public AudioClip jumpScareSound;    // �������ɾ� ����
    public float soundVolume = 1f;      // ���� ���� (0~1)
    //public PlayerState pstate;

    private Camera mainCamera;          // ���� �÷��̾��� ī�޶�
    private AudioSource audioSource;    // AudioSource ������Ʈ
    private Vector3 originalPosition;   // ���� ��ġ ����
    private Quaternion originalRotation; // ���� ȸ�� ����

    float shakeDuration; //카메라 떨림 지속시간_ EnemyProximity 여기 꺼 사용함
    void Start()
    {
        // AudioSource �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = soundVolume;
        audioSource.playOnAwake = false;

        // jumpScareSound Ȯ��
        if (jumpScareSound == null)
        {
            Debug.LogError("JumpScare ���尡 �������� �ʾҽ��ϴ�. Inspector���� �߰��ϼ���.");
        }

        shakeDuration = GetComponent<EnemyProximity>().shakeDuration;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonView playerPhotonView = other.GetComponent<PhotonView>();
                PlayerState playerState = other.GetComponent<PlayerState>(); // PlayerState 컴포넌트 가져오기

                // 플레이어 상태 확인
                if (playerState != null && playerState.State == PlayerState.playerState.Die)
                {
                    Debug.Log("플레이어는 이미 사망 상태입니다. 점프 스케어 무시.");
                    return;
                }

                // 점프 스케어 트리거
                if (playerPhotonView != null && playerPhotonView.IsMine)
                {
                    Debug.Log("점프 스케어 트리거 발동.");
                    TriggerJumpScare();
                }
            }
            else
            {
                PlayerState playerState = other.GetComponent<PlayerState>(); // PlayerState 컴포넌트 가져오기

                // 플레이어 상태 확인
                if (playerState != null && playerState.State == PlayerState.playerState.Die)
                {
                    Debug.Log("플레이어는 이미 사망 상태입니다. 점프 스케어 무시.");
                    return;
                }
                TriggerJumpScare();
            }
        }
    }


    void TriggerJumpScare()
    {
        mainCamera = CameraInfo.MainCam;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� ã�� �� �����ϴ�.");
            return;
        }

        // �������ɾ� ����� ī�޶� ȿ�� ����
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
            Debug.LogWarning("�������ɾ� ���尡 �������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator ZoomInOnEnemy()
    {
        Transform originalParent = mainCamera.transform.parent;

        // ���� ��ġ�� ȸ�� ����
        originalPosition = mainCamera.transform.localPosition;
        originalRotation = mainCamera.transform.localRotation;

        // ī�޶� �� �󱼷� �̵�
        mainCamera.transform.SetParent(enemyFacePosition);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;

        // ���� ����
        yield return new WaitForSeconds(zoomInDuration);

        // ���� �θ� �� ��ġ/ȸ������ ����
        mainCamera.transform.SetParent(originalParent);
        mainCamera.transform.localPosition = originalPosition;
        mainCamera.transform.localRotation = originalRotation;
    }
}
