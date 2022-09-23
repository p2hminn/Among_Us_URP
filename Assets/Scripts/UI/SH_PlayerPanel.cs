using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviourPun
{
    // �÷��̾� �г���, �÷��̾� ����, �÷��̾� ����(�׾����� ����) ����

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // �г� �� ���� ����
    public void SetInfo(PhotonView photonView)
    {
         // �÷��̾� ����
         Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
         Material mat = transform.GetChild(0).GetComponent<Image>().material;
         mat.SetColor("_PlayerColor", color);

         // �÷��̾� �г���
         transform.GetChild(1).GetComponent<Text>().text = photonView.Owner.NickName;

        print("color : " + color);
        print("nickname : " + photonView.Owner.NickName);
    }
    //public void SetInfo(PhotonView photonView)
    //{
    //    // ũ��
    //    if (photonView.gameObject.CompareTag("Crew"))
    //    {
    //        print("Crew : " + photonView.Owner.NickName);
    //        // �÷��̾� ����
    //        Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
    //        Material mat = transform.GetChild(0).GetComponent<Image>().material;
    //        mat.SetColor("_PlayerColor", color);

    //        // �÷��̾� �г���
    //        transform.GetChild(1).GetComponent<Text>().text = photonView.Owner.NickName;
    //    }
    //    // ��Ʈ
    //    else if (photonView.gameObject.CompareTag("Ghost"))
    //    {
    //        print("Ghost : ");//+ photonView.Owner.NickName);
    //    }
    //}
}
