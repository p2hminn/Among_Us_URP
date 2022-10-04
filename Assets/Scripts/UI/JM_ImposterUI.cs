using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JM_ImposterUI : MonoBehaviourPun
{
    // singleton
    public static JM_ImposterUI instance;
    private void Awake()
    {
        instance = this;
    }

    // �������� ���� �ڵ�
    public JM_ImposterStatus imposterCode;

    // ����ð�
    float currentTime;

    // ** �������� ���� ���� **

    // ���ݹ�ư Ȱ��ȭ ���� ����
    public bool isButtonActivate;

    // �������� ���ݰ��ɿ���
    public bool isAttackOK;
    // ���ݹ�ư 
    public Button attackButton;
    // ���� ��Ÿ��
    public float attackCoolTime;

    // ���ݴ��ϴ� ũ��
    public GameObject victimCrew;

    // �������� �� ũ�� ����
    [SerializeField]
    public Color imposterColor;
    [SerializeField]
    public Color crewColor;

    // ����Ʈ ��ư
    public Button reportButton;
    public bool isReportAble;

    // Use ��ư
    public Button useButton;
    public bool isUseable;

    // �������� ��Ʈ ����
    public bool isVent;

    public Button ventButton;
    public Button sabotageButton;
    public GameObject imposterPos;

    // Start is called before the first frame update
    void Start()
    {
        reportButton.interactable = false;
        // ���� Ŀ���ҿ��� ������ ���������� �ϴ� ������Ÿ�� ���� 10�ʷ�
        attackCoolTime = 3;
        attackButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ** ����Ʈ **
        // ����Ʈ ������ ����(
        if (isReportAble)
        {
            // ����Ʈ ��ư Ȱ��ȭ
            reportButton.interactable = true;
        }
        else if (!isReportAble)
        {
            reportButton.interactable = false;
        }

        // ���ݹ�ư ��� �Ұ��� �����϶� ��Ÿ�� �Լ� ����
        if (!isButtonActivate)
            AttackTimeCount();

        if (isAttackOK && isButtonActivate)
        {
            attackButton.interactable = true;
        }
        else if (!isAttackOK)
        {
            attackButton.interactable = false;
        }

        // ��Ʈ
        if (isVent)
        {
            sabotageButton.gameObject.SetActive(false);
            ventButton.gameObject.SetActive(true);
            ventButton.interactable = true;
        }
        if (!isVent)
        {
            sabotageButton.gameObject.SetActive(true);
            ventButton.gameObject.SetActive(false);
        }

        // Use ��ư
        if (isUseable)
        {
            useButton.interactable = true;
        }
        else if (!isUseable)
        {
            useButton.interactable = false;
        }

        

    }
    public AudioSource killSound;
    // ���ݹ�ư ������ ���������ڵ忡�� �����Լ� ����
    public void ClickAttack()
    {
        // imposterCode.Attack();
        // �÷��̾� �״� �Լ� ȣ��
        victimCrew.GetComponent<JM_PlayerMove>().Dead(crewColor.r, crewColor.g, crewColor.b, crewColor.a, 
            imposterColor.r, imposterColor.g, imposterColor.b, imposterColor.a);

        killSound.enabled = true;

        isButtonActivate = false;
    }

    public void ClickVent()
    {
        imposterCode.GetInsideVent();
        imposterCode.LimitSpeed();
        imposterCode.originPos = imposterPos.transform.position;
    }

    // �������� ������ �� �� �ִ� ���°� �Ƿ��� ��Ÿ���� �־�ߵȴ�
    // ��ư ��Ÿ�� ���� ī��Ʈ�ٿ��� ���߿� �߰� ����
    void AttackTimeCount()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= attackCoolTime)
        {
            isButtonActivate = true;
        } 
    }

    public GameObject map;

    public void OnClickMap()
    {
        map.SetActive(true);
        JM_ImposterMapManager.instance.SetPlayerPos();
    }

    public void onClickUse()
    {
        if (SH_RoomUI.instance.isEmergency)
        {
            SH_RoomUI.instance.photonView.RPC("EmergencyMeeting", RpcTarget.All);
        }
    }

}
