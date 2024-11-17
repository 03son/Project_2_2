using Photon.Pun;
using UnityEngine;

public class PhotonOfflineModeManager : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true; // 오프라인 모드 활성화
            Debug.Log("Photon 오프라인 모드 활성화. 로컬 테스트 중...");
        }
    }
}
