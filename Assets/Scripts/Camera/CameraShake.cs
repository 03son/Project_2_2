using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition; // 원래 카메라 위치 저장
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // 진동 크기만큼 랜덤으로 이동
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime; // 시간 경과

            yield return null; // 프레임마다 대기
        }

        transform.localPosition = originalPosition; // 진동 후 원래 위치로 복구
    }
}
