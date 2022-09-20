using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_ImposterUI : MonoBehaviour
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


    // Start is called before the first frame update
    void Start()
    {
        // ���� Ŀ���ҿ��� ������ ���������� �ϴ� ������Ÿ�� ���� 10�ʷ�
        attackCoolTime = 10f;

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
        else
        {
            attackButton.interactable = false;
        }
    }

    // ���ݹ�ư ������ ���������ڵ忡�� �����Լ� ����
    public void ClickAttack()
    {
        imposterCode.Attack();
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
