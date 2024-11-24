using Photon.Pun;
using UnityEngine;

public class PlayerRotationSync : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // 카메라의 Transform (직접 할당)
    private PhotonView pv;

    private float mouseX = 0f; // 좌우 회전 값

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

        // 카메라의 y축 회전을 가져와 플레이어의 y축 회전에 반영
        mouseX = cameraTransform.eulerAngles.y;
        Vector3 playerRotation = transform.eulerAngles;
        playerRotation.y = mouseX;
        transform.eulerAngles = playerRotation;
    }
}
