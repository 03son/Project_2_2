using UnityEngine;
using Photon.Pun;

public class CameraCulling : MonoBehaviourPun
{
    [SerializeField] private Camera playerCamera; // 플레이어의 카메라를 연결

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        PhotonView pv = GetComponent<PhotonView>();

        if (pv != null && pv.IsMine && playerCamera != null)
        {
            // 자신에게만 해당하는 경우, LocalPlayerBody 레이어를 카메라에서 제외
            playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("LocalPlayerBody"));
        }
    }
}
