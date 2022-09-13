using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_LobbyManager : MonoBehaviourPunCallbacks
{
    public Text roomName;
    public GameObject privateUI;
    public GameObject joinRoomFailedUI;

    public Text statusText;

    // �Է¹��� ���̸����� �� ���� ��û
    public void OnClickJoinRoom()
    {
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

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}
