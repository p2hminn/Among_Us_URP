using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JM_Ghost : MonoBehaviourPun
{
    Color color;
    SpriteRenderer sr;
    // 플레이어 이동속도
    public float playerSpeed = 3;

    Material mat;


    // Start is called before the first frame update
    void Start()
    {
        // 스프라이트 렌더러                                                                                                                                                                                                                                    
        sr = GetComponent<SpriteRenderer>();
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", color);

        //// 닉네임 설정
        //photonView.Owner.NickName = PhotonNetwork.NickName;

        //// 게임 매니저 플레이어리스트에 자기 포톤뷰 올리기
        //JM_GameManager.instance.playerList.Add(photonView);

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 인풋이 있을 경우 
        if (h != 0f || v != 0f)
        
            // 이동함수 실행
            Move(h, v);
    }

    public void SetColor(Color settingColor)
    {
        color = settingColor;
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = color.a;
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

    void ChangeDir(Vector3 dir)
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
}
