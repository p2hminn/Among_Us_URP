using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JM_PlayerMove : MonoBehaviourPun
{
    public GameObject light;

    // 플레이어 이동속도
    public float playerSpeed = 3;
    float originSpeed;

    // Rigidbody2D
    public Rigidbody2D rb;

    // 움직이는지 여부
    bool isMoving;
    // 임포스터 여부
    public bool isImposter;
    // 게임 인트로 UI 시작 여부
    public bool introStart = false;

    // 애니메이터
    public Animator anim;

    // 임포스터코드 및 일반 플레이어 코드
    JM_ImposterStatus imposterCode;
    JM_PlayerStatus playerCode;

    // 카메라
    public Transform camPos;

    // 도착 위치
    Vector3 receivePos;

    // 회전되야 하는 값
    Quaternion receiveRot;

    // 닉네임 UI
    public Text nickName;

    // 플레이어 색상
    [SerializeField]

    public Color color;

    GameObject deadBody;

    // 귀신 생성공장
    public GameObject ghostGenerator;

    GameObject ghost;

    SpriteRenderer sr;

    public GameObject body;

    public SH_RoomUI roomUICode;

    // 임포스터끼리 확인 가능한 임포스터 포톤뷰의 리스트
    public List<PhotonView> imposterList;


    void Start()
    {
        // 시작할때는 시야 없음
        light.SetActive(false);

        roomUICode = SH_RoomUI.instance;

        // 게임매니저에 플레이어 들어왔다는 사실 던져줌
        JM_GameManager.instance.AddPlayer(photonView);

        // 게임 오브젝트 이름 재설정
        gameObject.name = "Crew_" + photonView.Owner.NickName;

        // 닉네임 가져와서 닉네임 지정
        nickName.text = photonView.Owner.NickName;
        
        // 스프라이트 렌더러                                                                                                                                                                                                                                    
        sr = GetComponent<SpriteRenderer>();

        // 마스터 클라이언트만 색깔지정 (컬러매니저는 마스터 클라이언트만 사용)
        if (PhotonNetwork.IsMasterClient)
        {
            //print(JM_ColorManager.instance.colorList.Count);
            int randomNum = Random.Range(0, JM_ColorManager.instance.colorList.Count); // 크루의 color index (방장에게만 표시)
            photonView.RPC("RPC_SetCrewColor", RpcTarget.AllBuffered, randomNum);
        }
        
        // 내 카메라 켜주기
        if (photonView.IsMine)
        {
            //camPos를 활성화한다
            camPos.gameObject.SetActive(true);

            // 시작할때 다른 얘들 말고 나만 스폰애니 ㄱ하고 그 애니 다른 얘들한테 공유
            Spawn();
            anim.SetTrigger("Spawn");

            

            // 미니맵에 현위치
            //JM_PlayerPosManager.instance.player = gameObject;
        } 
        
        
        // 최초 속도 저장
        originSpeed = 3;

        // rb = GetComponent<Rigidbody2D>();
        imposterCode = GetComponent<JM_ImposterStatus>();
        playerCode = GetComponent<JM_PlayerStatus>();

        // waitroom에 있는동안은 무조건 다들 플레이어
        // 게임매니저로부터 isWaitRoom 변수 받아서 isWaitRoom일 경우 다들 플레이어 상태
    }

    
    
    bool isOnce = true;
    void Update()
    {
        // 만약 내 것이 아니라면 함수를 나가겠다
        if (!photonView.IsMine) return;

        // gameRoom 안에 있지 않을 경우 
        if (!JM_GameManager.instance.isGameRoom)
        {
            // 크루도 아니고 
            playerCode.enabled = false;
            // 임포스터도 아님
            imposterCode.enabled = false;
        }

        // gameRoom 안에 있을 경우
        if (JM_GameManager.instance.isGameRoom && isOnce) 
        {
            // 게임 매니저로부터 임포스터인지 크루인지 지정받아서 어떤 코드를 활성화할건지 결정
            if (isImposter)
            {
                imposterCode.enabled = true;
                playerCode.enabled = false;
                GetComponent<JM_ImposterStatus>().enabled = true;

                // 미니맵에 정보 공유
                JM_ImposterMapManager.instance.player = gameObject;
                JM_ImposterMapManager.instance.playerImgColor = color;

                nickName.color = Color.red;

                // 색상을 서로 볼 수 있게
                for (int i = 0; i < JM_GameManager.instance.imposterIndexList.Count; i++)
                {
                    GameObject imposterMate = JM_GameManager.instance.playerList[JM_GameManager.instance.imposterIndexList[i]].gameObject;
                    imposterMate.GetComponent<JM_PlayerMove>().nickName.color = Color.red;
                }

                //print("빨간색 지정 완료");
            }
            else
            {
                imposterCode.enabled = false;
                playerCode.enabled = true;
                GetComponent<JM_PlayerStatus>().enabled = true;
                // 미니맵에 정보 공유
                JM_CrewMapManager.instance.player = gameObject;
                JM_CrewMapManager.instance.playerImgColor = color;
            }
            isOnce = false;
            light.SetActive(true);
        }


       
        // 채팅 중에 마우스 커서 UI 위에 있을 경우 플레이어 안 움직이게 하기
        if (SH_RoomUI.instance.isChat && EventSystem.current.IsPointerOverGameObject()) { }
        else
        {
            // 이동 인풋 받기
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 이동 인풋이 있을 경우 
            if (h != 0f || v != 0f)
            {
                // 이동함수 실행
                Move(h, v);
                // 이동 중 
                isMoving = true;
            }
            else
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }
            SetBool(isMoving);
        }
    }  

    // 스폰
    void Spawn()
    {
        photonView.RPC("RPC_Spawn", RpcTarget.Others);
    }

    // RPC 스폰
    [PunRPC]
    void RPC_Spawn()
    {
        anim.SetTrigger("Spawn");
    }

    // 이동 함수
    private void Move(float h, float v)
    {

        // 방향 지정
        Vector3 playerDir = h * Vector3.right + v * Vector3.up;
        playerDir.Normalize();

        
        // 이동방향으로 향하도록 뒤집히게 설정
        // 하는 함수로 뺀 후 포톤에 동기화

        ChangeDir(playerDir);

        // 이동
        // transform.position += playerDir * 3 * Time.deltaTime;
        rb.velocity = playerDir * playerSpeed;
        //rb.MovePosition(p
        //layerDir);
       

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기
        if (stream.IsWriting) // isMine == trueS
        {
            // position, rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        // 데이터 받기
        if (stream.IsReading) // isMine == false
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();

        }
    }

    // 방향 바꿀때 애니메이션 좌우 플립
    void ChangeDir(Vector3 dir)
    {
        photonView.RPC("RPC_ChangeDir", RpcTarget.All, dir);
    }

    // 이동하는 동안 이동 애니메이션 지정
    void SetBool(bool bl)
    {
        photonView.RPC("RPC_SetBool", RpcTarget.All, bl);
    }

    
    [PunRPC]
    public void RPC_SetBool(bool bl)
    {
        anim.SetBool("IsMoving", bl);
    }

    [PunRPC]
    public void RPC_ChangeDir(Vector3 dir)
    {
        if (dir.x > 0f)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;
        }
        //transform.localScale = new Vector3(-1f, 1f, 1f);

        else if (dir.x < 0f)
        {
            body.GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    [PunRPC]
    void RPC_SetCrewColor(int colorIndex)
    {
        // 머티리얼 받아서
        Material mat = body.GetComponent<SpriteRenderer>().material;
        // 플레이어 컬러에 컬러리스트의 색상값을 지정해주고
        Color settingColor = JM_ColorManager.instance.colorList[colorIndex];

        // 색상의 rgba 값을 구해서
        float r = settingColor.r;
        float g = settingColor.g;
        float b = settingColor.b;
        float a = settingColor.a;

        // 해당 플레이어에게 저장
        photonView.RPC("RPC_SaveColor", RpcTarget.AllBuffered, r, g, b, a);

        // 머티리얼에 플레이어 컬러를 지정
        body.GetComponent<SpriteRenderer>().material.SetColor("_PlayerColor", settingColor);
        //mat.SetColor("_PlayerColor", color);

        // 컬러매니저의 컬러 리스트를 업데이트
        JM_ColorManager.instance.UpdateColorInfo(colorIndex);
    }

    [PunRPC]
    void RPC_SaveColor(float r, float g, float b, float a)
    {
        color = new Color(r, g, b, a);
        if (photonView.IsMine) JM_ColorManager.instance.localColor = color;
    }

    // 죽음
    public void Dead(float crewR, float crewG, float crewB, float crewA, 
        float imposterR, float imposterG, float imposterB, float imposterA)
    {
        // GameManager 에서 player 수 -1
        JM_GameManager.instance.crewNum--;

        // 크루 모두 죽었니?
        if (JM_GameManager.instance.crewNum == 0)
        {
            JM_GameManager.instance.FindYourEnd(false);  // 크루 Loose, 임포스터 Win
        }

        photonView.RPC("RPC_Dead", RpcTarget.All, crewR, crewG, crewB, crewA, 
            imposterR, imposterG, imposterB, imposterA );
    }

    public string myNickName;
    [PunRPC]
    void RPC_Dead(float crewR, float crewG, float crewB, float crewA, 
        float imposterR, float imposterG, float imposterB, float imposterA)
    {
        // 오브젝트 태그 바꿔주기
        gameObject.tag = "Ghost";

        if (photonView.IsMine)
        {
            deadBody = PhotonNetwork.Instantiate("DeadBody", transform.position, Quaternion.identity);  // 모든 화면에서 생성
            JM_DeadBody deadBodyCode = deadBody.GetComponent<JM_DeadBody>();
            deadBodyCode.SetColor(color);

            //ghost = PhotonNetwork.Instantiate("Ghost", transform.position, Quaternion.identity);  // 모든 화면에서 생성 후 자기 자신이 아닐때는 ghost 끔
            //JM_Ghost ghostCode = ghost.GetComponent<JM_Ghost>();
            //ghostCode.SetColor(color);

            // crewUI 에서 죽는 UI 재생
            JM_CrewUI.instance.Die(crewR, crewG, crewB, crewA, 
                imposterR, imposterG, imposterB, imposterG);

            ToGhost();  // 플레이어 고스트로 변신하는 함수 호출 (죽는 사람 포톤뷰 넘기기)
        }
        else
        {
            // 리모트 플레이어는 비활성화
            gameObject.SetActive(false);
        }
    }


    [PunRPC]
    void RPC_SetImposter()
    {                                                                                                                                                                                               
        isImposter = true;
        // 플레이어가 로컬일 때 SH_RoomUI에 임포스터 여부 알려줌
        //if (photonView.IsMine) 
            SH_RoomUI.instance.isLocalImposter = isImposter;

        print("I am Imposter");
        print("isImposter : " + isImposter + " ui : " + SH_RoomUI.instance.isLocalImposter);

        introStart = true;
        // SH_RoomUI.instance.StartCoroutine("GameIntro");
        //JM_GameManager.instance.isGameRoom = true;
    }

    [PunRPC]
    void RPC_SetCrew()
    {
        isImposter = false;
        //if (photonView.IsMine) 
        SH_RoomUI.instance.isLocalImposter = isImposter;

        print("I am Crew");
        print("isImposter : " + isImposter + " ui : " + SH_RoomUI.instance.isLocalImposter);

        introStart = true;
        // SH_RoomUI.instance.StartCoroutine("GameIntro");
        //JM_GameManager.instance.isGameRoom = true;
    }

    // 게임 시작할때 각자 위치값 받기
    [PunRPC]
    void RPC_SetIndividualPos(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void SetIndividualPos(float x, float y, float z)
    {
        photonView.RPC("RPC_SetIndividualPos", RpcTarget.All, x, y, z);
    }

    
    // 로컬 플레이어의 ViewID 저장
    [PunRPC]
    void RPC_SendReportPlayer(int id)
    {
        SH_VoteManager.instance.reportViewID = id;
    }

    // 투표 완료했을 경우
    [PunRPC]
    public void RPC_SendVoted(int localpanelIdx)
    {
        // 투표 완료 표시 모두에게~
        print("받은 panelIdx : " + localpanelIdx);
        GameObject panel = SH_RoomUI.instance.trPanels.gameObject;
        panel.transform.GetChild(localpanelIdx).GetChild(3).gameObject.SetActive(true);  // Voted Img
    }

    // 투표 결과 보내기
    [PunRPC]
    public void RPC_SendVoteResult(int idx)
    {
        SH_VoteManager.instance.voteResult[idx] += 1;
        SH_VoteManager.instance.voteCompleteNum++;  
    }


    // 플레이어 고스트로 변신! (임포스터, 크루 모두 접근하기 위해  PlayerMove 코드에 작성)
    public Sprite ghostSprite;
    public RuntimeAnimatorController ghostController;
    public void ToGhost()
    {
        

        // 리모트 플레이어 => 비활성화,  로컬 플레이어 => Ghost
        //if (!photonView.IsMine)
        //{
        //    gameObject.SetActive(false);
        //    return;
        //}

        #region 상태 업데이트
        //if (isImposter)
        //{
        //    JM_ImposterStatus.State stateOfImposter = GetComponent<JM_ImposterStatus>().state;
        //    stateOfImposter = JM_ImposterStatus.State.ghost;
        //    print("임포스터 상태 업데이트 : " + GetComponent<JM_ImposterStatus>().state);
        //}
        //else
        //{
        //    JM_PlayerStatus.State stateOfImposter = GetComponent<JM_PlayerStatus>().state;
        //    stateOfImposter = JM_PlayerStatus.State.ghost;
        //    print("플레이어 상태 업데이트 : " + GetComponent<JM_PlayerStatus>().state);
        //}
        #endregion

        // 비활성화 및 활성화
        body.GetComponent<BoxCollider2D>().enabled = false;
        //Destroy(GetComponent<Rigidbody2D>());
        if (isImposter) GetComponent<JM_PlayerStatus>().enabled = false;
        else GetComponent<JM_ImposterStatus>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<JM_Ghost>().enabled = true;
        // Animator 변경
        body.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)ghostController;
        // Sprite  변경
        body.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        // 유령 색상 변경
        GetComponent<JM_Ghost>().SetColor(color);
        GetComponent<JM_PlayerMove>().enabled = false;

    }
    //[PunRPC]
    //public void Disappear()
    //{
    //    gameObject.SetActive(false);
    //}
}
