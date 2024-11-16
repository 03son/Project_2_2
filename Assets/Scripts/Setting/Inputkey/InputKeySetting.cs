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

    protected static bool isClickChangeButton = false; //������ Ű�� 1������ ����

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
        keyText.text = "�����Ϸ��� Ű�� �Է��ϼ���.";
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
                // ��Ʈ��, ����Ʈ , �����̽� Ű ����
                bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                bool isSpaceBar = Input.GetKey(KeyCode.Space);

                if (isShiftPressed | isCtrlPressed | isSpaceBar) //XOR ���������� �Ǵ�
                {
                    if (isShiftPressed) //����Ʈ
                    {
                        if (Input.GetKey(KeyCode.LeftShift)) //���� ����Ʈ
                        {
                            keyText.text = "LeftShift";
                        } 
                        else if (Input.GetKey(KeyCode.RightShift)) //������ ����Ʈ
                        {
                            keyText.text = "RightShift";
                        }
                    }
                    if (isCtrlPressed) //��Ʈ��
                    {
                        if (Input.GetKey(KeyCode.LeftControl)) //���� ��Ʈ��
                        {
                            keyText.text = "LeftControl";
                        }
                        else if (Input.GetKey(KeyCode.RightControl)) //������ ��Ʈ��
                        {
                            keyText.text = "RightControl";
                        }
                    }
                    if (isSpaceBar) //�����̽� ��
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
            }// ��Ʈ��, ����Ʈ Ű ����

            yield return new WaitForNextFrameUnit();
        }
    }

    protected virtual void SetChangeKey()
    {
        keyText.fontSize = 24;
        isClickChangeButton = false;
        //Ű ����
    }
    KeyCode StringToKeyCode(string keyString)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
    }

    bool Bool_StringToKeyCode(string keyString)
    {
        KeyCode keyCode;
        // ���ڿ��� ��ȿ�� KeyCode���� Ȯ�� �� ��ȯ
        if (Enum.TryParse(keyString, true, out keyCode))
        {
            return true;
        }
        else
        {
            Debug.LogWarning("������ �� ����");
            return false; // �⺻�� ��ȯ
        }
    }

    public virtual void ResetKey()
    {

    }
}
