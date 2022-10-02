using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

public class JM_GameManager : MonoBehaviourPun
{
    // 싱글톤
    public static JM_GameManager instance;

    // 현재 waitRoom 인지 gameRoom 인지 판단
    public bool isGameRoom;

    // 플레이어들을 저장할 리스트
    public List<PhotonView> playerList = new List<PhotonView>();

    // 초기 스폰위치 리스트
    public List<Transform> spawnPosList = new List<Transform>();

    // 게임씬 스폰위치 기준
    public Transform gameStartOrigin;

    // 로컬 플레이어 photonView
    public PhotonView localPv;


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
    //bool isOnce3;
    void Update()
    {
        // 방장이 Start버튼 누른 경우 playerList photonView의 gameObject 비활성화 (한번만 실행할 것)
        if (SH_RoomUI.instance.isStart && !isOnce)
        {
            // imposter 수 매개변수로 넣어서 imposter 지정 로직 시작
            SetGameScene((int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"]);

            for (int i=0; i < playerList.Count; i++)
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
            isOnce2 = true;
        }

        //if (SH_VoteManager.instance.p)
        //{
        //    print("�׾���? 2 : " + SH_VoteManager.instance.p.gameObject.activeSelf);
        //    print("���� ���� �Լ�2_GameManager : " + MethodBase.GetCurrentMethod().Name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3) && !isOnce3)
        //{
        //    isOnce3 = true;
        //    GameObject g = GameObject.Find("GameOverUI");
        //    g.GetComponent<SH_GameOVer>().Crew(true);   // 크루가  이긴 경우 & 로컬 플레이어가 크루인 경우
        //                  //Crew(false);  // 크루가 진 경우 & 로컬 플레이어가 크루인 경우
        //}
    }



    // 게임씬 활성화
    //[PunRPC]
    //public void RPC_EnablePlayers()
    //{
    //    for (int i = 0; i < playerList.Count; i++)
    //    {
    //        playerList[i].gameObject.SetActive(true);
    //    }
    //}

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
    }


    // 임포스터 idx 선정
    public void SetGameScene(int imposterAmt)
    {
        //isGameRoom = true;
        if (PhotonNetwork.IsMasterClient)
        {
            // int 로 이루어진 리스트를 만들고
            List<int> imposterIndexList = new List<int>();

            // 임포스터 수만큼의 for 문을 돌려서
            for (int i = 0; i < imposterAmt; i++)
            {
                // 플레이어 최대 숫자(현재 방에 있는 최대 인원)와 0 사이에서 랜덤 숫자 생성
                int randomNum = Random.Range(0, playerList.Count);
                //int randomNum = 0;
                // 임포스터 리스트에 랜덤숫자가 없다면
                if (!imposterIndexList.Contains(randomNum))
                {
                    // 리스트에 랜덤숫자 추가                                                        
                    imposterIndexList.Add(randomNum);
                }
                // 그렇지 않다면
                else
                {
                    // 다시 랜덤숫자 구함
                    randomNum = Random.Range(0, playerList.Count);
                }
            }
            //print("ChooseImposter");
            ChooseImposter(imposterIndexList);
        }
    }
    // imposter 인덱스를 담은 리스트를 받아 imposter 지정
    void ChooseImposter(List<int> imposterIndexList)
    {
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
                else
                {
                    playerList[i].RPC("RPC_SetCrew", RpcTarget.All);
                    //print("크루 인덱스 : " + i);
                }
            }
        }
    }
    
}