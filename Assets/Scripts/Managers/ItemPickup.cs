using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    public itemData item;

    //  [PunRPC]
    /*  public void RPC_HandleItemPickup()
      {
          // ������ ��Ȱ��ȭ ó��
          gameObject.SetActive(false);
         // Debug.Log($"{item.ItemName}��(��) ȹ��Ǿ� ��Ȱ��ȭ�Ǿ����ϴ�.");
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
            // �̱��÷��� ó��
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
