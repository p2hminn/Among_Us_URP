using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Reflection;

public class JM_GameManager : MonoBehaviourPun
{
    // 싱글톤
    public static JM_GameManager instance;

    // 현재 waitRoom 인지 gameRoom 인지 판단
    public bool isGameRoom;

    // 플레이어 포톤뷰 저장 리스트
    public List<PhotonView> playerList = new List<PhotonView>();

    // 플레이어들의 인덱스를 저장할 리스트
    public List<int> playerIndexList = new List<int>();

    // 초기 스폰위치 리스트
    public List<Transform> spawnPosList = new List<Transform>();

    // 게임씬 스폰위치 기준
    public Transform gameStartOrigin;

    // 로컬 플레이어 photonView
    public PhotonView localPv;

    int randomNum;

    // playerList의 게임오브젝트가 임포스터인지 여부 리스트
    public List<bool> isImposterList = new List<bool>();

    // playerList의 게임오브젝트 컬러 리스트
    public List<Color> colorList = new List<Color>();

    public List<PhotonView> playerList2 = new List<PhotonView>();
    public List<bool> isImposterList2 = new List<bool>();
    public List<Color> colorList2 = new List<Color>();

    // 로컬 플레이어 임포스터 여부
    public bool isLocalImposter;

    // 임포스터 수
    public int imposterNum;

    // 크루 수
    public int crewNum;

    // 죽은 시체의 포톤뷰가 들어갈 리스트
    public List<PhotonView> deadBodylist = new List<PhotonView>();

    // 맵 내부 콜라이더들
    public GameObject map_Interior;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.SerializationRate = 60;

        PhotonNetwork.SendRate = 60;

        int randomNum = Random.Range(0, 3);

        // 방 인원 수 text 업데이트
        SH_RoomUI.instance.PlayerNumUpdate();

