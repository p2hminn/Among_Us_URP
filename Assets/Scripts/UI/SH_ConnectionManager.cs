using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;  // 유니티용 포톤 컴포넌트
using Photon.Realtime;  // 포톤 서비스 관련 라이브러리

// 마스터 서버 접속 성공 시에만 온라인 버튼 활성화

public class SH_ConnectionManager : MonoBehaviourPunCallbacks  // 포톤 서비스가 발생시키는 콜백 감지 
{
    // 게임 버전
    private string gameVersion = "1";
    // 네트워크 정보 표시 텍스트
    public Text connectionInfoText;
    // 온라인 버튼
    public Button onlineButton;
    // 닉네임
    public Text nickName;


    private void Awake()
    {
        connectionInfoText = GameObject.Find("Text_ConnectionInfo").GetComponent<Text>();
    }
    // 게임 실행과 동시에 마스터 서버 접속 시도
    void Start()
    {
        Connect();
    }

    void Update()
    {
        // 닉네임 설정
        PhotonNetwork.NickName = nickName.text;
    }

    void Connect()
    {
        PhotonNetwork.GameVersion = gameVersion;
        //PhotonNetwork.ConnectToRegion("kr");

        // 마스터 서버 접속 
        PhotonNetwork.ConnectUsingSettings();

        // 온라인 버튼 잠시 비활성화
        onlineButton.interactable = false;
        // 접속 시도 중임 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속 중...";
    }


    // 마스터 서버 접속 성공할 경우
    public override void OnConnectedToMaster()
    {
        // 온라인 버튼 활성화
        onlineButton.interactable = true;
        // 접속 정보 표시
        connectionInfoText.text = "마스터 서버와 연결됨";
    }
    // 마스터 서버 접속 실패할 경우
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 온라인 버튼 비활성화
        onlineButton.interactable = false;
        // 접속 정보 표시
        connectionInfoText.text = "마스터 서버와 연결되지 않음\n접속 재시도 중...";

        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 로비 접속 성공시 호출
    public override void OnJoinedLobby()
    {
        //SH_ConnectionManager.connectionInfoText.text = "로비 접속 완료";
    }
}
