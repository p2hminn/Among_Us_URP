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

    // 입력받은 방이름으로 방 입장 요청
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    // 방 입장 성공할 경우
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("JM_WaitRoomScene");
    }
    // 방 입장 실패할 경우
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        // Private UI 비활성화하고 JoinRoomFailed UI 활성화
        privateUI.SetActive(false);
        joinRoomFailedUI.SetActive(true);
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}
