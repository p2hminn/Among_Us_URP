using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ColorManager : MonoBehaviour
{
    public Material crewMat;
    public static JM_ColorManager instance;
    // ���� ����Ʈ
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
        // �÷��̾� ���� ����
        SetColor("red");
    }

    // �� ���� �Լ�
    public void SetColor(string colorCode)
    {
        // ����
        if (colorCode == "red")
        {
            crewMat.SetColor("_PlayerColor", colorList[0]);
            // crewMatColor = colorList[0];
        }
        // �Ķ�
        else if (colorCode == "blue")
        {
            crewMat.SetColor("_PlayerColor", colorList[1]);
        }
        // �ʷ�
        else if (colorCode == "green")
        {
            crewMat.SetColor("_PlayerColor", colorList[2]);
        }
        // ��ȫ
        else if (colorCode == "pink")
        {
            crewMat.SetColor("_PlayerColor", colorList[3]);
        }
        // ��Ȳ
        else if (colorCode == "orange")
        {
            crewMat.SetColor("_PlayerColor", colorList[4]);
        }
        // ���
        else if (colorCode == "yellow")
        {
            crewMat.SetColor("_PlayerColor", colorList[5]);
        }
        // ����
        else if (colorCode == "black")
        {
            crewMat.SetColor("_PlayerColor", colorList[6]);
        }
        // �Ͼ�
        else if (colorCode == "white")
        {
            crewMat.SetColor("_PlayerColor", colorList[7]);
        }
        // ����
        else if (colorCode == "purple")
        {
            crewMat.SetColor("_PlayerColor", colorList[8]);
        }
        // ����
        else if (colorCode == "brown")
        {
            crewMat.SetColor("_PlayerColor", colorList[9]);
        }
        // û��
        else if (colorCode == "cyan")
        {
            crewMat.SetColor("_PlayerColor", colorList[10]);
        }
        // ����
        else if (colorCode == "lime")
        {
            crewMat.SetColor("_PlayerColor", colorList[11]);
        }
        // ������
        else if (colorCode == "maroon")
        {
            crewMat.SetColor("_PlayerColor", colorList[12]);
        }
        // ��̻�
        else if (colorCode == "rose")
        {
            crewMat.SetColor("_PlayerColor", colorList[13]);
        }
        // �ٳ�����
        else if (colorCode == "banana")
        {
            crewMat.SetColor("_PlayerColor", colorList[14]);
        }
        // ȸ����
        else if (colorCode == "gray")
        {
            crewMat.SetColor("_PlayerColor", colorList[15]);
        }
        // Ȳ����
        else if (colorCode == "tan")
        {
            crewMat.SetColor("_PlayerColor", colorList[16]);
        }
        // ��ȣ��
        else if (colorCode == "coral")
        {
            crewMat.SetColor("_PlayerColor", colorList[17]);
        }
    }

    

}
