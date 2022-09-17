using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ImposterStatus : MonoBehaviour
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
        pm = GetComponent<JM_PlayerMove>();
        curPlayerSpeed = pm.playerSpeed;
        state = State.idle;
    }

    // Update is called once per frame
    void Update()
    {
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





    }

    // �÷��̾�� ����ִ� ���� ���ݰ���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew"))
        {
            isAttackOk = true;
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
        isAttackOk = false;
        isVent = false;
        isMission = false;
    }
}
