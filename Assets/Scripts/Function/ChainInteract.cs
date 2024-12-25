using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class ChainInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "절단기"; // 필요한 아이템 이름
    public float holdTime = 10f;           // 홀드 시간
    private bool isChainRemoved = false;   // 사슬 제거 여부
    private float holdProgress = 0f;       // 홀드 진행 시간
    private bool isHolding = false;        // 홀드 중인지 여부

    [Header("UI Settings")]
    public Image holdTimeBar;              // UI 타임바 이미지 추가 (동그랗게 표시되는 타임바)

    [Header("Audio Settings")]
    public AudioSource audioSource;        // AudioSource 컴포넌트
    public AudioClip cuttingSound;         // 사슬 절단 중 사운드
    public AudioClip cutCompleteSound;     // 절단 완료 사운드

    public string GetInteractPrompt()
    {
        if (isChainRemoved)
            return ""; // 사슬이 이미 제거되었으면 텍스트 없음
        return $"{requiredItem}로 사슬 제거하기 ({holdProgress:F1}/{holdTime:F1}초)";
    }

    public void OnInteract()
    {
        if (isChainRemoved) return;

        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip == null || !playerEquip.HasEquippedItem(requiredItem))
        {
            Debug.Log($"{requiredItem}이(가) 필요합니다.");
            return;
        }

        isHolding = true; // 홀드 시작

        // 타임바 초기화 및 활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.fillAmount = 0f; // 타임바를 초기 상태로 설정
            holdTimeBar.gameObject.SetActive(true); // 타임바 활성화
        }

        // 절단 중 사운드 시작
        if (cuttingSound != null && audioSource != null)
        {
            audioSource.clip = cuttingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (isHolding && !isChainRemoved)
        {
            if (Input.GetKey(KeyCode.F)) // F 키를 꾹 누르는 중
            {
                holdProgress += Time.deltaTime;

                // 타임바 업데이트
                if (holdTimeBar != null)
                {
                    holdTimeBar.fillAmount = holdProgress / holdTime; // 진행 시간에 따른 타임바 채우기
                }

                if (holdProgress >= holdTime)
                {
                    CompleteInteract(); // 작업 완료
                }
            }
            else
            {
                CancelInteract(); // 키를 뗐을 경우 진행 취소
            }
        }
    }

    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // 타임바 비활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        // 절단 완료 사운드
        if (audioSource != null)
        {
            audioSource.Stop(); // 절단 중 소리 정지
            if (cutCompleteSound != null)
            {
                audioSource.PlayOneShot(cutCompleteSound); // 완료 사운드 재생
            }
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonItem photonItem = player.GetComponent<PhotonItem>();
                if (photonItem != null)
                {
                    photonItem.RemoveEquippedItem(requiredItem);
                    Debug.Log($"{requiredItem} 아이템이 제거되었습니다.");
                }
            }
        }

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_RemoveChain", RpcTarget.All);
        }
        else
        {
            RPC_RemoveChain();
        }
    }

    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // 타임바 비활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("사슬 제거 취소됨");
    }

    [PunRPC]
    private void RPC_RemoveChain()
    {
        if (isChainRemoved) return;

        isChainRemoved = true;
        HelicopterController helicopter = GetComponentInParent<HelicopterController>();

        if (PhotonNetwork.IsConnected)
        {
            helicopter?.photonView.RPC("RemoveChain", RpcTarget.All);
        }
        else
        {
            helicopter?.RemoveChain();
        }

        Debug.Log("사슬이 제거되었습니다.");
    }
}