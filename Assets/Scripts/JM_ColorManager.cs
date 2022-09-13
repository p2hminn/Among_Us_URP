using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ColorManager : MonoBehaviour
{
    public Material crewMat;
    public static JM_ColorManager instance;
    // 색깔 리스트
    public List<Color> colorList = new List<Color>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        // 플레이어 색상 정리
        SetColor("red");
    }

    // 색 지정 함수
    public void SetColor(string colorCode)
    {
        // 빨강
        if (colorCode == "red")
        {
            crewMat.SetColor("_PlayerColor", colorList[0]);
            // crewMatColor = colorList[0];
        }
        // 파랑
        else if (colorCode == "blue")
        {
            crewMat.SetColor("_PlayerColor", colorList[1]);
        }
        // 초록
        else if (colorCode == "green")
        {
            crewMat.SetColor("_PlayerColor", colorList[2]);
        }
        // 분홍
        else if (colorCode == "pink")
        {
            crewMat.SetColor("_PlayerColor", colorList[3]);
        }
        // 주황
        else if (colorCode == "orange")
        {
            crewMat.SetColor("_PlayerColor", colorList[4]);
        }
        // 노랑
        else if (colorCode == "yellow")
        {
            crewMat.SetColor("_PlayerColor", colorList[5]);
        }
        // 검정
        else if (colorCode == "black")
        {
            crewMat.SetColor("_PlayerColor", colorList[6]);
        }
        // 하양
        else if (colorCode == "white")
        {
            crewMat.SetColor("_PlayerColor", colorList[7]);
        }
        // 보라
        else if (colorCode == "purple")
        {
            crewMat.SetColor("_PlayerColor", colorList[8]);
        }
        // 갈색
        else if (colorCode == "brown")
        {
            crewMat.SetColor("_PlayerColor", colorList[9]);
        }
        // 청록
        else if (colorCode == "cyan")
        {
            crewMat.SetColor("_PlayerColor", colorList[10]);
        }
        // 연두
        else if (colorCode == "lime")
        {
            crewMat.SetColor("_PlayerColor", colorList[11]);
        }
        // 적갈색
        else if (colorCode == "maroon")
        {
            crewMat.SetColor("_PlayerColor", colorList[12]);
        }
        // 장미색
        else if (colorCode == "rose")
        {
            crewMat.SetColor("_PlayerColor", colorList[13]);
        }
        // 바나나색
        else if (colorCode == "banana")
        {
            crewMat.SetColor("_PlayerColor", colorList[14]);
        }
        // 회색ㄹ
        else if (colorCode == "gray")
        {
            crewMat.SetColor("_PlayerColor", colorList[15]);
        }
        // 황갈색
        else if (colorCode == "tan")
        {
            crewMat.SetColor("_PlayerColor", colorList[16]);
        }
        // 산호색
        else if (colorCode == "coral")
        {
            crewMat.SetColor("_PlayerColor", colorList[17]);
        }
    }

    

}
