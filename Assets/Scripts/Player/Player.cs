using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance; // �÷��̾� �ν��Ͻ� �̱���
    public AudioSource audioSource; // ����� �ҽ�

    void Awake()
    {
        // �̱��� ����: �̹� �����ϴ� �ν��Ͻ��� ������ �ı��ϰ�, ������ �Ҵ�
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // ����� �ҽ� ��������
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Player ������Ʈ�� AudioSource ������Ʈ�� ����. ����� �ҽ��� �߰��ؾ� ��.");
        }
    }

    // �ٸ� Player ���� �޼��峪 ������ ���� �߰��ϸ� ��.
}
