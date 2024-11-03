using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePhoton : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main_Screen");
    }
}
