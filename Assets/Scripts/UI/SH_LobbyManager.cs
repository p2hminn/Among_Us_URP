using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_LobbyManager : MonoBehaviourPunCallbacks
{
    public Text roomName;
    public Text statusText;

    public GameObject privateUI;
    public GameObject joinRoomFailedUI;

    // �г���
    public Text nickNameInput;
    public string nickName;


    // �Է¹��� ���̸����� �� ���� ��û
    public void OnClickJoinRoom()
    {
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.JoinRoom(roomName.text);
    }

    // �� ���� ������ ���
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("JM_WaitRoomScene");
    }
    // �� ���� ������ ���
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        // Private UI ��Ȱ��ȭ�ϰ� JoinRoomFailed UI Ȱ��ȭ
        privateUI.SetActive(false);
        joinRoomFailedUI.SetActive(true);
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());

        // �г��� ����
        nickName = nickNameInput.text;
    }
}