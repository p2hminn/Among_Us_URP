using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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
    public Color playerColor;
    GameObject emergencyBtn;
    Button btnUse;

    void Start()
    {
    }


    // ��ü�� �浹 �� ����Ʈ 
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
            // ���� �÷��̾ ����Ʈ Ȱ��ȭ ����
            if (photonView.IsMine)
            {
                JM_CrewUI.instance.isReportAble = true;
                SH_RoomUI.instance.dieColor = collision.gameObject.GetComponent<JM_DeadBody>().color;
                print("��ü �Ű�Ϸ�");
            }
        }

        // ���ȸ�� 
        if (collision.gameObject.CompareTag("Emergency"))
        {
            if (photonView.IsMine)
            {
                // ���ȸ�� ������ ��� ����� ID ����
                photonView.RPC("RPC_SaveID", RpcTarget.All);
                SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = true;
                JM_CrewUI.instance.isMissionAble = true;
                SH_RoomUI.instance.isEmergency = true;
                print("���ȸ�� �ε���");
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
