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
    public float r;
    public float g;
    public float b;
    public float a;

    // 색깔 저장 리스트
    public List<Color> colorList2 = new List<Color>();

    public Color localColor;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    // 색상리스트에서 해당 색상 삭제
    public void UpdateColorInfo(int input)
    {
        colorList.RemoveAt(input);
    }


    [PunRPC]
    public void RPC_GetColor(int rn)
    {
        colorList.RemoveAt(rn);
    }

    public void GetColor(int rn)
    {
        int randomNum = Random.Range(0, 18);
        curColor = colorList[rn];
        r = curColor.r;
        g = curColor.g;
        b = curColor.b;
        a = curColor.a;

        photonView.RPC("RPC_GetColor", RpcTarget.AllBuffered, rn);
    }

    [PunRPC]
    void RPC_SetColor(Material mat)
    {
        int randomNum = Random.Range(10, 18);
        // Material mat = crew.GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", colorList[randomNum]);
    }

    // 색 지정 함수
    public void SetColor(Material mat)
    {
        photonView.RPC("RPC_SetColor", RpcTarget.AllBuffered, mat);
        // crewMat.SetColor("_PlayerColor", colorList[colorCode]);
        
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

    
      
