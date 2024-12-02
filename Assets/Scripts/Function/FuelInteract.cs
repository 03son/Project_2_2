using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.UI; // UI 관련 네임스페이스 추가
using UnityEngine.EventSystems; // Layout 관련 네임스페이스 추가

public class FuelInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "헬기 연료통"; // 필요한 아이템 이름
    public float holdTime = 10f;         // 홀드 시간
    private bool isFuelAdded = false;    // 연료 추가 여부
    private float holdProgress = 0f;     // 홀드 진행 시간
    private bool isHolding = false;      // 홀드 중인지 여부

    [Header("Audio Settings")]
    public AudioSource audioSource;        // AudioSource 컴포넌트
    public AudioClip fuelAddingSound;      // 연료 주입 중 사운드
    public AudioClip fuelAddedCompleteSound; // 연료 주입 완료 사운드

    [Header("UI Settings")]
    public Image holdTimeBar; // 진행 상황을 표시할 타임바 UI 이미지

    public string GetInteractPrompt()
    {
        // 연료가 이미 추가된 상태면 텍스트를 표시하지 않음
        return isFuelAdded ? "" : $"{requiredItem}로 연료 주입하기 ({holdProgress:F1}/{holdTime:F1}초)";
    }

    public void OnInteract()
    {
        if (isFuelAdded) return; // 이미 연료가 추가된 상태면 실행하지 않음

        // Player_Equip에서 현재 장착된 아이템이 연료인지 확인
        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip == null || !playerEquip.HasEquippedItem(requiredItem))
        {
            Debug.Log($"{requiredItem}이(가) 필요합니다.");
            return;
        }

        isHolding = true; // 홀드 시작



        // 타임바 UI 활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.fillAmount = 0f; // 초기화
            holdTimeBar.gameObject.SetActive(true);
            Debug.Log("FuelInteract - HoldTimeBar Initialized");
        }
        else
        {
            Debug.LogError("FuelInteract - HoldTimeBar is null!");
        }

        // 연료 주입 사운드 시작
        if (fuelAddingSound != null && audioSource != null)
        {
            audioSource.clip = fuelAddingSound;
            audioSource.loop = true; // 반복 재생
            audioSource.Play();
        }
    }

    private void LateUpdate()
    {
        if (isHolding && !isFuelAdded)
        {
            if (Input.GetKey(KeyCode.F))
            {
                holdProgress += Time.deltaTime;

                if (holdTimeBar != null)
                {
                    holdTimeBar.fillAmount = holdProgress / holdTime;
                    Debug.Log($"FuelInteract (LateUpdate) - FillAmount: {holdTimeBar.fillAmount}");
                }

                if (holdProgress >= holdTime)
                {
                    CompleteInteract();
                }
            }
            else
            {
                CancelInteract();
            }
        }
    }




    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // 연료 주입 완료 사운드
        if (audioSource != null)
        {
            audioSource.Stop(); // 주입 중 소리 정지
            if (fuelAddedCompleteSound != null)
            {
                audioSource.PlayOneShot(fuelAddedCompleteSound); // 완료 사운드 재생
            }
        }

        // 타임바 UI 비활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        // 인벤토리에서 연료통 제거
        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip != null)
        {
            playerEquip.RemoveEquippedItem(requiredItem);
            Debug.Log($"{requiredItem} 아이템이 제거되었습니다.");
        }

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_AddFuel", RpcTarget.All);
        }
        else
        {
            RPC_AddFuel();
        }
    }

    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // 주입 중 소리 정지
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // 타임바 UI 비활성화
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        Debug.Log("연료 주입 취소됨");
    }

    [PunRPC]
    private void RPC_AddFuel()
    {
        if (isFuelAdded) return; // 이미 연료가 추가된 상태면 실행하지 않음

        isFuelAdded = true;
        HelicopterController helicopter = GetComponentInParent<HelicopterController>();

        if (PhotonNetwork.IsConnected)
        {
            helicopter?.photonView.RPC("AddFuel", RpcTarget.All); // 멀티플레이에서는 RPC 호출
        }
        else
        {
            helicopter?.AddFuel(); // 싱글플레이에서는 직접 호출
        }

        Debug.Log("연료가 추가되었습니다.");
    }
}
