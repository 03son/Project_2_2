using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // ��ȣ�ۿ� �Ÿ�
    public LayerMask interactionLayer; // ��ȣ�ۿ��� ���̾�
    private Camera playerCamera; // �÷��̾��� ī�޶� ����

    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    void Start()
    {
        playerCamera = Camera.main;
        interactionLayer = LayerMask.GetMask("Interactable"); // "Interactable" ���̾� ����
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ��ũ��Ʈ ã��
    }

    void Update()
    {
        // ��Ŭ�� �� "Key"��� �̸��� ���谡 ������ ���¶�� ����ĳ��Ʈ �߻�
        if (Input.GetMouseButtonDown(0) && playerEquip.HasEquippedKey())
        {
            InteractWithDoor();
        }
    }

    // ���� ��ȣ�ۿ��ϴ� �޼���
    void InteractWithDoor()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ�� �߻��Ͽ� ���� �浹�ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("���� ��ȣ�ۿ� �õ�");
                door.OnInteract(); // ���� ���� �޼��� ȣ��
            }
            else
            {
                Debug.Log("���� �ƴ� ���� �浹");
            }
        }
        else
        {
            Debug.Log("����ĳ��Ʈ�� �ƹ��͵� �������� ���߽��ϴ�.");
        }
    }
}
