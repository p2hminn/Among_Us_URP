using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_PlayerStatus : MonoBehaviourPun
{
    Animator anim;
    // ������ ��ü�� �����Ѵ�.
    // ���ӸŴ������� ����?
    // ��ü ���� �÷��̾��� ��
    // �ϴ� ��ü �����ϴ°ź���

    // ��ü ��������
    public GameObject deadBodyGenerator;
    // ����Ʈ UI
    public GameObject reportUI;

    public enum State
    {
        crew,
        ghost,
    }

    public State state;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    [SerializeField]
    Color deadColor;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DeadBody"))
        {
            // ���� �÷��̾ ����Ʈ Ȱ��ȭ ����
            if (photonView.IsMine)
            {
                JM_CrewUI.instance.isReportAble = true;
                SH_RoomUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
            }
            /*
            // Report ��ư�� ������
            if (JM_CrewUI.instance.onReport && �߰�����)
            {
                // ��ü�� ���� ��������
                deadColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
                // RPC�� ��� ����Ʈ UI Ȱ��ȭ
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
