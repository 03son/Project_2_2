using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;

    void Start()
    {
        roomName.text = $"πÊ¿Ã∏ß : {PhotonNetwork.CurrentRoom.Name}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
