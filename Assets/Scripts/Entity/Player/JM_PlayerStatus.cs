using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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
    GameObject emergencyBtn;
    Button btnUse;

    void Start()
    {
    }


    // 시체와 충돌 시 리포트 
    [SerializeField]
    Color deadColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody") && SH_VoteManager.instance.isVote == false)
        {
            SH_RoomUI.instance.reportedDeadBody = collision.gameObject.GetPhotonView();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody") && SH_VoteManager.instance.isVote == false)
        {
            // 로컬 플레이어만 리포트 활성화 가능
            if (photonView.IsMine)
            {
                JM_CrewUI.instance.isReportAble = true;
                SH_RoomUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
                print("시체 신고완료");
            }
        }

        // 긴급회의 
        if (collision.gameObject.CompareTag("Emergency"))
        {
            if (photonView.IsMine)
            {
                // 긴급회의 소집한 사람 포톤뷰 ID 저장
                photonView.RPC("RPC_SaveID", RpcTarget.All);
                SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = true;
                JM_CrewUI.instance.isMissionAble = true;
                SH_RoomUI.instance.isEmergency = true;
                print("긴급회의 부딪힘");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        JM_CrewUI.instance.isReportAble = false;
        SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = false;
        JM_CrewUI.instance.isMissionAble = false;
        SH_RoomUI.instance.isEmergency = false;
    }

    [PunRPC]
    public void RPC_SaveID()
    {
        SH_VoteManager.instance.emergeViewID = photonView.ViewID;
    }
}
