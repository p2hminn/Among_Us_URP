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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    [SerializeField]
    Color deadColor;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody"))
        {
            JM_CrewUI.instance.isReportAble = true; 
            JM_CrewUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;

            /*
            // Report 버튼이 눌리면
            if (JM_CrewUI.instance.onReport && 추가조건)
            {
                // 시체의 색깔 가져오기
                deadColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
                // RPC로 모든 리포트 UI 활성화
                // photonView.RPC("RPC_Report", RpcTarget.All, deadColor.r, deadColor.g, deadColor.b, deadColor.a);
                SH_RoomUI.instance.Report(deadColor.r, deadColor.g, deadColor.b, deadColor.a);
            }
            */
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        JM_CrewUI.instance.isReportAble = false;
    }

    

}
