using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_WaitingRoomPlayer : MonoBehaviourPun
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
    Animator anim;

    JM_PlayerStatus playerCode;

    // 카메라
    public Camera cam;


    // 도착 위치
    Vector3 receivePos;

    // 회전되야 하는 값
    Quaternion receiveRot;

    // 보간 속도
    public float lerpSpeed = 100;

    void Start()
    {
        if (photonView.IsMine) cam.gameObject.SetActive(true);
        // 최초 속도 저장
        originSpeed = playerSpeed;

        // rb = GetComponent<Rigidbody2D>();
        
        playerCode = GetComponent<JM_PlayerStatus>();
        anim = GetComponent<Animator>();

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
        photonView.RPC("RPC_SetBool", RpcTarget.All, isMoving);
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

    [PunRPC]
    public void RPC_SetBool(bool bl)
    {
        anim.SetBool("IsMoving", bl);
    }
}
