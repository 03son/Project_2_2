using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // �÷��̾ ��ȣ�ۿ��� �� �ִ� �Ÿ�
    public LayerMask interactionLayer; // �÷��̾ ��ȣ�ۿ��� �� �ִ� ������Ʈ�� ���̾�
    private Camera playerCamera; // �÷��̾��� ī�޶� ����

    void Start()
    {
        playerCamera = Camera.main; // �÷��̾��� ���� ī�޶� ��������
        interactionLayer = LayerMask.GetMask("Item", "Interactable"); // "Item"�� "Interactable" ���̾� ��� ��ȣ�ۿ� �����ϵ��� ����
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // �÷��̾ 'E' Ű�� ������ ��
        {
            Interact(); // ��ȣ�ۿ� �޼��� ȣ��
        }
    }

    // ��ȣ�ۿ��� ó���ϴ� �޼���
    void Interact()
    {
        // ī�޶� ��ġ���� �� �������� ���� ����
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // ���̸� ��� ������ �Ÿ��� ���̾� ������ ������Ʈ�� �浹�ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Debug.Log("���̰� �浹�� ������Ʈ: " + hit.collider.gameObject.name); // ���̰� �浹�� ������Ʈ�� �̸� ���
            Item item = hit.collider.GetComponent<Item>(); // �浹�� ������Ʈ���� Item ������Ʈ ��������
            if (item != null)
            {
                Debug.Log("��ȣ�ۿ� ������ ������ �߰�: " + item.itemName); // ��ȣ�ۿ� ������ ������ �߰� �� �α� ���
                item.OnInteract(); // �������� ��ȣ�ۿ� �޼��� ȣ��
            }
        }
        else
        {
            Debug.Log("����ĳ��Ʈ�� �ƹ��͵� �������� ���߽��ϴ�."); // ���̰� �ƹ��͵� �������� ������ �� �α� ���
        }
    }
}
