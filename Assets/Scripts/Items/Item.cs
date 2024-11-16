using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // �������� �̸�
    public Transform playerHand; // �������� ������ ��ġ (�÷��̾��� �� ��ġ�� ī�޶� ����)

    // �÷��̾ �����۰� ��ȣ�ۿ��� �� ȣ��Ǵ� �޼���
    public virtual void OnInteract()
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>(); // ������ �÷��̾��� �κ��丮�� ã��
        if (playerInventory != null)
        {
            playerInventory.PickUpItem(this); // �� �������� �÷��̾��� �κ��丮�� �߰�

            // �������� �÷��̾��� �ڽ����� �ű��, �� ��ġ�� ����
            transform.SetParent(playerHand);
            transform.localPosition = Vector3.zero; // �÷��̾��� �� ��ġ�� �°� ����
            transform.localRotation = Quaternion.identity;

            // ������ ȹ�� ó��
            FlashlighFunction flashlightController = GetComponent<FlashlighFunction>();
            if (flashlightController != null)
            {
                flashlightController.enabled = true; // ������ ��� �����ϵ��� Ȱ��ȭ
                flashlightController.AcquireFlashlight(); // ������ ȹ�� ó��
                Debug.Log("�������� �÷��̾�� �����ϰ� ȹ�� ó�� �Ϸ�");
            }
            else
            {
                Debug.Log("FlashlightController ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log("PlayerInventory�� ã�� �� �����ϴ�.");
        }
    }
}
