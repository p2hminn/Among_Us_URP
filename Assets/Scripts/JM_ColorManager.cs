using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class JM_ColorManager : MonoBehaviourPun
{
    public Material crewMat;
    public static JM_ColorManager instance;
    // ���� ����Ʈ
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
            // �÷��̾� ���� ����
            int randomNum = Random.Range(0, 18);
           // SetColor(randomNum);
        }      
    }

    void Update()
    {
        
    }

   

    // �� ���� �Լ�
    public void SetColor(int colorCode)
    {
        crewMat.SetColor("_PlayerColor", colorList[colorCode]);
        
        /*
        // ����
        if (colorCode == 0)
        {
            crewMat.SetColor("_PlayerColor", colorList[0]);
            // crewMatColor = colorList[0];
        }
        // �Ķ�
        else if (colorCode == 1)
        {
            crewMat.SetColor("_PlayerColor", colorList[1]);
        }
        // �ʷ�
        else if (colorCode == 2)
        {
            crewMat.SetColor("_PlayerColor", colorList[2]);
        }
        // ��ȫ
        else if (colorCode == 3)
        {
            crewMat.SetColor("_PlayerColor", colorList[3]);
        }
        // ��Ȳ
        else if (colorCode == 4)
        {
            crewMat.SetColor("_PlayerColor", colorList[4]);
        }
        // ���
        else if (colorCode == 5)
        {
            crewMat.SetColor("_PlayerColor", colorList[5]);
        }
        // ����
        else if (colorCode == 6)
        {
            crewMat.SetColor("_PlayerColor", colorList[6]);
        }
        // �Ͼ�
        else if (colorCode == 7)
        {
            crewMat.SetColor("_PlayerColor", colorList[7]);
        }
        // ����
        else if (colorCode == 8)
        {
            crewMat.SetColor("_PlayerColor", colorList[8]);
        }
        // ����
        else if (colorCode == 9)
        {
            crewMat.SetColor("_PlayerColor", colorList[9]);
        }
        // û��
        else if (colorCode == 10)
        {
            crewMat.SetColor("_PlayerColor", colorList[10]);
        }
        // ����
        else if (colorCode == 11)
        {
            crewMat.SetColor("_PlayerColor", colorList[11]);
        }
        // ������
        else if (colorCode == 12)
        {
            crewMat.SetColor("_PlayerColor", colorList[12]);
        }
        // ��̻�
        else if (colorCode == 13)
        {
            crewMat.SetColor("_PlayerColor", colorList[13]);
        }
        // �ٳ�����
        else if (colorCode == 14)
        {
            crewMat.SetColor("_PlayerColor", colorList[14]);
        }
        // ȸ����
        else if (colorCode == 15)
        {
            crewMat.SetColor("_PlayerColor", colorList[15]);
        }
        // Ȳ����
        else if (colorCode == 16)
        {
            crewMat.SetColor("_PlayerColor", colorList[16]);
        }
        // ��ȣ��
        else if (colorCode == 17)
        {
            crewMat.SetColor("_PlayerColor", colorList[17]);
        }
        */
    }
}

    
      
