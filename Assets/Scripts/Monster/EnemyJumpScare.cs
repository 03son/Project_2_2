using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnemyJumpScare : MonoBehaviourPun
{
    public Transform enemyFacePosition; // ���� �� ��ġ�� �ٶ󺸴� Transform
    public float zoomInDuration = 0.5f; // ���� �ð�
    public AudioClip jumpScareSound;    // �������ɾ� ����
    public float soundVolume = 1f;      // ���� ���� (0~1)

    private Camera mainCamera;          // ���� �÷��̾��� ī�޶�
    private AudioSource audioSource;    // AudioSource ������Ʈ
    private Vector3 originalPosition;   // ���� ��ġ ����
    private Quaternion originalRotation; // ���� ȸ�� ����

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // ���� �÷��̾�Ը� �������ɾ� Ʈ����
                TriggerJumpScare();
            }
        }
    }

    void TriggerJumpScare()
    {
        mainCamera = Camera.main;
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
