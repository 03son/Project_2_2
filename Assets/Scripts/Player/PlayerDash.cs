using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;

    PhotonView pv;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        // 대쉬 기능 (왼쪽 Shift 키를 누르고 있는 동안 작동)
        if (Input.GetKey(KeyManager.Run_Key))
        {
            Vector3 dashDirection = cameraTransform.forward;
            dashDirection.y = 0f; // 대쉬는 수평 방향으로만 적용
            dashDirection.Normalize();
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        }
    }
}