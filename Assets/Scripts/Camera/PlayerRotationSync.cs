using Photon.Pun;
using UnityEngine;

public class PlayerRotationSync : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ī�޶��� Transform (���� �Ҵ�)
    private PhotonView pv;

    private float mouseX = 0f; // �¿� ȸ�� ��

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        SyncPlayerRotation();
    }

    void SyncPlayerRotation()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
            return;
        }

        // ī�޶��� y�� ȸ���� ������ �÷��̾��� y�� ȸ���� �ݿ�
        mouseX = cameraTransform.eulerAngles.y;
        Vector3 playerRotation = transform.eulerAngles;
        playerRotation.y = mouseX;
        transform.eulerAngles = playerRotation;
    }
}
