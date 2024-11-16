using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    private void Start()
    {
        // �ʿ��� �ʱ�ȭ �ڵ尡 �ִٸ� ���⿡ �ۼ�
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������ �ݶ��̴��� �÷��̾�� �浹�� ��
        if (other.CompareTag("Player"))
        {
            // PlayerDeathManager ��ũ��Ʈ�� ������ Die �޼��� ȣ��
            PlayerDeathManager playerHealth = other.GetComponent<PlayerDeathManager>();

            if (playerHealth != null)
            {
                playerHealth.Die(); // �÷��̾��� Die �޼��� ȣ��
                Debug.Log("�÷��̾ ���Ϳ��� ���� �׾����ϴ�.");
            }
            else
            {
                Debug.LogWarning("PlayerDeathManager ������Ʈ�� �÷��̾�� ã�� �� �����ϴ�.");
            }
        }
    }
}
