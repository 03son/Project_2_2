using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 1.0f;    // �ɾ��� �� ����
    public float normalHeight = 2.0f;    // �� ���� �� ����
    public float crouchSpeed = 0.1f;     // �ɱ�� ���� ��ȯ �ӵ�
    private CharacterController characterController;
    private Animator animator;           // Animator �߰�

    private Vector3 normalCenter;    // �⺻ center �� ����
    private Vector3 crouchCenter;    // �ɾ��� �� center ��

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������

        if (characterController == null)
        {
            Debug.LogError("CharacterController�� �÷��̾ �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        // �⺻ ���̿� �ɾ��� �� ���̿� �´� center �� ���
        normalCenter = characterController.center;
        crouchCenter = new Vector3(normalCenter.x, normalCenter.y - (normalHeight - crouchHeight) / 2, normalCenter.z);
    }

    void Update()
    {
        if (characterController == null || animator == null) return;

        // Control Ű �Է� ���� Ȯ��
        bool isCrouching = Input.GetKey(KeyManager.SitDown_Key);

        // �ִϸ����� �Ķ���� ���� (isCrouching�� bool�� ���)
        animator.SetBool("isCrouching", isCrouching);

        // ���̿� center ����
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        Vector3 targetCenter = isCrouching ? crouchCenter : normalCenter;

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, crouchSpeed);
    }
}
