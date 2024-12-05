using System.Collections.Generic;
using UnityEngine;

public class CanHearSoundSource : MonoBehaviour
{
    public LayerMask detectableLayers; // ���� ���̾ ������ �� �ִ� LayerMask
    private List<Collider> objectsInTrigger = new List<Collider>();

    private float checkInterval = 1f; // �˻� �ֱ�
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // ���� ���̾� �� �ϳ��� ���ϴ��� Ȯ��
        if ((detectableLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            objectsInTrigger.Add(other);
            //Debug.Log($"{other.gameObject.name}��(��) Ʈ���ſ� ���� (���̾�: {LayerMask.LayerToName(other.gameObject.layer)})");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInTrigger.Contains(other))
        {
            objectsInTrigger.Remove(other);
            //Debug.Log($"{other.gameObject.name}��(��) Ʈ���ſ��� ����");
        }
    }

    private void Update()
    {
        // �˻� �ֱ⿡ ���� ����
        timer += Time.deltaTime;
        if (timer < checkInterval)
            return;

        timer = 0f; // Ÿ�̸� �ʱ�ȭ

        foreach (var obj in objectsInTrigger)
        {
            if (obj == null) continue; // �̹� ������ ������Ʈ�� ����
            // Player Ȯ�� (���̾� �̸� Ȯ��)
            string layerName = LayerMask.LayerToName(obj.gameObject.layer);
            if (layerName == "Player")
            {
                PlayerState playerState = obj.GetComponent<PlayerState>();
                //Debug.Log(playerState);
                if (playerState != null && playerState.State == PlayerState.playerState.Die)
                {
                    //Debug.Log("�÷��̾ ��� �����Դϴ�. ���� �ߴ�.");
                    return;
                }
            }
            // SoundSource ó��
            SoundSource soundSource = obj.GetComponent<SoundSource>();
            if (soundSource != null)
            {
                float decibel = soundSource.GetDecibelAtDistance(transform.position);
                if (decibel >= MonsterAI.Instance.minDecibelToDetect)
                {
                    //Debug.Log($"���� �ҽ� ����: {soundSource.gameObject.name}, ���ú�: {decibel}");
                    MonsterAI.Instance.HandleItemSound(obj.transform.position);
                }
            }
        }
    }
}
