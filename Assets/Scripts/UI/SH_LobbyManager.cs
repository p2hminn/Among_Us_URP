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
    public InputField inputNickName;
    public string nickName;

    // ��ư
    public Button btnCreateRoom;
    public Button btnFindRoom;
    public Button btnJoinRoom;


    private void Start()
    {
        // �г���(InputField)�� ����� �� ȣ��Ǵ� �Լ� ���
        inputNickName.onValueChanged.AddListener(OnNickNameValueChanged);
        // �г���(InputField)���� Focusing�� �Ҿ��� �� ȣ��Ǵ� �Լ� ���

    }

    public void OnNickNameValueChanged(string s)
    {
        // ��ư 3�� ��� Ȱ��ȭ
        btnCreateRoom.interactable = s.Length > 0;
        btnFindRoom.interactable = s.Length > 0;
        btnJoinRoom.interactable = s.Length > 0;
        // �г��� ���
        PhotonNetwork.NickName = inputNickName.text;
    }

    // �Է¹��� ���̸����� �� ���� ��û
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        print(roomName.text);
    }

    // �� ���� ������ ���
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("JM_WaitRoomScene");
    }
    // �� ���� ������ ���
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        // Private UI ��Ȱ��ȭ�ϰ� JoinRoomFailed UI Ȱ��ȭ
        privateUI.SetActive(false);
        joinRoomFailedUI.SetActive(true);
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}
