using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo //게임 정보
{
    public static string InGameScenes = "Ver_0.1"; //인게임 씬 이름
    public static float MouseSensitivity;//마우스 감도
    public static bool IsMasterClient;
    public static bool IsGameFinish = false; //게임이 끝났는가? 오버 or 클리어 여부
    public static int endingNumber; //엔딩 종류 0 = 전원탈출, 1 = 일부 생존, 2 = 전원사망
}
