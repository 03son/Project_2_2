using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviourPun
{
    protected PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
}
