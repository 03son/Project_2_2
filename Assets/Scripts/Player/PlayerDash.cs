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
        if (!pv.IsMine)
            return;

        // �뽬 ��� (���� Shift Ű�� ������ �ִ� ���� �۵�)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 dashDirection = cameraTransform.forward;
            dashDirection.y = 0f; // �뽬�� ���� �������θ� ����
            dashDirection.Normalize();
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        }
    }
}