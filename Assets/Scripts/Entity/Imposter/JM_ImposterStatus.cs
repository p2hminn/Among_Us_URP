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
    Rigidbody2D rb;
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

    // 벤트
    public bool isUp;
    public bool isDown;
    bool isInVent;
    bool isOutVent;
    public Vector3 originPos;
    public JM_VentTrigger ventCode;

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
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();

        // imposterColor = pm.color;
        state = State.idle;

        // 시작할때 코드정보 공유
        JM_ImposterUI.instance.imposterCode = this;

        reportButton = GameObject.Find("Report_Imposter").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            GoUp();
            
        }
        if (isDown)
        {
            GoDown();         
        }
        if (isInVent)
        {
            //gameObject.SetActive(false);
            ventCode.isInVent = true;
        }
        if (isOutVent)
        {
            //gameObject.SetActive(true);
            ventCode.isInVent = false;
        }
    }

    public void GoUp()
    {
        transform.position += Vector3.up * 2 * Time.deltaTime;
    }

    public void GoDown()
    {
        transform.position -= Vector3.up * 5 * Time.deltaTime;
        if (Vector3.Distance(originPos, transform.position) <= 0.1f)
        {
            transform.position = originPos;
            isDown = false;
            isInVent = true;
        }
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
            if (photonView.IsMine)
            {
                SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = true;
                JM_ImposterUI.instance.isUseable = true;
                SH_RoomUI.instance.isEmergency = true;
            }
        }
    }
    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 닿아있다가 떨어지면 공격 불가능
        if (collision.gameObject.name.Contains("Crew"))
        {
            JM_ImposterUI.instance.isAttackOK = false;
            isAttackOk = false;
            isMission = false;
        }
        else if (collision.gameObject.name.Contains("Vent"))
        {
            isVent = true;
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
