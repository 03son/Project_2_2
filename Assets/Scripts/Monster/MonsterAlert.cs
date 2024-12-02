using Photon.Pun;
using UnityEngine;

public class MonsterAlert : MonoBehaviourPun
{
    public AudioClip monsterRoar;    // ���� ��� �Ҹ�
    public AudioClip sirenSound;     // ���̷� �Ҹ�
    public float alertDelay = 30f;   // �˸��� ���۵� ������ (�� ����)
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (PhotonNetwork.IsMasterClient)
        {
            // Ÿ�̸Ӹ� �����ϰ� ���� �ð��� ���� �� �˸��� Ʈ����
            Invoke(nameof(TriggerAlert), alertDelay);
        }
    }

    [PunRPC]
    void PlayAlertSounds()
    {
        if (audioSource != null)
        {
            // ���� ��� �Ҹ�
            audioSource.PlayOneShot(monsterRoar);
            // ���̷� �Ҹ� 1�� �ڿ� ���
            Invoke(nameof(PlaySiren), 1f);
        }
    }

    void PlaySiren()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(sirenSound);
        }
    }

    void TriggerAlert()
    {
        // ��� Ŭ���̾�Ʈ���� �˸� �Ҹ� ���
        photonView.RPC("PlayAlertSounds", RpcTarget.All);
    }
}
