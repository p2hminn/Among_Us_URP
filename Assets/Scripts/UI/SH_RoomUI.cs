using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SH_RoomUI : MonoBehaviour
{
    // 현재 방이름
    string roomName;
    // 방 참가 인원
    int currPlayerNum;
    // 방 참가 최대 인원
    int maxPlayerNum;
    // Start 버튼
    public Button btn_Start;


    public Text txt_RoomName;
    public Text txt_PlayerNum;

    void Start()
    {
        btn_Start.interactable = false;

        roomName = PhotonNetwork.CurrentRoom.Name;
        currPlayerNum = PhotonNetwork.CurrentRoom.PlayerCount;
        maxPlayerNum = PhotonNetwork.CurrentRoom.MaxPlayers;

        // 방이름 UI text
        txt_RoomName.text = roomName;
    }

    
    void Update()
    {
        // 참가 인원 / 참가 최대 인원 UI text 업데이트
        txt_PlayerNum.text = $"{currPlayerNum}/{maxPlayerNum}";

        // 현재 참가 인원이 4명이면  Start 버튼  interatable 활성화
        if (currPlayerNum == 4)
        {
            btn_Start.interactable = true;
        }
    }
}
