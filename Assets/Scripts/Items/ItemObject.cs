using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public itemData item;

    private GlassCupThrower glassCupThrower;

    void Start()
    {
        glassCupThrower = FindObjectOfType<GlassCupThrower>();
        if (glassCupThrower == null)
        {
            Debug.LogError("GlassCupThrower ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    public string GetInteractPrompt()
    {
        return string.Format("�ݱ� {0}", item.ItemName);
    }

    public void OnInteract()
    {
        // ���Կ� �� ������ �ִ��� Ȯ�� �� ������ �߰�
        if (addSlot())
        {
            Inventory.instance.Additem(item);
            Debug.Log($"������ ȹ��: {item.ItemName}");

            // �������� ȹ���� ��� ó��
            if (item.ItemName == "GlassCup")
            {
                Debug.Log("�������� ȹ���Ϸ��� �մϴ�...");

                if (glassCupThrower != null)
                {
                    Debug.Log("GlassCupThrower.PickUpGlassCup ȣ�� �غ��");
                    glassCupThrower.PickUpGlassCup(gameObject);
                    gameObject.SetActive(false); // ��Ȱ��ȭ
                }
                else
                {
                    Debug.LogWarning("GlassCupThrower�� �������� �ʽ��ϴ�. �������� ȹ���� �� �����ϴ�.");
                }
            }
        }
        else
        {
            Debug.LogWarning("�κ��丮�� �� ������ �����ϴ�. �������� ȹ���� �� �����ϴ�.");
        }
    }

    bool addSlot()
    {
        // �κ��丮�� �� ������ �ִ��� Ȯ��
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}
