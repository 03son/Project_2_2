using UnityEngine;

public class HelicopterStart : MonoBehaviour, IInteractable
{
    public HelicopterController helicopterController; // �︮���� ��Ʈ�ѷ�
    public Transform equipItem; // �÷��̾��� EquipItem ��ü
    private bool isPlayerNearby = false; // �÷��̾ ��ó�� �ִ��� ����
    public string requiredItemName = "HelicopterKey"; // �ʿ��� ������ �̸�

    // �������̽� �޼���: ��ȣ�ۿ� �޽��� ��ȯ
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

        // EquipItem�� �ڽ� ��ü�� Ȯ��
        Transform equippedItem = equipItem.Find(requiredItemName);
        if (equippedItem != null)
        {
            // �ʿ��� �������� �����Ǿ� ������ �õ� �ɱ�
            Debug.Log($"{requiredItemName}�� ����Ͽ� �︮���� �õ��� �̴ϴ�.");
            Destroy(equippedItem.gameObject); // ������ ��� �� ����
            helicopterController.StartHelicopter();
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
