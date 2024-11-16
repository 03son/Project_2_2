using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputKeySetting : MonoBehaviour , IResetKey
{
    public Button currentKeyButton;
    protected TextMeshProUGUI keyText;

    protected KeyCode currentKey;

    protected static bool isClickChangeButton = false; //변경할 키는 1개씩만 가능

    protected GameSettingSetValue key;
    void Awake()
    {
        keyText = currentKeyButton.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();

        currentKeyButton.gameObject.AddUIEvent(OnClickKeyButton);

        key = GameSettingSetValue.Instance;
    }

    protected virtual void OnClickKeyButton(PointerEventData button)
    {
        if (!isClickChangeButton) StartCoroutine(ChangeKey());
    }

    protected IEnumerator ChangeKey()
    {
        isClickChangeButton = true;
        keyText.text = "변경하려는 키를 입력하세요.";
        keyText.fontSize = 15;

        while (true)
        {
            if (!string.IsNullOrEmpty(Input.inputString) && Bool_StringToKeyCode(Input.inputString[0].ToString().ToUpper()) && !Input.GetKey(KeyCode.Space))
            {
                char key = Input.inputString[0];
                keyText.text = key.ToString().ToUpper();
                currentKey = StringToKeyCode(keyText.text);
                SetChangeKey();
                yield break;
            }
            else
            {
                // 컨트롤, 쉬프트 , 스페이스 키 감지
                bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                bool isSpaceBar = Input.GetKey(KeyCode.Space);

                if (isShiftPressed | isCtrlPressed | isSpaceBar) //XOR 연산자으로 판단
                {
                    if (isShiftPressed) //쉬프트
                    {
                        if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 쉬프트
                        {
                            keyText.text = "LeftShift";
                        } 
                        else if (Input.GetKey(KeyCode.RightShift)) //오른쪽 쉬프트
                        {
                            keyText.text = "RightShift";
                        }
                    }
                    if (isCtrlPressed) //컨트롤
                    {
                        if (Input.GetKey(KeyCode.LeftControl)) //왼쪽 컨트롤
                        {
                            keyText.text = "LeftControl";
                        }
                        else if (Input.GetKey(KeyCode.RightControl)) //오른쪽 컨트롤
                        {
                            keyText.text = "RightControl";
                        }
                    }
                    if (isSpaceBar) //스페이스 바
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            keyText.text = "Space";
                        }
                    }
                    currentKey = StringToKeyCode(keyText.text);
                    SetChangeKey();
                    yield break;
                }
            }// 컨트롤, 쉬프트 키 감지

            yield return new WaitForNextFrameUnit();
        }
    }

    protected virtual void SetChangeKey()
    {
        keyText.fontSize = 24;
        isClickChangeButton = false;
        //키 저장
    }
    KeyCode StringToKeyCode(string keyString)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
    }

    bool Bool_StringToKeyCode(string keyString)
    {
        KeyCode keyCode;
        // 문자열이 유효한 KeyCode인지 확인 후 변환
        if (Enum.TryParse(keyString, true, out keyCode))
        {
            return true;
        }
        else
        {
            Debug.LogWarning("지정할 수 없음");
            return false; // 기본값 반환
        }
    }

    public virtual void ResetKey()
    {

    }
}
