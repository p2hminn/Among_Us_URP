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
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        // imposterColor = pm.color;
        state = State.idle;

        // �����Ҷ� �ڵ����� ����
        JM_ImposterUI.instance.imposterCode = this;

        reportButton = GameObject.Find("Report_Imposter").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    { 



    }
    
    // �÷��̾�� ����ִ� ���� ���ݰ���
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

        // �������͵� ��ü��  �浹�� ��� ����Ʈ�� �� �ִ�.
        else if (collision.gameObject.name.Contains("DeadBody"))
        {
            // ���� �������͸� ����Ʈ Ȱ��ȭ ����
            if (photonView.IsMine)
            {
                reportButton.interactable = true;
            }
        }

    }
    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
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
        // ��ü�� ������ ��� ����Ʈ �Ұ���
        else if (collision.gameObject.name.Contains("DeadBody"))
        {
            reportButton.interactable = false;
        }
    }
}
