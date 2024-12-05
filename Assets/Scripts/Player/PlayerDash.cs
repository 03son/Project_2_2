using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float moveSpeed = 5f; // �⺻ �̵� �ӵ� �߰�

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

        HandleMovement();
    }

    private void HandleMovement()
    {
        // �÷��̾� ������ �Է� �ޱ�
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // �̵� ���� ���
        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0f; // ���� ���� ����
        direction.Normalize(); // ���� ���� ����ȭ

        // �̵� �ӵ� ���� (Shift Ű�� ������ ���� ���� ������Ʈ �ӵ��� �̵�)
        float speed = (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0) ? dashSpeed : moveSpeed;

        // �̵� ó��
        controller.Move(direction * speed * Time.deltaTime);

        // ������ ���� ���ο� ���� �ִϸ��̼� ����
        if (speed == dashSpeed)
        {
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
        else
        {
            // ������Ʈ ����
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningWithItem", false);
        }
    }
}
