using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MedkitFunction : ItemFunction, IItemFunction
{
    private PhotonItem _PhotonItem; // PhotonItem ����
    public float itemRemoveDelay = 1f; // ������ ���� ������

    void Start()
    {
        _PhotonItem = GetComponentInParent<PhotonItem>(); // PhotonItem ������Ʈ ��������
    }

    public void Function()
    {
        Debug.Log("Medkit ����");
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
            Debug.LogWarning("PhotonItem �Ǵ� Inventory�� null�Դϴ�. Medkit ���� ����");
        if (targetPlayer.GetComponent<PlayerState>().State == PlayerState.playerState.Die)
        {
            if (isHolding && targetPlayer != null)
            {
                holdCounter += Time.deltaTime;

                if (holdCounter >= holdTime)
                {
                    targetPlayer.Revive(); // �÷��̾� ��Ȱ
                    Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");
                    Destroy(gameObject); // ��� �� ���޻��� ����
                    ResetHold();
                    return;
                }
            }
        }
    }
}
