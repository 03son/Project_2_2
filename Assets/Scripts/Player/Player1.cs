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
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)//싱글 플레이
        {
            FindCam();
            return;
        }

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //멀티 플레이
        {
            FindCam();

            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("animalName"))//캐릭터 할당
            {
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("animalName", out object _animalName);
                if ((string)_animalName == "무작위")
                {
                    //4개 캐릭터 중 랜덤 1택
                }
                else
                {
                    //선택한 캐릭터로 1택
                }
                Debug.Log((string)_animalName);
            }
        }
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)//멀티일 때만
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
        // Main Camera, Follow Cam, EquipCamera를 찾아 플레이어 트랜스폼을 부모로 설정
        Transform mainCameraTransform = GameObject.Find("Main Camera")?.transform;
        Transform followCamTransform = GameObject.Find("Follow Cam")?.transform;
        Transform equipCameraTransform = GameObject.Find("EquipCamera")?.transform;

        if (mainCameraTransform != null)
        {
            mainCameraTransform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다.");
        }

        if (followCamTransform != null)
        {
            followCamTransform.SetParent(this.transform);
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


    void notMine() //자기 자신이 아니면 스크립트와 인벤토리 UI 비활성화
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
