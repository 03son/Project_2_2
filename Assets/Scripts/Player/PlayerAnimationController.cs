using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    public void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    public void SetRunning(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
    }
}
