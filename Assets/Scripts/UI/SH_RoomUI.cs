using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEngine.Rendering.Universal;

public class SH_RoomUI : MonoBehaviourPunCallbacks
{
    // singleton
    public static SH_RoomUI instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject light;

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
    // GameIntro UI 카메라 
    public Camera cam;
    // GameIntro UI
    public GameObject shhh;
    public GameObject crews;
    public GameObject imposters;
    public float introSpeed;
    bool isSelectionUI;
    public bool isGameScene;
    public GameObject gameMap;

    // 로컬 플레이어가 임포스터인지 아닌지 여부
    public bool isLocalImposter;

    // 로컬 임포스터UI 코드 및 크루UI 코드
    JM_ImposterUI imposterUICode;
    JM_CrewUI crewUICode;

    public GameObject imposterGameUI;
    public GameObject crewGameUI;
    public GameObject reportUI;
    public GameObject diedCrew;
    public GameObject voteUI;
    public GameObject btnEmergency;
    public Button btnCrewUse;
    public Transform trPanels;

    // 시체 색깔
    public Color dieColor;

    public bool isEmergency;
    public bool isChat;

    void Start()
    {
        // 방이름 UI text
        txt_RoomName.text = PhotonNetwork.CurrentRoom.Name;

        // 방장일 경우에만 Start 버튼 활성화
        if (PhotonNetwork.IsMasterClient)
        {
            btn_Start.gameObject.SetActive(true);
        }

        // 크루UI 임포스터UI 시작할때는 둘다 꺼져있는 상태 추후에 게임으로 넘어갈때 상태 판단해서 킨다
        imposterUICode = GetComponent<JM_ImposterUI>();
        crewUICode = GetComponent<JM_CrewUI>();
        imposterUICode.enabled = false;
        crewUICode.enabled = false;

        // 대기실에 있을때는 어두운거 없음
        light.GetComponent<Light2D>().intensity = 1;
    }

