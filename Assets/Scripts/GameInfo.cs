using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo //���� ����
{
    public static string InGameScenes = "Ver_0.1"; //�ΰ��� �� �̸�
    public static float MouseSensitivity;//���콺 ����
    public static bool IsMasterClient;
    public static bool IsGameFinish = false; //������ �����°�? ���� or Ŭ���� ����
    public static int endingNumber; //���� ���� 0 = ����Ż��, 1 = �Ϻ� ����, 2 = �������
}
