using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_CrewUI : MonoBehaviour
{
    // singleton
    public  static JM_CrewUI instance;

    // UI ���� ũ�� �� �������� ������Ʈ
    public GameObject imposter;
    public GameObject crew;

    // ũ�� �״� UI
    public GameObject crewDieUI;

    [SerializeField]
    Color imposterColor;
    [SerializeField]
    Color crewColor;

    float currentTime;

    bool dieUIEnd;

    // ** �̼� ���� **

    public Button missionButton;

    public bool isMissionAble;
    public JM_MissionTrigger missionTrigger;

    // ** ����Ʈ ���� **
    public Button reportButton;
    public bool isReportAble;
    public GameObject reportUI;
    public bool onReport;


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Report ��ư ���� ����
        reportButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ** �̼� **
        // �̼� ������ �����϶�
        if (isMissionAble)
        {
            // �̼� ��ư Ȱ��ȭ
            missionButton.interactable = true;
        }
        else if (!isMissionAble)
        {
            missionButton.interactable = false;
        }

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
   
        if (!dieUIEnd && crewDieUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 1)
            {
                crewDieUI.SetActive(false);
                dieUIEnd = true;
                currentTime = 0;
            }
        }
    }

    // �̼� UI 
    public void onClickMission()
    {
        // �̼ǹ�ư ������ ���� �̼�Ʈ���� �ڵ��� �̼ǽ��� �Լ� ȣ��
        missionTrigger.StartMission();
    }



    

    // ũ�� �״� UI
    public void Die(float crewR, float crewG, float crewB, float crewA,
        float imposterR, float imposterG, float imposterB, float imposterA)
    {
        imposterColor = new Color(imposterR, imposterG, imposterB, imposterA);
        crewColor = new Color(crewR, crewG, crewB, crewA);

        crewDieUI.SetActive(true);
        crewDieUI.transform.Find("Imposter").gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", imposterColor);
        crewDieUI.transform.Find("Crew").gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", crewColor);
    }

    

}