    bool isOnce;
    void Update()
    {
        if (isLocalImposter) print("RoomUI recognizes I am imposter");

        // 현재 참가 인원이 4명이고 방장인 경우에  Start 버튼  interactable 활성화
        if (PhotonNetwork.CurrentRoom.PlayerCount ==  1 && PhotonNetwork.IsMasterClient)
        {
            btn_Start.interactable = true;
        }

        // 게임 인트로
        if (isStart)
        {
            JM_GameIntro();
        }
        if (isSelectionUI)
        {
            JM_ShowPlayerRole();
        }
        if (isGameScene && !isOnce)
        {
            JM_GameEnable();
            isOnce = true;
        }

        //if (SH_VoteManager.instance.p)
        //{
        //    print("죽었니? 2 : " + SH_VoteManager.instance.p.gameObject.activeSelf);
        //    print("현재 실행 함수2_RoomUI : " + MethodBase.GetCurrentMethod().Name);
        //}

        // Vote UI 활성화 중인지 체크
        if (voteUI.activeSelf)
        {
            isChat = true;
        }
        else
        {
            isChat = false;
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

    public float startSeconds = 5;
    public Text txtStartCount;

    // 방장이 Start버튼 누른 경우
    public void OnClickStart()
    {
        // Start버튼 눌렸다고 RPC 날려주기
        photonView.RPC("GameIntroStart", RpcTarget.All);
    }

    [PunRPC]
    void GameIntroStart()
    {
        // 게임 시작 카운트
        txtStartCount.gameObject.SetActive(true);
        StartCoroutine("StartCount");
    }
    IEnumerator StartCount()
    {
        float currTime = 0;

        while (currTime < startSeconds)
        {
            currTime += Time.deltaTime;
            txtStartCount.text = $"{(int)startSeconds - (int)currTime}초 후 시작";
            yield return null;
        }
        txtStartCount.text = "";

        JM_GameManager.instance.SetStartPos();
        // 해당 리스트 내의 모든 오브젝트들 비활성화시키기
        for (int i = 0; i < toOff.Count; i++)
        {
            toOff[i].SetActive(false);
        }
        // UI 보이게할 카메라 활성화
        cam.gameObject.SetActive(true);
        // 게임 인트로 시작
        isStart = true;
    }


    // 게임 인트로 
    float currentTime = 0;
    // 쉿 UI
    void JM_GameIntro()
    {
        shhh.SetActive(true);
        float delayTime = 3;
        currentTime += Time.deltaTime;
        if (currentTime > delayTime)
        {
            shhh.SetActive(false);
            isSelectionUI = true;
            isStart = false;
            currentTime = 0;
        }
    }
    // 플레이어 역할 배정
    void JM_ShowPlayerRole()
    {
        currentTime += Time.deltaTime;
        float delayTime = 3;
        // 임포스터라면
        if (isLocalImposter)
        {
            // 임포스터 ui 실행
            imposters.SetActive(true);
        }
        // 크루라면
        else
        {
            // 크루 ui 실행
            crews.SetActive(true);
        }

        if (currentTime > delayTime)
        {
            #region 위치 지정
            // 플레이어 활성화 함수 게임매니저에서 호출
            // photonView.RPC("RPC_EnablePlayers", RpcTarget.All);

            // 위치도 지정


            // photonView.RPC("RPC_SetPlayerPos", RpcTarget.All);
            // photonView.RPC("RPC_Test", RpcTarget.All);
            #endregion
            isSelectionUI = false;

            // 플레이 역할 알려주는 UI 꺼줌
            if (isLocalImposter)  imposters.SetActive(false);
            else crews.SetActive(false);

            cam.gameObject.SetActive(true);
            currentTime = 0;

            // 게임 세팅 시작
            isGameScene = true;
        }
    }
    public GameObject missionStatusUI;
    // 게임 가능하게 세팅
    void JM_GameEnable()
    {
        //JM_GameManager.instance.RPC_EnablePlayers();

        // 게임 시작하면 전체 어둡게
        light.GetComponent<Light2D>().intensity = 0.5f;


        gameMap.SetActive(true);
        missionStatusUI.SetActive(true);
        if (isLocalImposter)
        {
            imposterGameUI.SetActive(true);
            imposterUICode.enabled = true;
        }
        else if (!isLocalImposter)
        {
            crewGameUI.SetActive(true);
            crewUICode.enabled = true;
        }
    }


    public GameObject reportedDeadBody;
    // 시체 발견 후 리포트 버튼 누르면 UI 활성화
    public void OnReportButton()
    {
        Report(dieColor.r, dieColor.g, dieColor.b, dieColor.a);
        
    }
    // RPC로 시체 색깔 넘기기
    public void Report(float deadR, float deadG, float deadB, float deadA)
    {
        photonView.RPC("RPC_Report", RpcTarget.All,  deadR, deadG, deadB, deadA);
    }
    [PunRPC]
    public void RPC_Report(float deadR, float deadG, float deadB, float deadA)
    {
        Color diedCrewColor = new Color(deadR, deadG, deadB, deadA);

        // 신고된 시체 Destroy
        Destroy(reportedDeadBody);

        StartReportUI(diedCrewColor);
    }
    // 시체 색 변환 후 리포트 UI 활성화 + 투표 시작
    void StartReportUI(Color diedCrewColor)
    {
        Material mat = diedCrew.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", diedCrewColor);
        // 리포트 UI 2초간 활성화
        StartCoroutine("ActivateReportUI");
    }
    IEnumerator ActivateReportUI()
    {
        reportUI.SetActive(true);
        yield return new WaitForSeconds(2);
        reportUI.SetActive(false);
        // 투표 시작
        SH_VoteManager.instance.PlayerPanelSetting();
    }



    // 채팅 오픈, 닫기
    public GameObject chatView;
    public bool open = false;
    public void OnClickChat() 
    {
        if (!open)
        {
            chatView.SetActive(true);
            open = true;
        }
        else
        {
            chatView.SetActive(false);
            open = false;
        }
    }

    // 긴급회의 
    public GameObject emergencyImg;
    [PunRPC]
    public void EmergencyMeeting()
    {
        StartCoroutine("ActivateEmergencyUI");
    }
    IEnumerator ActivateEmergencyUI()
    {
        emergencyImg.SetActive(true);
        yield return new WaitForSeconds(2);
        emergencyImg.SetActive(false);
        // 투표 시작
        SH_VoteManager.instance.PlayerPanelSetting();
    }
}