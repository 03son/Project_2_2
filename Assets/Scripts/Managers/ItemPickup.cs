using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    public itemData item;

    [PunRPC]
    public void RPC_HandleItemPickup()
    {
        // ������ ��Ȱ��ȭ ó��
        gameObject.SetActive(false);
       // Debug.Log($"{item.ItemName}��(��) ȹ��Ǿ� ��Ȱ��ȭ�Ǿ����ϴ�.");
    }

    public void OnInteract()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Photon RPC�� ��� Ŭ���̾�Ʈ���� ��Ȱ��ȭ
            photonView.RPC("RPC_HandleItemPickup", RpcTarget.AllBuffered);
        }
        else
        {
            // �̱��÷��� ó��
            gameObject.SetActive(false);
        }
    }
}
