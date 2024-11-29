using System.Collections.Generic; // Dictionary 사용을 위해 필요
using UnityEngine; // Unity 기본 API
using Photon.Pun; // Photon 네트워크 동기화를 위해 필요

public class ItemManager : MonoBehaviourPun // Photon 동기화를 위해 photonView 사용
{
    // 싱글톤 패턴으로 쉽게 접근할 수 있도록 설정
    public static ItemManager Instance;

    // 아이템 상태를 저장할 Dictionary (이름, 획득 여부)
    private Dictionary<string, bool> itemStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 ItemManager가 있으면 제거
        }
    }

    // 아이템 상태를 변경 (획득 처리)
    public void SetItemState(string itemName, bool isPickedUp)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp); // 새로운 아이템이면 추가
        }
        else
        {
            itemStates[itemName] = isPickedUp; // 기존 아이템이면 상태 업데이트
        }

        // 모든 클라이언트에 상태 동기화
        photonView.RPC("SyncItemState", RpcTarget.AllBuffered, itemName, isPickedUp);
    }

    // 아이템 상태 동기화 (RPC로 호출)
    [PunRPC]
    private void SyncItemState(string itemName, bool isPickedUp)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp);
        }
        else
        {
            itemStates[itemName] = isPickedUp;
        }

        // 해당 아이템 비활성화 처리
        GameObject item = GameObject.Find(itemName);
        if (item != null)
        {
            item.SetActive(!isPickedUp); // 획득된 아이템이면 비활성화
        }
    }

    // 아이템 상태 가져오기
    public bool GetItemState(string itemName)
    {
        return itemStates.ContainsKey(itemName) && itemStates[itemName];
    }

    [PunRPC] // RPC 어트리뷰트 추가
    public void RPC_HandleItemPickup()
    {
        // 아이템 비활성화 처리
        gameObject.SetActive(false);
    }
}
