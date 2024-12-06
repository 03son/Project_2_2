using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    TextMeshProUGUI NickNameText;

    public bool FirstRun;

    PlayerState playerState;
    PlayerState.playerState state;

    int playerNumber = 0;

    bool escMenu = false;
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
    private void OnEnable()
    {
        if (PhotonNetwork.IsConnected)
        {
            playerObjects = GameObject.FindGameObjectsWithTag("Player");

            NickNameText = GameObject.Find("NickNameText").GetComponent<TextMeshProUGUI>();
            NickNameText.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }
    public CinemachineFreeLook freeLookCamera;

    private float lastXAxisValue; // 마지막 X축 값
    private float lastYAxisValue; // 마지막 Y축 값
    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
            if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu)
            {
                if (!escMenu)
                {
                    escMenu = true;
                    LockRotation();
                    return;
                }
            }
            if(CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu == false)
            {
                if (escMenu)
                {
                    escMenu = false;
                    UnlockRotation();
                    return;
                }
            }
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
                    PlayerNickName();
                    break;
                case 1:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    PlayerNickName();
                    break;
                case 2:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    PlayerNickName();
                    break;
                case 3:
                    target = playerObjects[index].transform;
                    CameraMove();
                    LookAround();
                    PlayerNickName();
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

    void PlayerNickName()
    {
        NickNameText.text = playerObjects[playerNumber].GetComponent<PhotonView>().Owner.NickName;
    }

    private void CameraMove()
    {
        GetComponent<CinemachineFreeLook>().Follow = target;
    }

    private void LookAround()
    {
        GetComponent<CinemachineFreeLook>().LookAt = target;
    }

    public void LockRotation()
    {
        // 입력을 비활성화하여 카메라 회전 고정
        freeLookCamera.m_XAxis.m_InputAxisName = ""; // X축(수평) 입력 제거
        freeLookCamera.m_YAxis.m_InputAxisName = ""; // Y축(수직) 입력 제거

        // 현재 X축과 Y축 값을 저장
        lastXAxisValue = freeLookCamera.m_XAxis.Value;
        lastYAxisValue = freeLookCamera.m_YAxis.Value;

        // 저장된 값을 회전값으로 고정
        freeLookCamera.m_XAxis.Value = lastXAxisValue;
        freeLookCamera.m_YAxis.Value = lastYAxisValue;

        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
    }
    public void UnlockRotation()
    {
        // 입력 복원하여 카메라 회전 활성화
        freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
        freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";

        freeLookCamera.m_YAxis.m_MaxSpeed = 2;
        freeLookCamera.m_XAxis.m_MaxSpeed = 300;
    }
}
