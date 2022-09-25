using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_CrewUI : MonoBehaviour
{
    // singleton
    public  static JM_CrewUI instance;

    // UI 사용될 크루 및 임포스터 오브젝트
    public GameObject imposter;
    public GameObject crew;

    // 크루 죽는 UI
    public GameObject crewDieUI;

    [SerializeField]
    Color imposterColor;
    [SerializeField]
    Color crewColor;

    float currentTime;

    bool dieUIEnd;

    // ** 미션 관련 **

    public Button missionButton;

    public bool isMissionAble;
    public JM_MissionTrigger missionTrigger;

    // ** 리포트 관련 **
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
        // Report 버튼 끄고 시작
        reportButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ** 미션 **
        // 미션 가능한 상태일때
        if (isMissionAble)
        {
            // 미션 버튼 활성화
            missionButton.interactable = true;
        }
        else if (!isMissionAble)
        {
            missionButton.interactable = false;
        }

        // ** 리포트 **
        // 리포트 가능한 상태(
        if (isReportAble)
        {
            // 리포트 버튼 활성화
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

    // 미션 UI 
    public void onClickMission()
    {
        // 미션버튼 누르면 받은 미션트리거 코드의 미션실행 함수 호출
        missionTrigger.StartMission();
    }



    

    // 크루 죽는 UI
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
