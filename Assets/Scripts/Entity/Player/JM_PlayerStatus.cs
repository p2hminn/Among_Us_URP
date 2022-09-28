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
    public Color playerColor;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerColor = GetComponent<JM_PlayerMove>().color;
    }


    // ��ü�� �浹 �� ����Ʈ 
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
                SH_RoomUI.instance.reportedDeadBody = collision.gameObject;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        JM_CrewUI.instance.isReportAble = false;
    }



    // �÷��̾� ��Ʈ�� ����!
    public Sprite ghostSprite;
    public void ToGhost()
    {
        // ������Ʈ �±� �ٲ��ֱ�
        gameObject.tag = "Ghost";

        // ���� �÷��̾� -> ��Ʈ, ����Ʈ �÷��̾� -> ��Ȱ��ȭ
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
            return;
        }

        // ���� ������Ʈ
        state = State.ghost;

        // ��Ȱ��ȭ �� Ȱ��ȭ
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<JM_PlayerMove>().enabled = false;
        if (GetComponent<JM_PlayerMove>().isImposter) GetComponent<JM_PlayerStatus>().enabled = false;
        else GetComponent<JM_ImposterStatus>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<JM_Ghost>().enabled = true;

        // Animator ����
        //anim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Animator/JM_GhostAnimator", typeof(RuntimeAnimatorController)));
        anim.runtimeAnimatorController = Resources.Load("Animator/JM_GhostAnimator") as RuntimeAnimatorController;
        // Sprite ����
        GetComponent<SpriteRenderer>().sprite = ghostSprite;
        // ���� ���� ����
        GetComponent<JM_Ghost>().SetColor(playerColor);
    }

    

}
