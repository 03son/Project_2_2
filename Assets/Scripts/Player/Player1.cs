using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_RoomInfo;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Player1 : MonoBehaviour
{
    HashTable playerCP;

    PhotonView pv;

    GameObject mainCam;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)//�̱� �÷���
        {
            FindCam();
            return;
        }

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //��Ƽ �÷���
        {
            FindCam();

            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("animalName"))//ĳ���� �Ҵ�
            {
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("animalName", out object _animalName);
                if ((string)_animalName == "������")
                {
                    //4�� ĳ���� �� ���� 1��
                }
                else
                {
                    //������ ĳ���ͷ� 1��
                }
                Debug.Log((string)_animalName);
            }
        }
        if (PhotonNetwork.IsConnected)//��Ƽ�� ����
        {
            notMine();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindCam()
    {
        // Main Camera, Follow Cam, EquipCamera�� ã�� �÷��̾� Ʈ�������� �θ�� ����
        Transform mainCameraTransform = GameObject.Find("Main Camera")?.transform;
        Transform followCamTransform = GameObject.Find("Follow Cam")?.transform;
        Transform equipCameraTransform = GameObject.Find("EquipCamera")?.transform;

        if (mainCameraTransform != null)
        {
            mainCameraTransform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Main Camera�� ã�� �� �����ϴ�.");
        }

        if (followCamTransform != null)
        {
            followCamTransform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Follow Cam�� ã�� �� �����ϴ�.");
        }

        if (equipCameraTransform != null)
        {
            equipCameraTransform.SetParent(mainCameraTransform); // EquipCamera�� Main Camera�� �ڽ����� ����
            equipCameraTransform.localPosition = Vector3.zero; // �θ� ���� ��ġ �ʱ�ȭ
            equipCameraTransform.localRotation = Quaternion.identity; // �θ� ���� ȸ�� �ʱ�ȭ
        }
        else
        {
            Debug.LogWarning("EquipCamera�� ã�� �� �����ϴ�.");
        }
    }


    void notMine() //�ڱ� �ڽ��� �ƴϸ� ��ũ��Ʈ�� �κ��丮 UI ��Ȱ��ȭ
    {
        if (!pv.IsMine)
        {
            GetComponent<Inventory>().enabled = false;
            GetComponent<InteractionManager>().enabled = false;
            GetComponent<Player_Equip>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
            GetComponent<PlayerCrouch>().enabled = false;
            GetComponent<RevivePlayer>().enabled = false;
        }
    }
}