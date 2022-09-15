using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
    bool isImposter;

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


    public GameObject crew;

    void Start()
    {
        if (photonView.IsMine)
        {
            cam.gameObject.SetActive(true);
            int randomNum = Random.Range(0, JM_ColorManager.instance.colorList.Count);
            //photonView.RPC("CreatedPlayer", RpcTarget.AllBuffered, rn);

            photonView.RPC("RPC_SetCrewColor", RpcTarget.AllBuffered, randomNum);
            JM_ColorManager.instance.UpdateColorInfo(randomNum);
            
        }
        
        // 최초 속도 저장
        originSpeed = playerSpeed;

        // rb = GetComponent<Rigidbody2D>();
        imposterCode = GetComponent<JM_ImposterStatus>();
        playerCode = GetComponent<JM_PlayerStatus>();

        // waitroom에 있는동안은 무조건 다들 플레이어
        if (SceneManager.GetActiveScene().name == "JM_WaitRoomScene")
        {
            imposterCode.enabled = false;
        }
        else
        {
            // 랜덤숫자 지정해서 임포스터 또는 플레이어 지정
            // (나중에는 네트워크랑 연동해서 바꿀 예정)
            int randomNum = 1;//Random.Range(0, 2);
            if (randomNum > 0)
            {
                isImposter = true;
            }
            if (isImposter)
            {
                imposterCode.enabled = true;
                playerCode.enabled = false;
            }
            else
            {
                imposterCode.enabled = false;
                playerCode.enabled = true;
            }
        }       

    }
    [PunRPC]
     void RPC_SetCrewColor(int colorIndex)
    {
        Material mat = gameObject.GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.colorList[colorIndex]);

        // JM_ColorManager.instance.GetColor();

    }

    [PunRPC]
    void CreatedPlayer(int colorNum)
    {
        Material mat = gameObject.GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.colorList[colorNum]);
    }

    void Update()
    {
        // 만약 내 것이 아니라면 함수를 나가겠다
        if (!photonView.IsMine) return;

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
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();

        // 방향 지정
        Vector3 playerDir = h * Vector3.right + v * Vector3.up;
        playerDir.Normalize();

        
        // 이동방향으로 향하도록 뒤집히게 설정
        // 하는 함수로 뺀 후 포톤에 동기화

        ChangeDir(playerDir);

        // 이동
        transform.position += playerDir * playerSpeed * Time.deltaTime;

    }
    /*
    // 충돌했을때 속도 0으로 제한 --> 안튕겨나게
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("콜라이더 작동함");
        playerSpeed = 0.5f;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        playerSpeed = originSpeed;
        // asdfasdf
        // apiosefbna;soeigvhj;aelskfjpa0soeibnv;osdih;vasolikdhfp;asoidbflk;sjbvp9o;sdb
    }
    */

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

    void ChangeDir(Vector3 dir)
    {
        photonView.RPC("RPC_ChangeDir", RpcTarget.All, dir);
    }

    [PunRPC]
    public void RPC_ChangeDir(Vector3 dir)
    {
        if (dir.x > 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (dir.x < 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void SetBool(bool bl)
    {
        photonView.RPC("RPC_SetBool", RpcTarget.All, bl);
    }

    [PunRPC]
    public void RPC_SetBool(bool bl)
    {
        anim.SetBool("IsMoving", bl);
    }

}
