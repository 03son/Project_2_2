using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f; // �Է��� �����ؾ� �ϴ� �ð�
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // Ÿ�� �÷��̾��� PlayerDeathManager
    public MedkitTimerUI medkitTimerUI; // Ÿ�̸� UI ��ũ��Ʈ

    void Start()
    {
        // MedkitTimerUI�� Inspector���� ������� �ʾ��� ��� �ڵ����� ã��
        if (medkitTimerUI == null)
        {
            medkitTimerUI = FindObjectOfType<MedkitTimerUI>();
            if (medkitTimerUI != null)
            {
                Debug.Log("MedkitTimerUI ���� ����: " + medkitTimerUI.name);
            }
            else
            {
                Debug.LogError("MedkitTimerUI�� ã�� �� �����ϴ�. ���� MedkitTimerUI�� �ִ��� Ȯ���ϼ���.");
            }
        }
    }

    void Update()
    {
        if (targetPlayer != null && Input.GetKey(KeyManager.Interaction_Key)) // ��ȣ�ۿ�
        {
            isHolding = true;
            holdCounter += Time.deltaTime;

            if (medkitTimerUI != null && holdCounter > 0)
            {
                medkitTimerUI.StartMedkitTimer(); // Ÿ�̸� UI ����
            }

            if (holdCounter >= holdTime) // ������ �ð��� ������ ��Ȱ
            {
                //targetPlayer.Revive(); // ��Ȱ ȣ��
                Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyManager.Interaction_Key) || !isHolding) // Ű�� ���ų� ��ȣ�ۿ� �ߴ�
        {
            if (medkitTimerUI != null)
            {
                medkitTimerUI.ResetTimer(); // Ÿ�̸� �ʱ�ȭ
            }
            ResetHold();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerDeathManager>().isDead)
        {
            targetPlayer = other.GetComponent<PlayerDeathManager>();
            Debug.Log("���� �÷��̾� �߰�. E Ű�� ���� ��Ȱ ����.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayer = null;
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
    }
}
