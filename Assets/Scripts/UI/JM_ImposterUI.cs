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

    // Use ��ư
    public Button useButton;
    public bool isUseable;

    // �������� ��Ʈ ����
    public bool isVent;

    public Button ventButton;
    public Button sabotageButton;

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
            print("isVent status");
            sabotageButton.gameObject.SetActive(false);
            ventButton.gameObject.SetActive(true);
            ventButton.interactable = true;
        }
        if (!isVent)
        {
            sabotageButton.gameObject.SetActive(true);
            ventButton.gameObject.SetActive(false);
        }
        print(isUseable);


        // Use ��ư
        if (isUseable)
        {
            useButton.interactable = true;
            print("true");
        }
        else if (!isUseable)
        {
            useButton.interactable = false;
            print("false");
        }
    }

    // ���ݹ�ư ������ ���������ڵ忡�� �����Լ� ����
    public void ClickAttack()
    {
        // imposterCode.Attack();
        // �÷��̾� �״� �Լ� ȣ��
        victimCrew.GetComponent<JM_PlayerMove>().Dead(crewColor.r, crewColor.g, crewColor.b, crewColor.a, 
            imposterColor.r, imposterColor.g, imposterColor.b, imposterColor.a);

        isButtonActivate = false;
    }

    public void ClickVent()
    {
        imposterCode.anim.SetTrigger("Vent");
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
    }

    public void onClickUse()
    {
        if (SH_RoomUI.instance.isEmergency)
        {
            SH_RoomUI.instance.photonView.RPC("EmergencyMeeting", RpcTarget.All);
        }
    }

}
