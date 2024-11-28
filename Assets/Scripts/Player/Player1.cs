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
    }
    void Start()
    {
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
        // "Main Camera", "Follow Cam", "EquipCamera"를 찾아 "Camera" 빈 오브젝트의 자식으로 설정
        Transform mainCameraTransform = GameObject.Find("Main Camera")?.transform;
        Transform followCamTransform = GameObject.Find("Follow Cam")?.transform;
        Transform equipCameraTransform = GameObject.Find("EquipCamera")?.transform;
        Transform cameraEmptyTransform = this.transform.Find("Camera"); // 플레이어의 자식 오브젝트 중 "Camera"라는 이름을 가진 빈 오브젝트 찾기

        if (cameraEmptyTransform == null)
        {
            Debug.LogWarning("Camera 빈 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (mainCameraTransform != null)
        {
            mainCameraTransform.SetParent(cameraEmptyTransform); // Main Camera를 빈 오브젝트의 자식으로 설정
            mainCameraTransform.localPosition = Vector3.zero; // 부모 기준 위치 초기화
            mainCameraTransform.localRotation = Quaternion.identity; // 부모 기준 회전 초기화
        }
        else
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다.");
        }

        if (followCamTransform != null)
        {
            followCamTransform.SetParent(cameraEmptyTransform); // Follow Cam도 빈 오브젝트의 자식으로 설정
            followCamTransform.localPosition = Vector3.zero; // 부모 기준 위치 초기화
            followCamTransform.localRotation = Quaternion.identity; // 부모 기준 회전 초기화
        }
        else
        {
            Debug.LogWarning("Follow Cam을 찾을 수 없습니다.");
        }

        if (equipCameraTransform != null)
        {
            equipCameraTransform.SetParent(mainCameraTransform); // EquipCamera를 Main Camera의 자식으로 설정
            equipCameraTransform.localPosition = Vector3.zero; // 부모 기준 위치 초기화
            equipCameraTransform.localRotation = Quaternion.identity; // 부모 기준 회전 초기화
        }
        else
        {
            Debug.LogWarning("EquipCamera를 찾을 수 없습니다.");
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
        }
    }
}
