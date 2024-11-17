using Photon.Pun;
using UnityEngine;

public class PhotonOfflineModeManager : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true; // �������� ��� Ȱ��ȭ
            Debug.Log("Photon �������� ��� Ȱ��ȭ. ���� �׽�Ʈ ��...");
        }
    }
}
