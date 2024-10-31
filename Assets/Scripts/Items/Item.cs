using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // 아이템의 이름
    public Transform playerHand; // 손전등이 장착될 위치 (플레이어의 손 위치나 카메라 하위)

    // 플레이어가 아이템과 상호작용할 때 호출되는 메서드
    public virtual void OnInteract()
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>(); // 씬에서 플레이어의 인벤토리를 찾음
        if (playerInventory != null)
        {
            playerInventory.PickUpItem(this); // 이 아이템을 플레이어의 인벤토리에 추가

            // 손전등을 플레이어의 자식으로 옮기고, 손 위치에 장착
            transform.SetParent(playerHand);
            transform.localPosition = Vector3.zero; // 플레이어의 손 위치에 맞게 조정
            transform.localRotation = Quaternion.identity;

            // 손전등 획득 처리
            FlashlighFunction flashlightController = GetComponent<FlashlighFunction>();
            if (flashlightController != null)
            {
                flashlightController.enabled = true; // 손전등 사용 가능하도록 활성화
                flashlightController.AcquireFlashlight(); // 손전등 획득 처리
                Debug.Log("손전등을 플레이어에게 장착하고 획득 처리 완료");
            }
            else
            {
                Debug.Log("FlashlightController 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.Log("PlayerInventory를 찾을 수 없습니다.");
        }
    }
}
