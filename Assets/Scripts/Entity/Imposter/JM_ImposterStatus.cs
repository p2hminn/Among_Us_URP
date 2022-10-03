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
    // ���� �ӵ��� ������ ���� ����
    float curPlayerSpeed;

    // ���ݰ��ɿ���
    bool isAttackOk;

    // ��Ʈ ����
    bool isVent;

    // ��¥�̼� ����
    bool isMission;

    // �������� ��
    Color imposterColor;

    // ����Ʈ ��ư
    public Button reportButton;

    // ��Ʈ
    public bool isUp;
    public bool isDown;
    public bool isInVent;
    public bool isOutVent;
    public Vector3 originPos;
    public JM_VentTrigger ventCode;
    public JM_VentTrigger2 ventCode2;
    public JM_VentTrigger3 ventCode3;


    public bool isOne;
    public bool isSecond;
    public bool isThird;


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

        // �����Ҷ� �ڵ����� ����
        JM_ImposterUI.instance.imposterCode = this;

        reportButton = GameObject.Find("Report_Imposter").GetComponent<Button>();

        curPlayerSpeed = gameObject.GetComponent<JM_PlayerMove>().playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOne)
        {
            isSecond = false;
            isThird = false;
        }
        if (isSecond)
        {
            isOne = false;
            isThird = false;
        }
        if (isThird)
        {
            isOne = false;
            isSecond = false;
        }

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
            if (isOne)
            {
                ventCode.isInVent = true;
            }
            if (isSecond)
            {
                ventCode2.isInVent = true;
            }
            if (isThird)
            {
                ventCode3.isInVent = true;
            }
        }
        if (isOutVent)
        {
            if (isOne)
            {
                ventCode.isInVent = false;
            }
            if (isSecond)
            {
                ventCode2.isInVent = false;
            }
            if (isThird)
            {
                ventCode3.isInVent = false;
            }


            /*
            if (ventCode != null)
            {
                ventCode.isInVent = false;

            }
            if (ventCode2 != null)
            {
                ventCode2.isInVent = false;
            }
            if (ventCode3 != null)
            {
                ventCode3.isInVent = false;
            }
            */

        }
    }

    public void LimitSpeed()
    {
        gameObject.GetComponent<JM_PlayerMove>().playerSpeed = 0;
    }

    public void ReleaseSpeed()
    {
        gameObject.GetComponent<JM_PlayerMove>().playerSpeed = curPlayerSpeed;
    }

    public void GoUp()
    {
        transform.position += Vector3.up * 2 * Time.deltaTime;

    }

    public void GoDown()
    {
        transform.position -= Vector3.up * 6 * Time.deltaTime;
        if (Vector3.Distance(originPos, transform.position) <= 0.1f)
        {
            transform.position = originPos;
            isDown = false;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("JM_VentUp"))
            {
                isOutVent = true;
                isInVent = false;
                ReleaseSpeed();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("JM_VentDown"))
            {
                isOutVent = false;
                isInVent = true;
            }

            
        }
    }

    public void SetInvisible()
    {
        photonView.RPC("RPC_SetInvisible", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SetInvisible()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    public void SetVisible()
    {
        photonView.RPC("RPC_SetVisible", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SetVisible()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        transform.Find("Canvas").gameObject.SetActive(true);
    }


    public void GetInsideVent()
    {
        photonView.RPC("RPC_GetInsideVent", RpcTarget.All);
    }

    [PunRPC]
    void RPC_GetInsideVent()
    {
        anim.SetTrigger("VentDown");
    }

    public void GetOutsideVent()
    {
        photonView.RPC("RPC_GetOutsideVent", RpcTarget.All);
    }

    [PunRPC]
    void RPC_GetOutsideVent()
    {
        anim.SetTrigger("VentUp");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody") && SH_VoteManager.instance.isVote == false)
        {
            SH_RoomUI.instance.reportedDeadBody = collision.gameObject.GetPhotonView();
        }
    }

    // �÷��̾�� ����ִ� ���� ���ݰ���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew") && collision.gameObject.GetComponent<JM_PlayerMove>().nickName.color != Color.red)
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

        // �������͵� ��ü��  �浹�� ��� ����Ʈ�� �� �ִ�.
        else if (collision.gameObject.name.Contains("DeadBody"))
        {
            JM_ImposterUI.instance.isReportAble = true;
            SH_RoomUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;

        }

        // ���ȸ��
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
        JM_ImposterUI.instance.isReportAble = false;
        // ����ִٰ� �������� ���� �Ұ���
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
        // ���ȸ��
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = false;
            JM_ImposterUI.instance.isUseable = false;
            SH_RoomUI.instance.isEmergency = false;
        }
       
    }
}
