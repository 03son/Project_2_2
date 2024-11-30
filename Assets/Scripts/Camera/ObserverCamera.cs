using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerState;

public class ObserverCamera : MonoBehaviour
{
    //카메라가 붙을 플레이어 트랜스폼
    [Header("카메라가 붙을 플레이어 트랜스폼")]
    public Transform target;

    //플레이어들 오브젝트 목록
    GameObject[] playerObjects = new GameObject[4];

    bool FirstRun;

    PlayerState playerState;
    PlayerState.playerState state;

    int playerNumber = 0;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            FirstRun = false;
            StartCoroutine(SetCamPos());
            StartCoroutine(SetCameraUpdate());
        }
    }
    IEnumerator SetCamPos()
    {
        yield return new WaitForSecondsRealtime(1);

        //하이어라키창의 플레이어들 탐색
        playerObjects = GameObject.FindGameObjectsWithTag("Player");

        //자신 캐릭터에 붙기
        target = playerObjects[playerNumber].transform;

        playerState = playerObjects[0].GetComponent<PlayerState>();

        CinemachineBrain brain = CameraInfo.ObserverCam.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut; // 즉시 전환
        brain.m_DefaultBlend.m_Time = 0f; // 블렌딩 시간 0으로 설정

        FirstRun = true;
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            //esc 창이 열려있으면 ,open esc
            GetComponent<CinemachineFreeLook>().enabled =
            CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu ? false : true;
        }
    }

    void LateUpdate()
    {
        if (!FirstRun)
            return;

        //다음 사람
        if (Input.GetMouseButtonDown(1))
        {
            playerNumber++;
            Nextplayer(playerNumber);
            return;
        }
        //이전 사람
        if (Input.GetMouseButtonDown(0))
        {
            playerNumber--;
            Nextplayer(playerNumber);
            return;
        }
    }
    void Nextplayer(int index)
    {
        if (!FirstRun)
            return;

        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu == false && state == PlayerState.playerState.Die)
        {
            if (playerNumber > playerObjects.Length-1)
                playerNumber = 0;
            else if (playerNumber < 0)
                playerNumber = playerObjects.Length - 1;

            index = playerNumber;

            if (playerObjects.Length == 1)
            {
                return;
            }

            //해당 번호의 플레이어가 없으면 return
            if (!playerObjects[index])
            {
                return;
            }

            switch (index)
            {
                case 0:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    break;
                case 1:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    break;
                case 2:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    break;
                case 3:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    break;
            }
        }
    }

    IEnumerator SetCameraUpdate()
    {
        yield return new WaitForSecondsRealtime(2);
        LookAround();
        CameraMove();
        FirstRun = true;
    }

    private void CameraMove()
    {
        GetComponent<CinemachineFreeLook>().Follow = target;
    }

    private void LookAround()
    {
        GetComponent<CinemachineFreeLook>().LookAt = target;
    }   
}
