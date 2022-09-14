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

    // 닉네임
    public InputField inputNickName;
    public string nickName;

    // 버튼
    public Button btnCreateRoom;
    public Button btnFindRoom;
    public Button btnJoinRoom;


    private void Start()
    {
        // 닉네임(InputField)이 변경될 때 호출되는 함수 등록
        inputNickName.onValueChanged.AddListener(OnNickNameValueChanged);
        // 닉네임(InputField)에서 Focusing을 잃었을 때 호출되는 함수 등록

    }

    public void OnNickNameValueChanged(string s)
    {
        // 버튼 3개 모두 활성화
        btnCreateRoom.interactable = s.Length > 0;
        btnFindRoom.interactable = s.Length > 0;
        btnJoinRoom.interactable = s.Length > 0;
        // 닉네임 등록
        PhotonNetwork.NickName = inputNickName.text;
    }

    // 입력받은 방이름으로 방 입장 요청
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        print(roomName.text);
    }

    // 방 입장 성공할 경우
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("JM_WaitRoomScene");
    }
    // 방 입장 실패할 경우
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        // Private UI 비활성화하고 JoinRoomFailed UI 활성화
        privateUI.SetActive(false);
        joinRoomFailedUI.SetActive(true);
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}
