using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : Singleton<GameManager>
{
  protected int i_Escape_Basement_ItemCount = 0;//������ ����� Ż�� ������ ����
  protected int i_Escape_Helicopter_ItemCount = 0;//������ �︮���� Ż�� ������ ����
  public abstract void CreatePlayer();//�÷��̾� ����
  public abstract void CreateEnemy();//���� ����
}
