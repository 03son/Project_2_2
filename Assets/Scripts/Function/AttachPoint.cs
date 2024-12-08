using Photon.Pun;
using UnityEngine;
using TMPro;
public class AttachPoint : MonoBehaviourPun, IInteractable
{
    public string requiredItemName; // �ʿ��� ������ �̸� (��: "Propeller")
    public GameObject attachedItemPrefab; // ������ �������� ������
    private bool isAttached = false; // �̹� �����Ǿ����� ����

    // IInteractable �������̽��� ��ȣ�ۿ� ������Ʈ ��ȯ �޼���
    public string GetInteractPrompt()
    {
        if (isAttached)
            return ""; // �̹� �����Ǿ����� �� ���ڿ� ��ȯ
        else
            return $"{requiredItemName} �����ϱ�"; // ���� �������� �ʾ����� ������Ʈ ǥ��
    }
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    PhotonView pv;
    Player_Equip playerEquip;
    PhotonItem photonItem;
    GameObject _Player;
    public void OnInteract()
    {
        if (isAttached)
        {
            Debug.Log("�̹� ������");
            return;
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerEquip = player.GetComponent<Player_Equip>();
                photonItem = player.GetComponent<PhotonItem>();
                _Player = player;
            }
                    
         }

         if (playerEquip != null && playerEquip.HasEquippedItem(requiredItemName))
        {
            Debug.Log($"{requiredItemName} �������� �����Ǿ� ����. ���� ����");

            // ������ ����
            //photonItem.RemoveEquippedItem(requiredItemName);

            //�÷��̾� �տ� �ִ� �� ����
            if (photonItem != null && photonItem.gameObject.GetComponent<PhotonView>() != null)
            {
                photonItem.RemoveEquippedItem(requiredItemName);
                Inventory.instance.RemoveSselectedItem(Inventory.instance.selectedItemIndex);
                Destroy(_Player.GetComponent<Player_Equip>().Item);
                GameObject.Find("ItemName_Text").GetComponent<TextMeshProUGUI>().text = "";
            }

            // ���� ���� ����ȭ (RPC ȣ��)
            pv.RPC("RPC_AttachItem", RpcTarget.All);
            Debug.Log("RPC_AttachItem ȣ���"); // ȣ�� Ȯ�ο� �α�
        }
        else
        {
            Debug.Log($"{requiredItemName}��(��) �ʿ��մϴ�.");
        }
    }


    [PunRPC]
    void RPC_AttachItem()
    {
        Debug.Log($"{requiredItemName} ���� �Ϸ�");

        // GameObject item = PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{attachedItemPrefab.name}", transform.position, transform.rotation);
        GameObject Item_ = Resources.Load<GameObject>($"Prefabs/Items/{attachedItemPrefab.name}");
        GameObject item = Instantiate(Item_, transform.position, transform.rotation);
        item.GetComponent<Rigidbody>().isKinematic = true;
        isAttached = true;

        SubmarineController submarine = GetComponentInParent<SubmarineController>();
        if (submarine != null)
        {
            submarine.AttachItem(requiredItemName);
            Debug.Log("����Կ� ������ ������ ���� ���� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("����� ��Ʈ�ѷ��� �����ϴ�.");
        }
    }
}
