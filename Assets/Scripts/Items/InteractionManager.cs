using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public interface IInteractable
{
    string GetInteractPrompt();//프롬프트 받아오는 메서드
    void OnInteract();//상호작용 후 실행 될 메서드
}
public class InteractionManager : MonoBehaviour
{   //아이템 상호작용 매니저

    public float checkRate = 0.05f;
    float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    GameObject curInteractGameobject;
    IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    new Camera camera;

    void Start()
    {
        camera =  Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //마지막으로 체크한 시간이 checkRate를 넘겼다면
        if (Time.time - lastCheckTime > checkRate)
        { 
            lastCheckTime = Time.time;
            // 화면의 정중앙에 상호작용 가능한 물체가 있는지 확인하기

            //화면 중앙에서 Ray 발사
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            RaycastHit hit;

            //ray에 뭔가 충돌했다면 hit에 충돌한 오브젝트에 대한 정보가 넘어감
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //부딪힌 오브젝트가 우리가 저장해 놓은 상호작용이 가능한 오브젝트인지 확인
                if (hit.collider.gameObject != curInteractGameobject)
                {
                    //충돌한 물체 가져오기
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                //화면 중앙에 상호작용 가능한 물체가 없는 경우
                curInteractGameobject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
        OnInteractInput();
    }

    public void OnInteractInput()
    {
        //E키를 누른 시점에서 현재 바라보는 curInteractable 오브젝트가 있다면
        if (Input.GetKeyDown(KeyCode.F) && curInteractable != null)
        {
            //아이템을 흭득하면 아이템과 상호작용을 진행하고 초기화
            curInteractable.OnInteract();

            //빈곳 번호 찾기
            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null)
                    GetComponent<Player_Equip>().numderKeySelectSlot(i+1);
            }

            curInteractGameobject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    void SetPromptText()
    { 
        promptText.gameObject.SetActive(true);

        //<b></b> : <b>는 볼트체
        promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.GetInteractPrompt());
    }
}
