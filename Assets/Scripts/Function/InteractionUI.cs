using UnityEngine;
using Photon.Pun; // Photon ���

public class InteractionUI : MonoBehaviourPun
{
    public GameObject uiImage; // UI Image�� �巡�׷� ����
    private bool isPlayerInRange = false;
    private PhotonView localPlayerView; // ���� �÷��̾��� PhotonView

    void Update()
    {
        // ���� �÷��̾ UI�� ������ �� ����
        if (localPlayerView != null && localPlayerView.IsMine && isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            uiImage.SetActive(!uiImage.activeSelf); // UI ���¸� ���
        }
    }

    // �÷��̾ ��ȣ�ۿ� ������ ������ ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();
            if (playerView != null && playerView.IsMine) // ���� �÷��̾����� Ȯ��
            {
                localPlayerView = playerView; // ���� �÷��̾��� PhotonView ����
                isPlayerInRange = true; // ��ȣ�ۿ� ���� ���·� ����
                uiImage.SetActive(true); // UI �ڵ� Ȱ��ȭ
            }
        }
    }

    // �÷��̾ ��ȣ�ۿ� ������ ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();
            if (playerView != null && playerView.IsMine) // ���� �÷��̾����� Ȯ��
            {
                localPlayerView = null; // ���� �÷��̾���� ���� ����
                isPlayerInRange = false; // ��ȣ�ۿ� �Ұ��� ���·� ����
                uiImage.SetActive(false); // UI ��Ȱ��ȭ
            }
        }
    }
}
