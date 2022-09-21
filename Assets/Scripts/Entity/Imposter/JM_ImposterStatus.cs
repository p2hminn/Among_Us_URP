using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_ImposterStatus : MonoBehaviourPun
{

    JM_PlayerStatus ps;
    Animator anim;
    JM_PlayerMove pm;
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

        // �����Ҷ� �ڵ����� ����
        JM_ImposterUI.instance.imposterCode = this;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // ���� ������ ��Ȳ�϶�
        if (isAttackOk)
        {
            print("kill ok");
            // ų ��ư Ȱ��ȭ�ǰ� ų ��ư ������ ũ�� ����
            // �ϴ� 2 ������ �״°ɷ�
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // �÷��̾� ���̱�
                ps.Dead();
                print("���������� ���̱� �۵���");
            }
        }

        // ��Ʈ Ż �� �ִ� ��Ȳ�϶�
        if (isVent)
        {
            // ��Ʈ ������ (�ϴ� v�� �����ϰ� ���߿� �ٲ� ����)
            if (Input.GetKeyDown(KeyCode.V))
            {
                // ��Ʈ Ÿ�� ����
                state = State.vent;
            }
        }

        // ��¥�̼� ������ ��Ȳ�϶�
        if (isMission)
        {
            // �̼� ������ 
            if (Input.GetKeyDown(KeyCode.B))
            {
                state = State.mission;
            }
        }
        */




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
        
    }
    
    // ����ִٰ� �������� ���� �Ұ���
    private void OnTriggerExit2D(Collider2D collision)
    {
        JM_ImposterUI.instance.isAttackOK = false;
        isAttackOk = false;
        isVent = false;
        isMission = false;
    }
}
