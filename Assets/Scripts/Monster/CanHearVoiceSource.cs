using UnityEngine;

public class CanHearVoiceSource : MonoBehaviour
{
    private float checkInterval = 1f; // 1�� ����
    private float timer = 0f;
    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        if (timer < checkInterval)
            return;
        if (!other.CompareTag("Player"))
            return; // �÷��̾ �ƴϸ� ����
        Mic mic = other.GetComponentInChildren<Mic>();
        if (mic != null)
        {
            float decibel = mic.GetDecibelAtDistance(transform.position);
            //Debug.Log($"��Ҹ� ����: {other.gameObject.name}, ���ú�: {decibel}");
            if (decibel >= MonsterAI.Instance.minDecibelToDetect)
            {
                MonsterAI.Instance.HandlePlayerSound(decibel, other.transform.position);
            }

        }
        timer = 0f; // Ÿ�̸� �ʱ�ȭ
    }
}
