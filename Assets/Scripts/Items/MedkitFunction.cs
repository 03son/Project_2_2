using UnityEngine;

public class MedkitFunction : MonoBehaviour, IItemFunction
{
    public float holdTime = 5f; // ��Ȱ�� �� �ʿ��� �ð�
    private float holdCounter = 0f;
    private bool isHolding = false;
    private PlayerDeathManager targetPlayer;

    void Update()
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
            }
        }
    }

    public void Function() // IItemFunction �������̽��� �޼��� ����
    {
        // �� �޼���� ������ ��� �� ȣ��˴ϴ�.
        // ���� ���, �÷��̾ �� �������� ����ϸ� �ش� ����� �����ϵ��� ������ �� �ֽ��ϴ�.
        Debug.Log("Medkit ����� ���۵Ǿ����ϴ�.");
    }

    public void StartRevive(PlayerDeathManager player)
    {
        targetPlayer = player;
        isHolding = true;
    }

    public void StopRevive()
    {
        ResetHold();
    }

    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
        targetPlayer = null;
    }
}
