using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        // W Ű�� ������ �ȱ� ���·� ��ȯ
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
