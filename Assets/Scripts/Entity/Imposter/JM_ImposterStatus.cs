using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JM_ImposterStatus : MonoBehaviourPun
{

    JM_PlayerStatus ps;
    public Animator anim;
    JM_PlayerMove pm;
    // 기존 속도를 저장할 변수 생성
    float curPlayerSpeed;

    // 공격가능여부
    bool isAttackOk;

    // 벤트 여부
    bool isVent;

    // 가짜미션 여부
    bool isMission;

    // 임포스터 색
    Color imposterColor;

    // 리포트 버튼
    Button reportButton;

    public enum State
    {
        idle,
        move,
        mission,
        vent,
        kill,
    }
    State state;

    void Start()
    {
        anim = GetComponent<Animator>();

        // imposterColor = pm.color;
        state = State.idle;

        // 시작할때 코드정보 공유
        JM_ImposterUI.instance.imposterCode = this;

        reportButton = GameObject.Find("Report_Imposter").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // 공격 가능한 상황일때
        if (isAttackOk)
        {
            print("kill ok");
            // 킬 버튼 활성화되고 킬 버튼 누르면 크루 죽음
            // 일단 2 누르면 죽는걸로
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 플레이어 죽이기
                ps.Dead();
                print("정상적으로 죽이기 작동함");
            }
        }

        // 벤트 탈 수 있는 상황일때
        if (isVent)
        {
            // 벤트 누르면 (일단 v로 설정하고 나중에 바꿀 예정)
            if (Input.GetKeyDown(KeyCode.V))
            {
                // 벤트 타는 상태
                state = State.vent;
            }
        }

        // 가짜미션 가능한 상황일때
        if (isMission)
        {
            // 미션 누르면 
            if (Input.GetKeyDown(KeyCode.B))
            {
                state = State.mission;
            }
        }
        */




    }
    
    // 플레이어랑 닿아있는 동안 공격가능
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew"))
        {
            isAttackOk = true;
            JM_ImposterUI.instance.isAttackOK = true;
            JM_ImposterUI.instance.victimCrew = collision.gameObject;

            JM_ImposterUI.instance.imposterColor = GetComponent<JM_PlayerMove>().color;
            JM_ImposterUI.instance.crewColor = collision.gameObject.transform.GetComponent<JM_PlayerMove>().color;
            
            ps = collision.gameObject.transform.GetComponent<JM_PlayerStatus>();
        }

        else if (collision.gameObject.name.Contains("Vent"))
        {
            isVent = true;

        }

        else if (collision.gameObject.name.Contains("ImposterMission"))
        {
            isMission = true;
        }

        // 임포스터도 시체와  충돌할 경우 리포트할 수 있다.
        else if (collision.gameObject.name.Contains("DeadBody"))
        {
            // 로컬 임포스터만 리포트 활성화 가능
            if (photonView.IsMine)
            {
                reportButton.interactable = true;
            }
        }

        // 긴급회의
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = true;
            JM_ImposterUI.instance.isUseable = true;
            SH_RoomUI.instance.isEmergency = true;
        }

    }
    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 닿아있다가 떨어지면 공격 불가능
        if (collision.gameObject.name.Contains("Crew"))
        {
            JM_ImposterUI.instance.isAttackOK = false;
            isAttackOk = false;
            isVent = false;
            isMission = false;
        }
        // 시체와 떨어질 경우 리포트 불가능
        else if (collision.gameObject.name.Contains("DeadBody"))
        {
            reportButton.interactable = false;
        }
        // 긴급회의
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = false;
            JM_ImposterUI.instance.isUseable = false;
            SH_RoomUI.instance.isEmergency = false;
        }
    }
}
