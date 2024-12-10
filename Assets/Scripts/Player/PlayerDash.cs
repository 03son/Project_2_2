using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float moveSpeed = 5f; // �⺻ �̵� �ӵ� �߰�
    float speed;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator �߰�
    private PhotonView pv;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����
    private PlayerState playerState; // �÷��̾� ���� ����
    private PlayerState.playerState state;
    SoundSource soundSource;

    void Start()
    {
        soundSource = GetComponent<SoundSource>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator ������Ʈ ��������
        pv = GetComponent<PhotonView>();
        playerEquip = GetComponent<Player_Equip>(); // Player_Equip ������Ʈ ��������
        playerState = GetComponent<PlayerState>(); // PlayerState ��ũ��Ʈ ��������
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;


        // esc �޴��� ���� ������ �۵� �ߴ�
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu)
        {
            StopMovement();
            return;
        }

        // �÷��̾� ���� ��������
        playerState.GetState(out state);

        // �÷��̾� ���°� Survival�� ���� �̵� ó��
        if (state == PlayerState.playerState.Survival)
        {
            HandleMovement();
        }
        else
        {
            // ���°� Die�� ��� �������� ����
            StopMovement();
        }
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
        //= (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0) ? dashSpeed : moveSpeed;

        if (!GetComponent<PlayerMove>().playerCrouch.isCrouching)
        {
            if (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0)
            {
                speed = dashSpeed;
                soundSource.baseDecibel = 80f;
                Decibel_Bar.instance.Decibel_Value(GetComponent<PlayerMove>().walkSound.volume * 1.8f, false);
            }
        }
        else 
        {
            speed = moveSpeed;
        }
        /*if (!GetComponent<PlayerMove>().playerCrouch.isCrouching && Input.GetKey(KeyManager.Run_Key))
        {
            soundSource.baseDecibel = 80f;
            Decibel_Bar.instance.Decibel_Value(GetComponent<PlayerMove>().walkSound.volume * 1.8f , false);
        }*/

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

    private void StopMovement()
    {
        // �̵��� ���߱� ���� Vector3.zero�� ����
        controller.Move(Vector3.zero);

        // �̵� ���� �ִϸ��̼� �ʱ�ȭ
        animator.SetBool("isRunning", false);
        animator.SetBool("isRunningWithItem", false);
    }
}
