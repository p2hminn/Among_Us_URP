using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_PlayerStatus : MonoBehaviourPun
{
    Animator anim;
    // 죽을때 시체를 생성한다.
    // 게임매니저에서 생성?
    // 시체 색은 플레이어의 색
    // 일단 시체 생성하는거부터

    // 시체 생성공장
    public GameObject deadBodyGenerator;
    // 리포트 UI
    public GameObject reportUI;

    public enum State
    {
        crew,
        ghost,
    }

    public State state;
    public Color playerColor;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerColor = GetComponent<JM_PlayerMove>().color;
    }


    // 시체와 충돌 시 리포트 
    [SerializeField]
    Color deadColor;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody"))
        {
            // 로컬 플레이어만 리포트 활성화 가능
            if (photonView.IsMine)
            {
                JM_CrewUI.instance.isReportAble = true;
                SH_RoomUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
                SH_RoomUI.instance.reportedDeadBody = collision.gameObject;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        JM_CrewUI.instance.isReportAble = false;
    }



    // 플레이어 고스트로 변신!
    public Sprite ghostSprite;
    public void ToGhost()
    {
        // 오브젝트 태그 바꿔주기
        gameObject.tag = "Ghost";

        // 로컬 플레이어 -> 고스트, 리모트 플레이어 -> 비활성화
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
            return;
        }

        // 상태 업데이트
        state = State.ghost;

        // 비활성화 및 활성화
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<JM_PlayerMove>().enabled = false;
        if (GetComponent<JM_PlayerMove>().isImposter) GetComponent<JM_PlayerStatus>().enabled = false;
        else GetComponent<JM_ImposterStatus>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<JM_Ghost>().enabled = true;

        // Animator 변경
        //anim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Animator/JM_GhostAnimator", typeof(RuntimeAnimatorController)));
        anim.runtimeAnimatorController = Resources.Load("Animator/JM_GhostAnimator") as RuntimeAnimatorController;
        // Sprite 변경
        GetComponent<SpriteRenderer>().sprite = ghostSprite;
        // 유령 색상 변경
        GetComponent<JM_Ghost>().SetColor(playerColor);
    }

    

}
