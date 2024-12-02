using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    public itemData item;

    //  [PunRPC]
    /*  public void RPC_HandleItemPickup()
      {
          // 아이템 비활성화 처리
          gameObject.SetActive(false);
         // Debug.Log($"{item.ItemName}이(가) 획득되어 비활성화되었습니다.");
      } */
    /*
    PhotonView PhotonView;
    public void OnInteract()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView = GetComponent<PhotonView>();
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonView.RPC("PhotonDestroyItem", RpcTarget.All);
        }
        else
        {
            // 싱글플레이 처리
            gameObject.SetActive(false);
        }
    }
    [PunRPC]
    void PhotonDestroyItem()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
    */
}
