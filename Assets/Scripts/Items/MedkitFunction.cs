using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MedkitFunction : ItemFunction, IItemFunction
{
    private PhotonItem _photonItem; // PhotonItem 참조
    public float itemRemoveDelay = 1f; // 아이템 제거 딜레이
    public GameObject targetPlayer; // 대상 플레이어
    public bool isHolding = false; // 홀드 상태
    public float holdTime = 2.5f; // 부활을 위한 홀드 시간
    private float holdCounter = 0f; // 홀드 타이머

    void Start()
    {
        _photonItem = GetComponentInParent<PhotonItem>(); // PhotonItem 컴포넌트 가져오기
    }

    public void Function()
    {
        Debug.Log("Medkit 사용");
        StartCoroutine(RemoveItemAfterDelay());
    }

    IEnumerator RemoveItemAfterDelay()
    {
        yield return new WaitForSeconds(itemRemoveDelay);

        if (_photonItem != null && _photonItem.photonView != null)
        {
            // Photon 아이템 제거 로직
            _photonItem.RemoveEquippedItem(GetComponent<ItemObject>().item.ItemName);
            Inventory.instance.RemoveItem(GetComponent<ItemObject>().item.ItemName);
            Destroy(GetComponentInParent<Player_Equip>().Item);
        }
        else
        {
            Debug.LogWarning("PhotonItem 또는 Inventory가 null입니다. Medkit 제거 실패");
        }
    }

    void Update()
    {
        // 플레이어 부활 로직
        if (targetPlayer != null && targetPlayer.GetComponent<PlayerState>().State == PlayerState.playerState.Die)
        {
            if (isHolding)
            {
                holdCounter += Time.deltaTime;

                if (holdCounter >= holdTime)
                {
                    targetPlayer.GetComponent<PlayerState>().Revive(); // 플레이어 부활
                    Debug.Log("플레이어가 부활했습니다.");
                    Destroy(gameObject); // Medkit 제거
                    ResetHold();
                }
            }
            else
            {
                ResetHold();
            }
        }
    }

    private void ResetHold()
    {
        holdCounter = 0f;
        isHolding = false;
    }
}
