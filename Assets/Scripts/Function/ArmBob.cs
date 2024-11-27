using UnityEngine;

public class ArmBob : MonoBehaviour
{
    [SerializeField] private Transform armsTransform; // �� �� Transform
    [SerializeField] private float bobSpeed = 5f;     // ��鸲 �ӵ�
    [SerializeField] private float bobAmount = 0.02f; // ��鸲 ũ��

    private Vector3 originalPosition; // ���� �ʱ� ��ġ
    private float timer = 0f;

    void Start()
    {
        // �� ���� �ʱ� ��ġ ����
        originalPosition = armsTransform.localPosition;
    }

    void Update()
    {
        // �ȱ⳪ �޸��� ���¿� ���� ��鸲 ȿ�� ����
        if (IsMoving())
        {
            timer += Time.deltaTime * bobSpeed;

            // ���� �� �¿� ��鸲 ���
            float bobX = Mathf.Sin(timer) * bobAmount; // �¿� ��鸲
            float bobY = Mathf.Cos(timer * 2f) * bobAmount; // ���� ��鸲

            // ��鸲�� �� ��ġ�� ����
            armsTransform.localPosition = originalPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            // �������� ���� �� �� ��ġ �ʱ�ȭ
            timer = 0f;
            armsTransform.localPosition = originalPosition;
        }
    }

    // ������ ���¸� Ȯ���ϴ� �Լ� (�ܼ��� ��)
    private bool IsMoving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // WASD �Է��� �ִ��� Ȯ��
        return Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
    }
}
