using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public float baseDecibel = 50f;  // �⺻ ���ú� ��
    public float range = 20f;        // �Ҹ��� �ִ� ����
    public AudioSource audioSource; // ����� �ҽ�

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.clip == null)
        {
            Debug.LogError("AudioClip�� �������� �ʾҽ��ϴ�.");
        }
    }

    // Ư�� ��ġ������ ���ú� ���
    public float GetDecibelAtDistance(Vector3 position)
    {
        if (audioSource.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, position);

            // ���ú� ��� (�Ÿ� ��� ����)
            float decibel = baseDecibel - 10 * Mathf.Log10(distance + 1e-6f);  // �Ÿ� ������� ���ú� �� ���� (����: dB)
            Debug.Log(decibel);
            return Mathf.Max(0, decibel); // ���� ����
        }
        return 0; // �Ҹ��� ��� ���� �ƴϸ� ���ú� 0
    }
    // �Ҹ� ���
    public void PlaySound()
    {
        audioSource.Play();
    }
    // Gizmo�� �Ҹ� ������ ǥ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
