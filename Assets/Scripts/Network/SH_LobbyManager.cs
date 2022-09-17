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

    // 방 정보 (Key = 방이름, Value = roomInfo)
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    // 룸 자식으로 삼을 Content(부모)
    public Transform content;


    private void Start()
    {
        // 닉네임(InputField)이 변경될 때 호출되는 함수 등록
        inputNickName.onValueChanged.AddListener(OnNickNameValueChanged);
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

    // 입력받은 방이름으로 방 입장 
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        print(roomName.text);
    }
    // 방 입장 성공할 경우
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //// 자신이 방에 들어갔을 때 방의 인원 수 업데이트
        //SH_RoomUI.instance.PlayerNumUpdate();
        // 방 씬으로  전환 
        PhotonNetwork.LoadLevel("SH_RoomScene UI");
    }
    // 방 입장 실패할 경우
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        // Private UI 비활성화하고 JoinRoomFailed UI 활성화
        privateUI.SetActive(false);
        joinRoomFailedUI.SetActive(true);
    }


    // 방 목록 : 방 생성 시 호출 (추가/삭제/수정)
    // roomList : 변동사항이 있는 방
    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        base.OnRoomListUpdate(roomList);
        // 룸 리스트 UI 전체삭제 
        DeleteRoomListUI();
        // 룸 리스트 정보 업데이트
        UpdateRoomCache(roomList);
        // 룸 리스트 UI 전체 생성
        CreateRoomListUI();
    }
    void DeleteRoomListUI()
    {
        // Transform : 자식 컴포넌트 가져옴
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }
    // roomList를 roomCache에 갱신
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //  만약에 해당 룸이 삭제된 것이라면 (RemovedFromList  : 삭제 여부)
                if (roomList[i].RemovedFromList)
                {
                    // roomCache에서 해당 정보 삭제
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            // 추가 및 수정 ( Key-Value )
            print(roomList[i]);
            roomCache[roomList[i].Name] = roomList[i];
        }
    }
    // roomCache의 value값으로 룸 아이템 생성
    public GameObject roomItemFactory;
    // 방목록 생성
    void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            // content의 자식으로 룸아이템 생성
            GameObject go = Instantiate(roomItemFactory, content);
            // 룸아이템 정보 셋팅 (방제목(0/0))
            SH_RoomItem item = go.GetComponent<SH_RoomItem>();
            item.SetInfo(info);
            // roomItem 버튼이 클릭되면 호출되는 함수 등록
            //item.onClickAction = SetRoomName;
            // 람다식으로 바로 함수 내용 넣어주기 (함수 선언후 넣어주기도 가능)
            item.onClickAction = (room) => { roomName.text = room; };


        }
    }

    
    void Update()
    {
        //statusText.text = PhotonNetwork.NetworkClientState.ToString();
        //Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
}

