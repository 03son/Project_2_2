using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // ������ �������� �����ϴ� ����Ʈ
    public List<Item> inventory = new List<Item>();

    // �������� �ݴ� �޼���
    public void PickUpItem(Item item)
    {
        if (item != null)
        {
            inventory.Add(item); // �������� �κ��丮 ����Ʈ�� �߰�
            Debug.Log(item.itemName + " ��(��) �ֿ����ϴ�.");
            item.gameObject.SetActive(false); // �������� �ݰ� ���� ������ ��Ȱ��ȭ
        }
    }
}