        GameObject crew = PhotonNetwork.Instantiate("Crew2_New", spawnPosList[randomNum].position, Quaternion.identity);
        // 로컬 플레이어의 photonView 저장
        localPv = crew.GetComponent<PhotonView>();
    }
    // 리포트 버튼 누르면 로컬 플레이어의 ViewID 저장하도록 뿌리기
    public void SendReportPlayer()
    {
        localPv.RPC("RPC_SendReportPlayer", RpcTarget.All, localPv.ViewID);
    }

    bool isOnce;
    bool isOnce1;
    bool isOnce2;
    bool isOnce3;

    //public Slider slider;
    //[PunRPC]
    //public void SliderUp()
    //{
    //    slider.value = 1;
    //}
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1) && PhotonNetwork.IsMasterClient)
        //{
        //    photonView.RPC("SliderUp", RpcTarget.All);
        //}

        if (PhotonNetwork.IsMasterClient && isGameRoom)
        {
            print("imposterNum : " + imposterNum);
            print("crewNum : " + crewNum);
        }
        // 방장이 Start버튼 누른 경우 playerList photonView의 gameObject 비활성화 (한번만 실행할 것)
        if (SH_RoomUI.instance.isStart && !isOnce)
        {
            // 방장이 관리 : 현재 방의 임포스터, 크루 수 저장
            if (PhotonNetwork.IsMasterClient)
            {
                imposterNum = (int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"];
                crewNum = PhotonNetwork.CurrentRoom.PlayerCount - imposterNum;
            }

            // imposter 수 매개변수로 넣어서 imposter 지정 로직 시작
            SetGameScene((int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"]);
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].gameObject.SetActive(false);
            }
            isOnce = true;
        }

        // 게임씬이 된 경우 다시 플레이어들 활성화시키기
        if (SH_RoomUI.instance.isGameScene && !isOnce1)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].gameObject.SetActive(true);
            }
            isGameRoom = true;
            isOnce1 = true;
        }

        // 게임 시작되면 플레이어 포톤뷰 정렬
        if (isGameRoom && !isOnce2)
        {
            playerList.Sort((photon1, photon2) => photon1.ViewID.CompareTo(photon2.ViewID));
            playerList2.Sort((photon1, photon2) => photon1.ViewID.CompareTo(photon2.ViewID));
            isOnce2 = true;

            // 포톤뷰의 임포스터 여부 저장
            for (int i = 0; i < playerList.Count; i++)
            {
                isImposterList.Add(playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter);
                isImposterList2.Add(playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter);
            }

            // 포톤뷰 컬러 저장
            for (int i = 0; i < playerList.Count; i++)
            {
                colorList.Add(playerList[i].gameObject.GetComponent<JM_PlayerMove>().color);
                colorList2.Add(playerList[i].gameObject.GetComponent<JM_PlayerMove>().color);
            }

            // 로컬 플레이어 임포스터인지 아닌지 저장
            isLocalImposter = localPv.gameObject.GetComponent<JM_PlayerMove>().isImposter;

            isOnce2 = true;
        }

        //if (SH_VoteManager.instance.p)
        //{
        //    print("죽었니? 2 : " + SH_VoteManager.instance.p.gameObject.activeSelf);
        //    print("현재 실행 함수2_GameManager : " + MethodBase.GetCurrentMethod().Name);
        //}


        //  Test
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            photonView.RPC("FindYourEnd", RpcTarget.All, true);  // 크루가 이기고 임포스터가 진 경우
            print("FindYourEnd");
            //photonView.RPC("FindYourEnd", RpcTarget.All, false) ;  // 크루가 지고 임포스터가 이긴 경우
        }
    }


    [PunRPC]
    public void RPC_SetPlayerPos()
    {
        for (int i = 0; i < playerList.Count; i++)
        {

        }
    }

    // 게임시작 위치들
    public Vector3[] startPos;

    // 마스터 클라이언트일때만 위치지정 ㄱㄱ
    public void SetStartPos()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        startPos = new Vector3[playerList.Count];
        float angle = 360 / playerList.Count;
        for (int i = 0; i < playerList.Count; i++)
        {
            startPos[i] = gameStartOrigin.position + transform.up * 2.5f;
            playerList[i].gameObject.transform.position = startPos[i];
            transform.Rotate(0, 0, angle);

            playerList[i].gameObject.GetComponent<JM_PlayerMove>().SetIndividualPos(startPos[i].x, startPos[i].y, startPos[i].z);
        }
    }

    // 지정한 위치값을 해당 얘들한테 각자 지정



    // 플레이어 생성될 때 플레이어 리스트에 플레이어 포톤 뷰 저장 (PlayerMove.cs의 Start)
    public void AddPlayer(PhotonView pv)
    {
        playerList.Add(pv);
        playerList2.Add(pv);
        playerIndexList.Add(playerIndexList.Count);
    }

    public List<int> imposterIndexList = new List<int>();

    // 임포스터 idx 선정
    public void SetGameScene(int imposterAmt)
    {
        print("임포스터 수 : " + imposterAmt);
        //isGameRoom = true;
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            // 임포스터 수만큼의 for 문을 돌려서
            for (int i = 0; i < imposterAmt; i++)
            {
                // 플레이어 최대 숫자(현재 방에 있는 최대 인원)와 0 사이에서 랜덤 숫자 생성
                int randomNum = Random.Range(0, playerIndexList.Count);
                //int randomNum = 0;

                // 랜덤숫자를 임포스터 인덱스 리스트에 저장
                imposterIndexList.Add(playerIndexList[randomNum]);
                // 랜덤숫자를 플레이어 인덱스 리스트에서 삭제
                playerIndexList.RemoveAt(randomNum);
            }
            ChooseImposter(imposterIndexList);
            ChooseCrew();
        }
        */

        for (int i = 0; i < imposterAmt; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int randomNum = Random.Range(0, playerIndexList.Count);
                //print("master : " + randomNum);
                photonView.RPC("RPC_ShareRandomNum", RpcTarget.All, playerIndexList[randomNum]);
                playerIndexList.RemoveAt(randomNum);
            }

        }
        if (PhotonNetwork.IsMasterClient)
        {
            ChooseImposter(imposterIndexList);
            ChooseCrew();
        }
    }

    [PunRPC]
    void RPC_ShareRandomNum(int i)
    {
        //print("others : " + randomNum);
        imposterIndexList.Add(i);
    }

    // imposter 인덱스를 담은 리스트를 받아 imposter 지정
    void ChooseImposter(List<int> imposterIndexList)
    {
        /*
        for (int i = 0; i < imposterIndexList.Count; i++)
        {
            if (playerList)
            playerList[imposterIndexList[i]].RPC("RPC_SetImposter", playerList[i].Owner);
        }
        */


        for (int i = 0; i < playerList.Count; i++)
        {
            // 임포스터의 인덱스가 맞다면
            for (int j = 0; j < imposterIndexList.Count; j++)
            {
                if (i == imposterIndexList[j])
                {
                    // RPC 함수로 해당 인덱스 플레이어는 임포스터 할당
                    playerList[i].RPC("RPC_SetImposter", RpcTarget.All);


                    print("임포스터 인덱스 : " + i);
                }
            }
        }
    }
    void ChooseCrew()
    {
        for (int i = 0; i < playerIndexList.Count; i++)
        {
            playerList[playerIndexList[i]].RPC("RPC_SetCrew", playerList[playerIndexList[i]].Owner);
        }
    }




    // 로컬 플레이어에 맞는 GameOverUI 제공하는 함수 => 게임 매니저 포톤뷰로 FindYourEnd RPC로 호출하면됌
    
    [PunRPC]
    public void FindYourEnd(bool isCrewWin)  //  파라미터로 크루가 이겼는지 여부를 넣어주어야 함
    {
        // 로컬 플레이어의 임포스터/크루 여부 확인
        // 로컬 = 임포스터
        if (isLocalImposter)
        {
            SH_GameOverUI.instance.Impostor(isCrewWin);
        }
        // 로컬 = 크루
        else
        {
            SH_GameOverUI.instance.Crew(isCrewWin);
        }
    }

    // 방장이 크루 수 업데이트
    public void CrewDead()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        crewNum--;
        // 모두에게 뿌리기
        photonView.RPC("RPC_CrewUpdate", RpcTarget.All, crewNum);
        // 크루 모두 죽었니?
        if (crewNum <= 0)
        {
            print("크루 모두 죽음");
            photonView.RPC("FindYourEnd", RpcTarget.All, false);  // 크루 Loose, 임포스터 Win
        }
    }
    // 임포스터 수 업데이트
    public void ImpoDead()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        imposterNum--;
        // 모두에게 뿌리기
        photonView.RPC("RPC_CrewUpdate", RpcTarget.All, imposterNum);
        if (imposterNum == 0)
        {
            print("임포스터 모두 죽음");
            photonView.RPC("FindYourEnd", RpcTarget.All, true);
        }
    }
    [PunRPC]
    void RPC_CrewUpdate(int n)
    {
        crewNum = n;
    }
    [PunRPC]
    void RPC_ImpoUpdate(int n)
    {
        imposterNum = n;
    }
    // 건물 내부 콜라이더 다 꺼주기
    public void DisableInterior()
    {
        map_Interior.SetActive(false);
    }


}