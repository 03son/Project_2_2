using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    public itemData item;

    [PunRPC]
    public void RPC_HandleItemPickup()
    {
        // 아이템 비활성화 처리
        gameObject.SetActive(false);
       // Debug.Log($"{item.ItemName}이(가) 획득되어 비활성화되었습니다.");
    }

    public void OnInteract()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Photon RPC로 모든 클라이언트에서 비활성화
            photonView.RPC("RPC_HandleItemPickup", RpcTarget.AllBuffered);
        }
        else
        {
            // 싱글플레이 처리
            gameObject.SetActive(false);
        }
    }
}
