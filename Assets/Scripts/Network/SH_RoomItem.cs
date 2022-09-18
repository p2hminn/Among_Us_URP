using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class SH_RoomItem : MonoBehaviour
{
    // ��ư�� ǥ�õǴ� text
    public Text roomInfo;
    // Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� �������ִ� ����
    public System.Action<string> onClickAction;

    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        // ���ӿ�����Ʈ�� �̸��� roomName����!
        name = roomName;
        // ���̸�   
        roomInfo.text = $"�ѱ���    THE SKELD     {roomName}   (   {currPlayer}   /   {maxPlayer}   ) ";
    }
    public void SetInfo(RoomInfo info)
    {
        //string map = (string)info.CustomProperties["map"];
        SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
    }

    // ���� ���� �� �ش� ������ �̵�
    public void OnClick()
    {
        // onClickAction�� null�� �ƴ϶��(� �Լ��� ���ִٸ�)
        if (onClickAction != null)
        {
            // onClickAction ����
            onClickAction(name);
        }
    }
}
