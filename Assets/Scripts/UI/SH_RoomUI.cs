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

        if (isStart)
        {
            JM_GameIntro();
        }
        if (isSelectionUI)
        {
            JM_ShowPlayerRole();
        }
        if (isGameScene)
        {
            JM_GameEnable();
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
        // UI 보이게할 카메라 활성화
        cam.gameObject.SetActive(true);
        // 해당 리스트 내의 모든 오브젝트들 비활성화시키기
        for (int i = 0; i < toOff.Count; i++)
        {
            toOff[i].SetActive(false);
        }

        /*
        if (isStart && JM_PlayerMove.instance.introStart)
        {
            // Start 버튼 눌린 경우 게임 인트로 시작
            StartCoroutine("GameIntro");
            JM_PlayerMove.instance.introStart = false;
        }
        */
    }

    float currentTime = 0;

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
            isGameScene = true;
            isSelectionUI = false;
            // 임포스터라면 임포스터 꺼주고 크루라면 크루 꺼줌
            if (isLocalImposter)
                imposters.SetActive(false);
            else if (!isLocalImposter)
                crews.SetActive(false);
            cam.gameObject.SetActive(true);

            currentTime = 0;
        }
    }
    void JM_GameEnable()
    {
        gameMap.SetActive(true);
    }


    // GameIntro 코루틴
    //IEnumerator GameIntro()
    //{
    //    //yield return new WaitForSeconds(2);
    //    float currTime = 0;
    //    float delayTime = 5000;
    //    shhh.SetActive(true);
    //    while (currTime < delayTime)
    //    {
    //        currTime += introSpeed * Time.deltaTime;
    //        print($"currTime : {currTime}");
    //    }
    //    shhh.SetActive(false);

    //    // 크루일 경우
    //    if (!JM_PlayerMove.instance.isImposter)
    //    {
    //        float a = 0;
    //        crews.SetActive(true);
    //        CanvasGroup crewsAlpha = crews.GetComponent<CanvasGroup>();
    //        // 만약에 로컬이라면 player 오브젝트 활성화하기
    //        if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
    //        while (a < 1)
    //        {
    //            print($"crewsAlpha : {a}");
    //            a += introSpeed * Time.deltaTime;
    //            crewsAlpha.alpha = a;
    //            yield return null;
    //        }
    //        crewsAlpha.alpha = 1;
    //        yield return new WaitForSeconds(2);
    //        crews.SetActive(false);
    //    }

    //    // 임포스터일 경우
    //    else
    //    {
    //        float a = 0;
    //        imposters.SetActive(true);
    //        CanvasGroup impostersAlpha = imposters.GetComponent<CanvasGroup>();
    //        if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
    //        while (a < 1)
    //        {
    //            print($"impostersAlpha : {a}");
    //            a += introSpeed * Time.deltaTime;
    //            impostersAlpha.alpha = a;
    //            yield return null;
    //        }
    //        impostersAlpha.alpha = 1;
    //        yield return new WaitForSeconds(2);
    //        imposters.SetActive(false);
    //    }

    //    yield return new WaitForSeconds(2);


    //}
}