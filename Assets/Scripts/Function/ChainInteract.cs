using UnityEngine;
using Photon.Pun;

public class ChainInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "Cutter"; // 필요한 아이템 이름
    public float holdTime = 10f;           // 홀드 시간
    private bool isChainRemoved = false;   // 사슬 제거 여부
    private float holdProgress = 0f;       // 홀드 진행 시간
    private bool isHolding = false;        // 홀드 중인지 여부

    public string GetInteractPrompt()
    {
        if (isChainRemoved)
            return ""; // 사슬이 이미 제거되었으면 텍스트 없음
        return $"{requiredItem}로 사슬 제거하기 ({holdProgress:F1}/{holdTime:F1}초)";
    }

    public void OnInteract()
    {
        if (isChainRemoved) return;

        Inventory inventory = Inventory.instance;
        if (!inventory.HasItem(requiredItem))
        {
            Debug.Log($"{requiredItem}이(가) 필요합니다.");
            return;
        }

        isHolding = true; // 홀드 시작
    }

    private void Update()
    {
        if (isHolding && !isChainRemoved)
        {
            if (Input.GetKey(KeyCode.F)) // F 키를 꾹 누르는 중
            {
                holdProgress += Time.deltaTime;
                if (holdProgress >= holdTime)
                {
                    CompleteInteract(); // 작업 완료
                }
            }
            else
            {
                CancelInteract(); // 키를 뗐을 경우 진행 취소
            }

            // 프롬프트 업데이트를 InteractionManager가 실시간으로 호출하도록 강제
            InteractionManager.UpdatePrompt(this);
        }
    }

    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        Inventory inventory = Inventory.instance;
        inventory.RemoveItem(requiredItem); // 아이템 사용

        if (PhotonNetwork.IsConnected)
        {
            // 멀티플레이에서는 RPC 호출
            photonView.RPC("RPC_RemoveChain", RpcTarget.All);
        }
        else
        {
            // 싱글플레이에서는 로컬 메서드 호출
            RPC_RemoveChain();
        }
    }

    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;
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
