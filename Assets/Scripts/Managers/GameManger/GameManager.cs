using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
   void CreatePlayer(); //�÷��̾� ����
}

public class GameManager : Singleton<GameManager>
{

}
