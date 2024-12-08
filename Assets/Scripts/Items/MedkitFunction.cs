using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MedkitFunction : ItemFunction, IItemFunction
{
    private PhotonItem _PhotonItem; // PhotonItem 참조
    public float itemRemoveDelay = 1f; // 아이템 제거 딜레이

    void Start()
    {
        _PhotonItem = GetComponentInParent<PhotonItem>(); // PhotonItem 컴포넌트 가져오기
    }

    public void Function()
    {
        Debug.Log("Medkit 사용됨");
        StartCoroutine(RemoveItemAfterDelay());
    }

    IEnumerator RemoveItemAfterDelay()
    {
        yield return new WaitForSeconds(itemRemoveDelay);

        if (_PhotonItem != null && _PhotonItem.photonView != null)
        {
            _PhotonItem.RemoveEquippedItem(GetComponent<ItemObject>().item.ItemName);
            Inventory.instance.RemoveItem(GetComponent<ItemObject>().item.ItemName);
            Destroy(GetComponentInParent<Player_Equip>().Item);
            Tesettext();
        }
        else
        {
            Debug.LogWarning("PhotonItem 또는 Inventory가 null입니다. Medkit 제거 실패");
        }
    }
}
