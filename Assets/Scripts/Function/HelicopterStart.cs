using UnityEngine;

public class HelicopterStart : MonoBehaviour, IInteractable
{
    public HelicopterController helicopterController; // �︮���� ��Ʈ�ѷ�
    public Transform equipItem; // �÷��̾��� EquipItem ��ü
    private bool isPlayerNearby = false; // �÷��̾ ��ó�� �ִ��� ����
    public string requiredItemName = "HelicopterKey"; // �ʿ��� ������ �̸�
    private Player_Equip playerEquip; // �÷��̾��� Player_Equip ��ũ��Ʈ ����

    // �������̽� �޼���: ��ȣ�ۿ� �޽��� ��ȯ

    void Start()
    {
        // playerEquip�� �������� �Ҵ�
        if (playerEquip == null)
        {
            playerEquip = FindObjectOfType<Player_Equip>();
            if (playerEquip == null)
            {
                Debug.LogError("Player_Equip ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }
    public string GetInteractPrompt()
    {
        return $"Ű�� ���� �︮���� �õ� ({requiredItemName} �ʿ�)";
    }

    // �������̽� �޼���: ��ȣ�ۿ� ����
    public void OnInteract()
    {
        if (!isPlayerNearby)
            return;

        if (helicopterController == null)
        {
            Debug.LogError("HelicopterController�� null�Դϴ�. Inspector���� �����ߴ��� Ȯ���ϼ���.");
            return;
        }

        // Player_Equip���� ���� ������ �������� �ʿ��� ���������� Ȯ��
        if (playerEquip != null && playerEquip.HasEquippedItem(requiredItemName))
        {
            // �ʿ��� �������� �����Ǿ� ������ �õ� �õ�
            Debug.Log($"{requiredItemName}�� ����Ͽ� �︮���� �õ��� �̴ϴ�.");

            bool startSuccess = helicopterController.StartHelicopter(); // �õ� �õ� �� ���� ���� ��ȯ

            if (startSuccess)
            {
                // �õ��� �����ϸ� ������ ����
                playerEquip.RemoveEquippedItem(requiredItemName);
                Debug.Log($"{requiredItemName}�� ���Ǿ����ϴ�.");
            }
            else
            {
                // �õ��� �����ϸ� ������ ����
                Debug.Log("�︮���� �õ� ����. �������� �����մϴ�.");
            }
        }
        else
        {
            // �ʿ��� �������� ������
            Debug.Log($"{requiredItemName} �������� �ʿ��մϴ�!");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("�︮���� ��Ʈ�� �г� ������ ����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("�︮���� ��Ʈ�� �г� �������� ���");
        }
    }
}
