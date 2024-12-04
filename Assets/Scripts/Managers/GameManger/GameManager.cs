using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : Singleton<GameManager>
{
  protected int i_Escape_Basement_ItemCount = 0;//부착된 잠수함 탈출 아이템 개수
  protected int i_Escape_Helicopter_ItemCount = 0;//부착된 헬리콥터 탈출 아이템 개수
  public abstract void CreatePlayer();//플레이어 생성
  public abstract void CreateEnemy();//몬스터 생성
}
