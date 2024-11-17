using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;
    public float baseDecibel = 50f;  // �⺻ ���ú� ��
    public float range = 20f;        // �Ҹ��� �ִ� ����

    // Ư�� ��ġ������ ���ú��� ����ϴ� �Լ�
    public float GetDecibelAtDistance(Vector3 position)
    {
        if (audioSource.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, position);
            float decibel = baseDecibel - 20f * Mathf.Log10(distance);
            return Mathf.Max(0, decibel); // ���ú��� ������ ���� �ʵ��� Ŭ����
        }
        return 0; // ����� �ҽ��� ��� ���� �ƴϸ� ���ú��� 0���� ��ȯ
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ����� Ŭ���� �������� �ʾҴٸ� ���� �޽��� ���
        if (audioSource.clip == null)
        {
            Debug.LogError("AudioSource�� AudioClip�� �Ҵ���� �ʾҽ��ϴ�. ����� Ŭ���� ������ �ּ���.");
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    void OnDrawGizmos()
    {
        // AudioSource ��������
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // ����� �ҽ� ��ġ�� ���� Gizmo ���� ����
        Gizmos.color = new Color(0, 1, 0, 0.3f); // �������� �ʷϻ�

        // Min Distance �ݰ� �׸���
        Gizmos.DrawWireSphere(transform.position, audioSource.minDistance);

        // Max Distance �ݰ� �׸���
        Gizmos.color = new Color(1, 0, 0, 0.3f); // �������� ������
        Gizmos.DrawWireSphere(transform.position, audioSource.maxDistance);
    }
}
