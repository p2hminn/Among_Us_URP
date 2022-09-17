using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JM_PlayerMove : MonoBehaviourPun
{

    // 플레이어 이동속도
    public float playerSpeed = 3;
    float originSpeed;

    // Rigidbody2D
    // Rigidbody2D rb;

    // 움직이는지 여부
    bool isMoving;

    // 임포스터 여부
    public bool isImposter;

    // 애니메이터
    public Animator anim;

    // 임포스터코드 및 일반 플레이어 코드
    JM_ImposterStatus imposterCode;
    JM_PlayerStatus playerCode;

    // 카메라
    public Camera cam;


    // 도착 위치
    Vector3 receivePos;

    // 회전되야 하는 값
    Quaternion receiveRot;

    // 닉네임 UI
    public Text nickName;

    // 플레이어 색상
    [SerializeField]
    Color color;

    GameObject deadBody;

    SpriteRenderer sr;

    void Start()
    {
        // 게임매니저에 플레이어 들어왔다는 사실 던져줌
        JM_GameManager.instance.AddPlayer(photonView);

        // 닉네임 가져와서 닉네임 지정
        nickName.text = photonView.Owner.NickName;
        

        // 스프라이트 렌더러                                                                                                                                                                                                                                    
        sr = GetComponent<SpriteRenderer>();

        // 마스터 클라이언트만 색깔지정 (컬러매니저는 마스터 클라이언트만 사용)
        if (PhotonNetwork.IsMasterClient)
        {
            print(JM_ColorManager.instance.colorList.Count);
            int randomNum = Random.Range(0, JM_ColorManager.instance.colorList.Count);
            photonView.RPC("RPC_SetCrewColor", RpcTarget.AllBuffered, randomNum);
        }
        
        // 내 카메라 켜주기
        if (photonView.IsMine)
        {  
            cam.gameObject.SetActive(true);   
            // 색상정보를 받는다

            // 받은 정보를 통해서 색상값을 저장한다. 
        }
        
        // 최초 속도 저장
        originSpeed = playerSpeed;

        // rb = GetComponent<Rigidbody2D>();
        imposterCode = GetComponent<JM_ImposterStatus>();
        playerCode = GetComponent<JM_PlayerStatus>();

        // waitroom에 있는동안은 무조건 다들 플레이어
        // 게임매니저로부터 isWaitRoom 변수 받아서 isWaitRoom일 경우 다들 플레이어 상태

 
    }

    
    

    void Update()
    {
        // 만약 내 것이 아니라면 함수를 나가겠다
        if (!photonView.IsMine) return;

        // gameRoom 안에 있지 않을 경우 
        if (!JM_GameManager.instance.isGameRoom)
        {
            // 크루도 아니고 
            playerCode.enabled = true;
            // 임포스터도 아님
            imposterCode.enabled = false;
        }

        // gameRoom 안에 있을 경우
        if (JM_GameManager.instance.isGameRoom)
        {
            print("isGameRoom is correctly working");
            // 게임 매니저로부터 임포스터인지 크루인지 지정받아서 어떤 코드를 활성화할건지 결정
            if (isImposter)
            {
                imposterCode.enabled = true;
                playerCode.enabled = false;
                nickName.color = Color.red;
                print("빨간색 지정 했음");
            }
            else
            {
                imposterCode.enabled = false;
                playerCode.enabled = true;
            }
        }

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
        }
        SetBool(isMoving);
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
        transform.position += playerDir * playerSpeed * Time.deltaTime;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기
        if (stream.IsWriting) // isMine == true
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
            sr.flipX = false;
        }
        //transform.localScale = new Vector3(-1f, 1f, 1f);

        else if (dir.x < 0f)
        {
            sr.flipX = true;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    [PunRPC]
    void RPC_SetCrewColor(int colorIndex)
    {
        // 머티리얼 받아서
        Material mat = gameObject.GetComponent<SpriteRenderer>().material;
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
        mat.SetColor("_PlayerColor", color);

        // 컬러매니저의 컬러 리스트를 업데이트
        JM_ColorManager.instance.UpdateColorInfo(colorIndex);
    }

    [PunRPC]
    void RPC_SaveColor(float r, float g, float b, float a)
    {
        color = new Color(r, g, b, a);
    }

    // 죽음
    [PunRPC]
    void RPC_Dead()
    {
        if (photonView.IsMine)
        {
            deadBody = PhotonNetwork.Instantiate("DeadBody", transform.position, Quaternion.identity);
            JM_DeadBody deadBodyCode = deadBody.GetComponent<JM_DeadBody>();
            deadBodyCode.SetColor(color);
        }
    }
    [PunRPC]
    void RPC_SetImposter()
    {
        isImposter = true;
        JM_GameManager.instance.isGameRoom = true;
    }

    [PunRPC]
    void RPC_SetCrew()
    {
        isImposter = false;
        JM_GameManager.instance.isGameRoom = true;
    }
}
