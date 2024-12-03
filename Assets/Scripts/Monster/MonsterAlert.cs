using Photon.Pun;
using UnityEngine;

public class MonsterAlert : MonoBehaviourPun
{
    public AudioClip monsterRoar;    // 괴물 비명 소리
    public AudioClip sirenSound;     // 사이렌 소리
    public float alertDelay = 30f;   // 알림이 시작될 딜레이 (초 단위)
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (PhotonNetwork.IsMasterClient)
        {
            // 타이머를 시작하고 일정 시간이 지난 후 알림을 트리거
            Invoke(nameof(TriggerAlert), alertDelay);
        }
    }

    [PunRPC]
    void PlayAlertSounds()
    {
        if (audioSource != null)
        {
            // 괴물 비명 소리
            audioSource.PlayOneShot(monsterRoar);
            // 사이렌 소리 1초 뒤에 재생
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
        // 모든 클라이언트에서 알림 소리 재생
        photonView.RPC("PlayAlertSounds", RpcTarget.All);
    }
}
