using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        // W 키를 누르면 걷기 상태로 전환
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
