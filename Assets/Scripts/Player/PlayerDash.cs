using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator �߰�
    PhotonView pv;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator ������Ʈ ��������
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        // ������Ʈ ��� (���� Shift Ű�� ������ �ִ� ���� �۵�)
        if (Input.GetKey(KeyManager.Run_Key))
        {
            HandleDash();
        }
        else
        {
            animator.SetBool("isRunning", false); // ������Ʈ ����
        }
    }

    private void HandleDash()
    {
        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        // ������Ʈ ó��
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);

        // Animator ������Ʈ (�޸��� ���·� ��ȯ)
        animator.SetBool("isRunning", true);
    }
}
