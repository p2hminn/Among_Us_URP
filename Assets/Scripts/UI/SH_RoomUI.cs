using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SH_RoomUI : MonoBehaviour
{
    // ���� ���̸�
    string roomName;
    // �� ���� �ο�
    int currPlayerNum;
    // �� ���� �ִ� �ο�
    int maxPlayerNum;
    // Start ��ư
    public Button btn_Start;


    public Text txt_RoomName;
    public Text txt_PlayerNum;

    void Start()
    {
        btn_Start.interactable = false;

        roomName = PhotonNetwork.CurrentRoom.Name;
        currPlayerNum = PhotonNetwork.CurrentRoom.PlayerCount;
        maxPlayerNum = PhotonNetwork.CurrentRoom.MaxPlayers;

        // ���̸� UI text
        txt_RoomName.text = roomName;
    }

    
    void Update()
    {
        // ���� �ο� / ���� �ִ� �ο� UI text ������Ʈ
        txt_PlayerNum.text = $"{currPlayerNum}/{maxPlayerNum}";

        // ���� ���� �ο��� 4���̸�  Start ��ư  interatable Ȱ��ȭ
        if (currPlayerNum == 4)
        {
            btn_Start.interactable = true;
        }
    }
}
