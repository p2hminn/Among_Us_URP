using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PlayerMove : MonoBehaviour
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

    // 임포스터코드 및 일반 플레이어 코드
    JM_ImposterStatus imposterCode;
    JM_PlayerStatus playerCode;

    void Start()
    {
        // 최초 속도 저장
        originSpeed = playerSpeed;

        // rb = GetComponent<Rigidbody2D>();
        imposterCode = GetComponent<JM_ImposterStatus>();
        playerCode = GetComponent<JM_PlayerStatus>();
        anim = GetComponent<Animator>();

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

    void Update()
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
        }

        anim.SetBool("IsMoving", isMoving);
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
        if (playerDir.x > 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (playerDir.x < 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);

        // 이동
        transform.position += playerDir * playerSpeed * Time.deltaTime;

    }

    // 충돌했을때 속도 0으로 제한 --> 안튕겨나게
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("콜라이더 작동함");
        playerSpeed = 0;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        playerSpeed = originSpeed;
        // asdfasdf
        // apiosefbna;soeigvhj;aelskfjpa0soeibnv;osdih;vasolikdhfp;asoidbflk;sjbvp9o;sdb
    }



}
