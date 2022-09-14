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

    // �� ���� (Key = ���̸�, Value = roomInfo)
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    // �� �ڽ����� ���� Content(�θ�)
    public Transform content;
    


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

    // �Է¹��� ���̸����� �� ���� 
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


    // �� ��� : �� ���� �� ȣ�� (�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // roomList => PhotonNetwork.CreateRoom(roomData.name, roomOptions...?
    {
        base.OnRoomListUpdate(roomList);
        for (int i=0; i < roomList.Count; i++)
        {
            print("OnRoomListUpdate===============================");
            print(roomList[i]);
        }
        // �� ����Ʈ UI ��ü���� 
        DeleteRoomListUI();
        // �� ����Ʈ ���� ������Ʈ
        UpdateRoomCache(roomList);
        // �� ����Ʈ UI ��ü ����
        CreateRoomListUI();
    }
    void DeleteRoomListUI()
    {
        // Transform : �ڽ� ������Ʈ ������
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }
    // roomList�� roomCache�� ����
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //  ���࿡ �ش� ���� ������ ���̶�� (RemovedFromList  : ���� ����)
                if (roomList[i].RemovedFromList)
                {
                    // roomCache���� �ش� ���� ����
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            // �߰� �� ���� ( Key-Value )
            print(roomList[i]);
            roomCache[roomList[i].Name] = roomList[i];
        }
    }
    // roomCache�� value������ �� ������ ����
    public GameObject roomItemFactory;
    void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            // content�� �ڽ����� ������� ����
            GameObject go = Instantiate(roomItemFactory, content);
            // ������� ���� ���� (������(0/0))
            SH_RoomInfo item = go.GetComponent<SH_RoomInfo>();
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        }
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}
