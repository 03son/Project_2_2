using UnityEngine;
using Photon.Pun;

public class PlayerLayerAssignment : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            // 이름이 "토끼모델링"인 오브젝트를 찾아서 LocalPlayerBody 레이어로 설정
            Transform rabbitModel = transform.Find("토끼모델링");
            if (rabbitModel != null)
            {
                SetLayerRecursively(rabbitModel.gameObject, LayerMask.NameToLayer("LocalPlayerBody"));
            }
        }
        else
        {
            // 이름이 "토끼모델링"인 오브젝트를 찾아서 RemotePlayerBody 레이어로 설정
            Transform rabbitModel = transform.Find("토끼모델링");
            if (rabbitModel != null)
            {
                SetLayerRecursively(rabbitModel.gameObject, LayerMask.NameToLayer("RemotePlayerBody"));
            }
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        // 해당 오브젝트와 모든 자식 오브젝트에 레이어를 설정
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
