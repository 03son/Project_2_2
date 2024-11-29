using UnityEngine;

public class ArmBob : MonoBehaviour
{
    [SerializeField] private Transform armsTransform; // 팔 모델 Transform
    [SerializeField] private float bobSpeed = 5f;     // 흔들림 속도
    [SerializeField] private float bobAmount = 0.02f; // 흔들림 크기

    private Vector3 originalPosition; // 팔의 초기 위치
    private float timer = 0f;

    void Start()
    {
        // 팔 모델의 초기 위치 저장
        originalPosition = armsTransform.localPosition;
    }

    void Update()
    {
        // 걷기나 달리기 상태에 따라 흔들림 효과 적용
        if (IsMoving())
        {
            timer += Time.deltaTime * bobSpeed;

            // 상하 및 좌우 흔들림 계산
            float bobX = Mathf.Sin(timer) * bobAmount; // 좌우 흔들림
            float bobY = Mathf.Cos(timer * 2f) * bobAmount; // 상하 흔들림

            // 흔들림을 팔 위치에 적용
            armsTransform.localPosition = originalPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            // 움직임이 없을 때 팔 위치 초기화
            timer = 0f;
            armsTransform.localPosition = originalPosition;
        }
    }

    // 움직임 상태를 확인하는 함수 (단순한 예)
    private bool IsMoving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // WASD 입력이 있는지 확인
        return Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
    }
}
