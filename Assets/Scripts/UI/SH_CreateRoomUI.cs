using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Linq;

public class SH_CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("[맵 이름 Images]")]
    [SerializeField]
    private List<Image> mapImgs;
    [Header("[크루 Images]")]
    [SerializeField]
    private List<Image> crewImgs;
    [Header("[임포스터 수 버튼]")]
    [SerializeField]
    private List<Button> imposterCountButtons;
    [Header("[최대 플레이어 수 버튼]")]
    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    // 새로 생성하는 방의 데이터
    private CreateRoomData roomData;
    // 닉네임 가져오기
    public SH_LobbyManager lobbyManager;
    

    void Start()
    {
        // 방 이름 6자리 랜덤 설정
        string roomName = RandomString(6);

        // 방 데이터 초기화
        roomData = new CreateRoomData() { name = roomName, imposterCount = 1, maxPlayerCount = 10 };
    }

    // 랜덤 string 출력 함수
    private System.Random random = new System.Random();
    public string RandomString(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }


    // 방 생성 옵션 선택 버튼
    public void OnClickMapImgs(int count)
    {
        roomData.mapName = mapImgs[count].name;

        for (int i=0; i < mapImgs.Count; i++)
        {
            if (i == count)
            {
                mapImgs[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                mapImgs[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
    public void OnClickImposterNum(int count)
    {
        roomData.imposterCount = count;

        for (int i=0; i < imposterCountButtons.Count; i++)
        {
            if (i == count-1)
            {
                imposterCountButtons[i].image.color = new Color(1, 1, 1, 1);
                
            }
            else
            {
                imposterCountButtons[i].image.color = new Color(1, 1, 1, 0);
            }
        }
        UpdateImposterImgs();
    }
    // 하얀색 : MainMenu_Crew_Mat 
    // 빨간색 : MainMenu_Imposter_Mat 
    private void UpdateImposterImgs()
    {
        int imposterCount = roomData.imposterCount;
        int maxCount = roomData.maxPlayerCount;

        // 초기화
        for (int i=0; i < crewImgs.Count; i++)
        {
            // crewImgs[i].color = new Color(1, 1, 1, 1);
            crewImgs[i].material = Resources.Load<Material>("Materials/MainMenu_Crew_Mat");
        }

        // 임포스터 수만큼 색상 랜덤 변경
        while (imposterCount > 0)
        {
            // 몇번째 이미지를 빨강으로 만들것인지 랜덤한 숫자 뽑기
            int n = Random.Range(0, maxCount);

            for (int i=0; i < maxCount; i++)
            {
                if (i == n && crewImgs[i].material != Resources.Load<Material>("Materials/MainMenu_Imposter_Mat") && crewImgs[i].gameObject.activeSelf)
                {
                    // 재민 추가코드
                    crewImgs[i].material = Resources.Load<Material>("Materials/MainMenu_Imposter_Mat");
                    //crewImgs[i].color = new Color(0, 0, 0, 1);
                    imposterCount--;
                }
            }
        }
    }
    private void UpdateCrewImgs()
    {
        int max = roomData.maxPlayerCount;

        for (int i = 0; i < crewImgs.Count; i++)
        {
            if (i < max)
            {
                crewImgs[i].gameObject.SetActive(true);
            }
            else
            {
                crewImgs[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            // 최대 플레이어 수로 선정된 버튼 이미지만 alpha값 = 1
            if (i == count - 4)
            {
                maxPlayerCountButtons[i].image.color = new Color(1, 1, 1, 1);
                
            }
            else
            {
                maxPlayerCountButtons[i].image.color = new Color(1, 1, 1, 0);
            }
        }

        // 크루 이미지, 임포스터 이미지 업데이트
        UpdateCrewImgs();
        UpdateImposterImgs();
    }


    // 확인 버튼 -> 방 생성
    public void OnClickToCreateRoom()
    {
        // 방 옵션 세팅
        RoomOptions roomOptions = new RoomOptions();

        // 최대 인원
        roomOptions.MaxPlayers = (byte) roomData.maxPlayerCount;
        // 룸 목록에 보이는지 여부
        roomOptions.IsVisible = true;
        // 방 만들기
        PhotonNetwork.CreateRoom(roomData.name, roomOptions, TypedLobby.Default);
    }

    // 방 생성 성공할 경우
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    // 방 생성 실패할 경우
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }
    // 방 입장 요청 (생성자는 자동 입장)
    public void JoinRoom()
    {
        PhotonNetwork.NickName = lobbyManager.nickName;
        PhotonNetwork.JoinRoom(roomData.name);
    }
    // 방 입장 성공할 경우 대기실로 씬 전환
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("JM_WaitRoomScene");
    }
}

// 새로 생성되는 방의 데이터 저장
public class CreateRoomData
{
    // 룸 이름
    public string name;
    // 맵 이름
    public string mapName;
    // 임포스터 수
    public int imposterCount;
    // 최대 플레이어 수
    public int maxPlayerCount;
}


