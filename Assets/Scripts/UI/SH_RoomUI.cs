using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SH_RoomUI : MonoBehaviourPunCallbacks
{
    // singleton
    public static SH_RoomUI instance;
    private void Awake()
    {
        instance = this;
    }
    // Start 버튼
    public Button btn_Start;
    // 방 이름 Text
    public Text txt_RoomName;
    // 현재 접속 인원 / 최대 접속 가능 인원 Text
    public Text txt_PlayerNum;
    // 방장 게임 Start 버튼 누름 여부
    public bool isStart = false;
    // 게임 Start 버튼 누를 시 비활성화해야 하는 오브젝트들
    [Header("GameStart 시 비활성화해야 하는 오브젝트")]
    public List<GameObject> toOff = new List<GameObject>();

    void Start()
    {
        // 방이름 UI text
        txt_RoomName.text = PhotonNetwork.CurrentRoom.Name;

        // 방장일 경우에만 Start 버튼 활성화
        if (PhotonNetwork.IsMasterClient)
        {
            btn_Start.gameObject.SetActive(true);
        }
    }

    
    void Update()
    {
        // 현재 참가 인원이 4명이고 방장인 경우에  Start 버튼  interactable 활성화
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && PhotonNetwork.IsMasterClient)
        {
            btn_Start.interactable = true;
        }

        // Start 버튼 눌린 경우 게임 인트로 시작
        if (isStart)
        {
            StartCoroutine("GameIntro");
        }
    }

    // 플레이어가 방에 들어올 때 & 나갈 때 방 인원 수 업데이트
    public override void OnPlayerEnteredRoom(Player newPlayer) => PlayerNumUpdate();
    public override void OnPlayerLeftRoom(Player otherPlayer) => PlayerNumUpdate();
    // 현재 방의 인원 수 Text 업데이트
    public void PlayerNumUpdate()
    {
        // 참가 인원 / 참가 최대 인원 UI text 업데이트
        txt_PlayerNum.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }



    // 방장이 Start버튼 누른 경우
    public void OnClickStart()
    {
        // Start버튼 눌렸다고 RPC 날려주기
        photonView.RPC("GameIntroStart", RpcTarget.All);
    }

    [PunRPC]
    void GameIntroStart()
    {
        isStart = true;
        // 해당 리스트 내의 모든 오브젝트들 비활성화시키기
        for (int i = 0; i < toOff.Count; i++)
        {
            toOff[i].SetActive(false);
        }
    }
    

    // GameIntro 코루틴
    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(2);


    }
}
