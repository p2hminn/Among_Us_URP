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


    // Start is called before the first frame update
    void Start()
    {
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
    }

    // ���ݹ�ư ������ ���������ڵ忡�� �����Լ� ����
    public void ClickAttack()
    {
        // imposterCode.Attack();
        victimCrew.GetComponent<JM_PlayerMove>().Dead(crewColor.r, crewColor.g, crewColor.b, crewColor.a, 
            imposterColor.r, imposterColor.g, imposterColor.b, imposterColor.a);

        isButtonActivate = false;
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
}
