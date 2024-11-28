using UnityEngine;
using Photon.Pun;

public class HelicopterController : MonoBehaviourPun
{
    private bool isChainRemoved = false; // 사슬 제거 여부
    private bool isFuelAdded = false;   // 연료 주입 여부
    private bool isStarted = false;     // 헬기 시동 여부
    private Player_Equip playerEquip; // 플레이어의 Player_Equip 스크립트 참조

    public AudioSource audioSource;
    public AudioClip startSound;

    private void Start()
    {
        // AudioSource가 없으면 자동으로 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // 사슬 제거 메서드
    [PunRPC]
    public void RemoveChain()
    {
        if (isChainRemoved) return;

        isChainRemoved = true;
        Debug.Log("사슬이 제거되었습니다.");

        // 사슬 오브젝트 비활성화
        Transform chainTransform = transform.Find("Chain");
        if (chainTransform != null)
        {
            chainTransform.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("사슬 오브젝트를 찾을 수 없습니다.");
        }
    }

    // 연료 주입 메서드
    [PunRPC]
    public void AddFuel()
    {
        if (isFuelAdded) return;

        isFuelAdded = true;
        Debug.Log("연료가 주입되었습니다.");
    }

    // 시동 가능 여부 확인
    public bool CanStart()
    {
        return isChainRemoved && isFuelAdded;
    }

    // 헬기 시동 메서드
    public bool StartHelicopter()
    {
        if (isStarted) return false; // 이미 시동이 걸린 상태라면 false 반환

        if (CanStart())
        {
            isStarted = true;
            Debug.Log("헬기 시동 시작!");

            // 오디오 재생
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource 또는 StartSound가 설정되지 않았습니다.");
            }

            // 헬기 탈출 연출 실행
            Invoke(nameof(EscapeSequence), 3.0f);
            return true; // 시동이 성공했으면 true 반환
        }
        else
        {
            Debug.Log("사슬과 연료 상태를 확인하세요.");
            return false; // 시동 조건이 충족되지 않으면 false 반환
        }
    }


    private void EscapeSequence()
    {
        Debug.Log("헬기 탈출 시작!");
        // Timeline 실행 또는 씬 전환 처리
        // 여기에 원하는 탈출 연출 로직 추가
    }

    // 싱글플레이와 멀티플레이 구분
    public void RemoveChainLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(RemoveChain), RpcTarget.All);
        }
        else
        {
            RemoveChain();
        }
    }

    public void AddFuelLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(AddFuel), RpcTarget.All);
        }
        else
        {
            AddFuel();
        }
    }

    public void StartHelicopterLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(StartHelicopter), RpcTarget.All);
        }
        else
        {
            StartHelicopter();
        }
    }

    


}
