using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class JM_ColorManager : MonoBehaviourPun
{
    public Material crewMat;
    public static JM_ColorManager instance;
    // 색깔 리스트
    public List<Color> colorList = new List<Color>();
    Color curColor;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            // 플레이어 색상 정리
            int randomNum = Random.Range(0, 18);
           // SetColor(randomNum);
        }      
    }

    void Update()
    {
        
    }

   

    // 색 지정 함수
    public void SetColor(int colorCode)
    {
        crewMat.SetColor("_PlayerColor", colorList[colorCode]);
        
        /*
        // 빨강
        if (colorCode == 0)
        {
            crewMat.SetColor("_PlayerColor", colorList[0]);
            // crewMatColor = colorList[0];
        }
        // 파랑
        else if (colorCode == 1)
        {
            crewMat.SetColor("_PlayerColor", colorList[1]);
        }
        // 초록
        else if (colorCode == 2)
        {
            crewMat.SetColor("_PlayerColor", colorList[2]);
        }
        // 분홍
        else if (colorCode == 3)
        {
            crewMat.SetColor("_PlayerColor", colorList[3]);
        }
        // 주황
        else if (colorCode == 4)
        {
            crewMat.SetColor("_PlayerColor", colorList[4]);
        }
        // 노랑
        else if (colorCode == 5)
        {
            crewMat.SetColor("_PlayerColor", colorList[5]);
        }
        // 검정
        else if (colorCode == 6)
        {
            crewMat.SetColor("_PlayerColor", colorList[6]);
        }
        // 하양
        else if (colorCode == 7)
        {
            crewMat.SetColor("_PlayerColor", colorList[7]);
        }
        // 보라
        else if (colorCode == 8)
        {
            crewMat.SetColor("_PlayerColor", colorList[8]);
        }
        // 갈색
        else if (colorCode == 9)
        {
            crewMat.SetColor("_PlayerColor", colorList[9]);
        }
        // 청록
        else if (colorCode == 10)
        {
            crewMat.SetColor("_PlayerColor", colorList[10]);
        }
        // 연두
        else if (colorCode == 11)
        {
            crewMat.SetColor("_PlayerColor", colorList[11]);
        }
        // 적갈색
        else if (colorCode == 12)
        {
            crewMat.SetColor("_PlayerColor", colorList[12]);
        }
        // 장미색
        else if (colorCode == 13)
        {
            crewMat.SetColor("_PlayerColor", colorList[13]);
        }
        // 바나나색
        else if (colorCode == 14)
        {
            crewMat.SetColor("_PlayerColor", colorList[14]);
        }
        // 회색ㄹ
        else if (colorCode == 15)
        {
            crewMat.SetColor("_PlayerColor", colorList[15]);
        }
        // 황갈색
        else if (colorCode == 16)
        {
            crewMat.SetColor("_PlayerColor", colorList[16]);
        }
        // 산호색
        else if (colorCode == 17)
        {
            crewMat.SetColor("_PlayerColor", colorList[17]);
        }
        */
    }
}

    
      
