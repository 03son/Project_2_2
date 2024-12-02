using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private float shakeMagnitude = 0.1f; // 기본 진동 강도

    public void SetShakeMagnitude(float magnitude)
    {
        shakeMagnitude = magnitude; // 진동 강도를 설정
    }

    public IEnumerator Shake(float duration)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
