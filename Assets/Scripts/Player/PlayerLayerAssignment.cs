using UnityEngine;
using Photon.Pun;

public class PlayerLayerAssignment : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            // �̸��� "�䳢�𵨸�"�� ������Ʈ�� ã�Ƽ� LocalPlayerBody ���̾�� ����
            Transform rabbitModel = transform.Find("�䳢�𵨸�");
            if (rabbitModel != null)
            {
                SetLayerRecursively(rabbitModel.gameObject, LayerMask.NameToLayer("LocalPlayerBody"));
            }
        }
        else
        {
            // �̸��� "�䳢�𵨸�"�� ������Ʈ�� ã�Ƽ� RemotePlayerBody ���̾�� ����
            Transform rabbitModel = transform.Find("�䳢�𵨸�");
            if (rabbitModel != null)
            {
                SetLayerRecursively(rabbitModel.gameObject, LayerMask.NameToLayer("RemotePlayerBody"));
            }
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        // �ش� ������Ʈ�� ��� �ڽ� ������Ʈ�� ���̾ ����
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
