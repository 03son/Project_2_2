using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator �߰�
    private PhotonView pv;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator ������Ʈ ��������
        pv = GetComponent<PhotonView>();
        playerEquip = GetComponent<Player_Equip>(); // Player_Equip ������Ʈ ��������
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
            // ������Ʈ ����
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningWithItem", false);
        }
    }

    private void HandleDash()
    {
        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        // ������Ʈ ó��
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);

        // ������ ���� ���ο� ���� �ִϸ��̼� ����
        if (playerEquip != null && playerEquip.HasAnyEquippedItem())
        {
            animator.SetBool("isRunningWithItem", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isRunningWithItem", false);
        }
    }
}
