using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // 회전 속도
    [SerializeField] private Transform playerTransform; // 플레이어 Transform
    [SerializeField] private Transform cameraObject; // 카메라 오브젝트 Transform
    [SerializeField] GameObject player; // 플레이어 오브젝트

    public Transform mainCamera; // 메인 카메라 Transform
    public Transform playerModel; // 플레이어 모델링 Transform
    public Transform headBone; // 머리 본 (Head Bone)

    private float mouseX = 0f; // 좌우 회전 값
    private float mouseY = 0f; // 위아래 회전 값
    [SerializeField] private Vector3 offset; // 카메라와 플레이어 사이 거리

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;

    PlayerState playerState;
    PlayerState.playerState state;

    public bool popup_escMenu = false; // esc 메뉴 활성화 여부

    public bool isControlledExternally = false;  // 외부에서 카메라를 제어하는 동안 true로 설정

    void Start()
    {
        if (this.transform.parent != null)
        {
            foreach (GameObject Player_ in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (Player_.GetComponent<PhotonView>().IsMine)
                {
                    player = this.transform.parent.gameObject;
                    playerTransform = Player_.transform;
                }
            }
            // PlayerModel 찾기
            playerModel = playerTransform.Find("캐릭터모델링");
            if (playerModel == null)
            {
                Debug.LogError("PlayerModel object not found under the player prefab.");
                return;
            }

            // HeadBone 찾기
            headBone = playerModel.Find("rabbit:Hips/rabbit:Spine/rabbit:Spine1/rabbit:Spine2/rabbit:Neck/Head");
            if (headBone == null)
            {
                headBone = playerModel.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
                if (headBone == null)
                {
                    Debug.LogError("HeadBone object not found in PlayerModel.");
                    return;
                }
            }
        }
        else
        {
            Debug.LogError("Player's parent object not found. Make sure the prefab is instantiated correctly.");
            return;
        }
        // ī�޶� ������Ʈ �Ҵ��� ���� �� ������Ʈ ã��
        if (cameraObject == null)
        {
            cameraObject = playerTransform.Find("Camera"); // �÷��̾��� �ڽ� �� "Camera"��� �̸��� �� ������Ʈ�� ã��
            if (cameraObject == null)
            {
                Debug.LogError("Camera object not found under player. Ensure that there is a child named 'Camera'.");
                return;
            }
        }

       // player = this.gameObject.GetComponent<Transform>().parent.gameObject;
        playerTransform = player.transform;
        playerState = player.gameObject.GetComponent<PlayerState>();
        if (PhotonNetwork.IsConnected)
        {
            pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                GetComponent<AudioListener>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                GetComponent<AudioListener>().enabled = false;
                Destroy(FollowCam);
                Destroy(EquipCamera);
                Destroy(this.gameObject);
            }
        }
        else
        {
            GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // 동적으로 생성된 플레이어 오브젝트 찾기
        if (this.transform.parent != null)
        {
            player = this.transform.parent.gameObject;
            playerTransform = player.transform;

            // Player Modeling 찾기
            playerModel = playerTransform.Find("캐릭터모델링"); // "PlayerModel"은 모델링의 이름
            if (playerModel == null)
            {
                Debug.LogError("PlayerModel object not found under the player prefab.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player's parent object not found. Make sure the prefab is instantiated correctly.");
            return;
        }
    }

    void Update()
    {

        playerState.GetState(out state);
        if (popup_escMenu || state == PlayerState.playerState.Die) // esc 메뉴 활성화 또는 죽었을 때
            return;

        if (isControlledExternally)
            return;

        mouseSpeed = GameInfo.MouseSensitivity;

        if (PhotonNetwork.IsConnected)
        {
            if (pv.IsMine)
            {
                cameraPos();
            }
        }
        else
        {
            cameraPos();
        }

        if (mainCamera != null && playerModel != null)
        {
            // 카메라의 Y축 회전을 모델링에 동기화
            Vector3 cameraRotation = mainCamera.rotation.eulerAngles;
            playerModel.rotation = Quaternion.Euler(0, cameraRotation.y, 0);

            // 카메라의 X축 회전을 머리 본에 적용
            if (headBone != null)
            {
                float headRotationX = Mathf.Clamp(mouseY, -30f, 30f); // 머리 각도 제한 (예: -30도 ~ 30도)
                headBone.localRotation = Quaternion.Euler(headRotationX, 0, 0);
            }
        }
    }

    void cameraPos()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);
        this.transform.position = cameraObject.position;
    }
}
