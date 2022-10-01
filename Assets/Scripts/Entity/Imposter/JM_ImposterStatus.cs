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

    // ��Ʈ
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

        // �����Ҷ� �ڵ����� ����
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

        // ���ȸ��
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = true;
            JM_ImposterUI.instance.isUseable = true;
            SH_RoomUI.instance.isEmergency = true;
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
        // ���ȸ��
        else if (collision.gameObject.CompareTag("Emergency"))
        {
            SH_RoomUI.instance.btnEmergency.GetComponent<SpriteRenderer>().enabled = false;
            JM_ImposterUI.instance.isUseable = false;
            SH_RoomUI.instance.isEmergency = false;
        }
    }
}
