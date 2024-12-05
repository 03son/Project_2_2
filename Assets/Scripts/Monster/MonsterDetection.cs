using Photon.Pun;
using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    public AudioSource warningSound; // ����� ����� �ҽ�
    public float detectionRange = 20f; // ���� ����
    public float fieldOfView = 120f; // �þ߰�
    public float cooldownTime = 20f; // ����� ��Ÿ�� (�� ����)

    private Transform player; // ���� �÷��̾� Transform
    private bool isWarningPlayed = false;
    private float lastWarningTime = -Mathf.Infinity; // ���������� ����� ����� �ð�

    void Start()
    {
        // ���� �÷��̾� ã��
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>()?.IsMine == true) // ���� �÷��̾����� Ȯ��
            {
                player = obj.transform;
                break;
            }
        }

        if (player == null)
        {
            Debug.LogError("���� �÷��̾ ã�� �� �����ϴ�. 'Player' �±׿� PhotonView ������ Ȯ���ϼ���.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {

        if (player == null) return;

        // PlayerState ������Ʈ�� ������
        PlayerState playerState = player.GetComponent<PlayerState>();

        // �÷��̾ ��� ���¶�� ���� ���� �ߴ�
        if (playerState != null && playerState.State == PlayerState.playerState.Die)
        {
            //Debug.Log("�÷��̾ ��� �����Դϴ�. ���� �ߴ�.");
            return;
        }

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �þ߰� ���� �ִ��� Ȯ��
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (distanceToPlayer <= detectionRange && angleToPlayer <= fieldOfView / 2)
        {
            // ����� ��Ÿ�� Ȯ��
            if (Time.time >= lastWarningTime + cooldownTime)
            {
                warningSound.Play();
                lastWarningTime = Time.time; // ������ ����� ��� �ð� ����
                isWarningPlayed = true;

                Debug.Log("�÷��̾ �߰��Ǿ����ϴ�!");
            }
        }
        else
        {
            isWarningPlayed = false; // ����� ���� �ʱ�ȭ
        }
    }
}
