using UnityEngine;
using Photon.Pun;

public class CameraCulling : MonoBehaviourPun
{
    [SerializeField] private Camera playerCamera; // �÷��̾��� ī�޶� ����

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        PhotonView pv = GetComponent<PhotonView>();

        if (pv != null && pv.IsMine && playerCamera != null)
        {
            // �ڽſ��Ը� �ش��ϴ� ���, LocalPlayerBody ���̾ ī�޶󿡼� ����
            playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("LocalPlayerBody"));
        }
    }
}
