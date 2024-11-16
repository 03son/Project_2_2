using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 1.0f;    // 앉았을 때 높이
    public float normalHeight = 2.0f;    // 서 있을 때 높이
    public float crouchSpeed = 0.1f;     // 앉기와 서기 전환 속도
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController가 플레이어에 할당되어 있지 않습니다.");
        }
    }

    void Update()
    {
        // Control 키를 누르고 있는 동안 crouchHeight로 전환, 떼면 normalHeight로 돌아감
        float targetHeight = Input.GetKey(KeyCode.LeftControl) ? crouchHeight : normalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
    }
}
