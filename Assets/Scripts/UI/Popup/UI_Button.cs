using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    enum GameObjects
    { 
     
    }
    enum Buttons
    {
        TestButton
    }
    enum Texts
    { 
        TMP_TestText
    }
    enum Images
    { 
    
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        //Get<Text>((int)Texts.TestText).text = "Test";
        GetText((int)Texts.TMP_TestText).text = "Bind Test";

        GetButton((int)Buttons.TestButton).gameObject.AddUIEvent(OnButtonClicked);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("Click Test");

    }
}
