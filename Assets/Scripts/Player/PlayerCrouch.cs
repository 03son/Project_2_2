using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 1.0f;    // 앉았을 때 높이
    public float normalHeight = 2.0f;    // 서 있을 때 높이
    public float crouchSpeed = 0.1f;     // 앉기와 서기 전환 속도
    private CharacterController characterController;
    private Animator animator;           // Animator 추가

    private Vector3 normalCenter;    // 기본 center 값 저장
    private Vector3 crouchCenter;    // 앉았을 때 center 값

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

        if (characterController == null)
        {
            Debug.LogError("CharacterController가 플레이어에 할당되어 있지 않습니다.");
            return;
        }

        // 기본 높이와 앉았을 때 높이에 맞는 center 값 계산
        normalCenter = characterController.center;
        crouchCenter = new Vector3(normalCenter.x, normalCenter.y - (normalHeight - crouchHeight) / 2, normalCenter.z);
    }

    void Update()
    {
        if (characterController == null || animator == null) return;

        // Control 키 입력 상태 확인
        bool isCrouching = Input.GetKey(KeyManager.SitDown_Key);

        // 애니메이터 파라미터 설정 (isCrouching을 bool로 사용)
        animator.SetBool("isCrouching", isCrouching);

        // 높이와 center 변경
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        Vector3 targetCenter = isCrouching ? crouchCenter : normalCenter;

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, crouchSpeed);
    }
}
