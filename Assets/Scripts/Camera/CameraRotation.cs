using Photon.Pun;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // 회전 속도
    [SerializeField] private Transform playerTransform; // 플레이어의 Transform

    [SerializeField] GameObject player; // 플레이어

    private float mouseX = 0f; // 좌우 회전 값
    private float mouseY = 0f; // 위아래 회전 값
    [SerializeField] private Vector3 offset; // 카메라와 플레이어 사이의 간격

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;
    public bool popup_escMenu = false; // esc T/F 여부

    void Start()
    {
        player = this.gameObject.GetComponent<Transform>().parent.gameObject;
        playerTransform = player.transform;

        if (PhotonNetwork.IsConnected)
        {
            pv = player.GetComponent<PhotonView>();

            if (pv.IsMine)
            {
                GetComponent<AudioListener>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;

                // 플레이어의 자식 중 'camera'라는 이름의 오브젝트 찾기
                Transform cameraParentTransform = playerTransform.Find("camera");
                if (cameraParentTransform != null)
                {
                    // 메인 카메라를 'camera' 오브젝트의 자식으로 설정
                    Camera.main.transform.SetParent(cameraParentTransform);

                    // 위치 및 회전 초기화
                    Camera.main.transform.localPosition = Vector3.zero;
                    Camera.main.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogError("'camera' 오브젝트를 찾을 수 없습니다!");
                }
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
    }

    void Update()
    {
        if (popup_escMenu) // esc 창이 열려있으면 카메라 회전X
            return;

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
    }

    void cameraPos()
    {
        // 마우스 입력을 받아 카메라 회전 처리
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // 위아래 회전 각도 제한
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // 카메라의 회전 적용 (플레이어의 회전을 따라감)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // 카메라 위치를 플레이어 위치에 고정
        this.transform.position = playerTransform.position + offset;
    }
}
