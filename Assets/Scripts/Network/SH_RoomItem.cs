using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class SH_RoomItem : MonoBehaviour
{
    // 버튼에 표시되는 text
    public Text roomInfo;
    // 클릭이 되었을 때 호출되는 함수를 가지고있는 변수
    public System.Action<string> onClickAction;

    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        // 게임오브젝트의 이름을 roomName으로!
        name = roomName;
        // 방이름   
        roomInfo.text = $"한국어    THE SKELD     {roomName}   (   {currPlayer}   /   {maxPlayer}   ) ";
    }
    public void SetInfo(RoomInfo info)
    {
        //string map = (string)info.CustomProperties["map"];
        SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
    }

    // 방목록 누를 때 해당 방으로 이동
    public void OnClick()
    {
        // onClickAction이 null이 아니라면(어떤 함수가 들어가있다면)
        if (onClickAction != null)
        {
            // onClickAction 실행
            onClickAction(name);
        }
    }
}
