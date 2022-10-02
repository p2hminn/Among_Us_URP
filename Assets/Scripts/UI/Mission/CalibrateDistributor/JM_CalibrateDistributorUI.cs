using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_CalibrateDistributorUI : MonoBehaviour
{
    public GameObject calibrateDistributorUI;

    // 메인보드
    public GameObject mainBoard;

    // 링들
    public GameObject yellowWheel;
    public GameObject blueWheel;
    public GameObject mintWheel;

    // 슬라이더들
    public Slider yellowSlider;
    public Slider blueSlider;
    public Slider mintSlider;

    // 버튼들
    public Button yellowButton;
    public Button blueButton;
    public Button mintButton;

    // 각 링들 잘 맞췄는지 --> 다 맞추면 미션성공
    bool isYellowCorrect;
    bool isBlueCorrect;
    bool isMintCorrect;

    JM_CalibrateTrigger calibrateTrigger;

    // 링들은 계속 돈다. 돌다가 CalibrateTrigger에서 이즈옐로우 등이 트루일때 버튼을 누르면 그때 해당 링의 이동을 멈춘다. 

    // 각자의 이동속도
    float yellowRotSpeed;
    float blueRotSpeed;
    float mintRotSpeed;

    float currentTime;

    public GameObject taskCompletedUI;

    public GameObject missionTrigger;


    // Start is called before the first frame update
    void Start()
    {
        calibrateTrigger = mainBoard.transform.GetComponent<JM_CalibrateTrigger>();
        // 리셋할때 노란색 버튼만 활성화
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // 기본적으로는 지정속도로 돌아간다
        yellowWheel.transform.Rotate(0, 0, yellowRotSpeed);
        blueWheel.transform.Rotate(0, 0, blueRotSpeed);
        mintWheel.transform.Rotate(0, 0, mintRotSpeed);

        // 노란색 맞추면 파란색 버튼 활성화, 파란색 맞추면 민트색 버튼 활성화
        if (calibrateTrigger.isYellow)
        {
            yellowSlider.value += 20;
            //yellowButton.interactable = false;
            //blueButton.interactable = true;
        }
        if (calibrateTrigger.isBlue)
        {
            blueSlider.value += 20;
            //blueButton.interactable = false;
            //mintButton.interactable = true;
        }
        if (calibrateTrigger.isMint)
        {
            mintSlider.value += 20;
            //mintButton.interactable = false;
        }      
        if (!calibrateTrigger.isYellow)
        {
            yellowSlider.value -= 10;       
        }
        if (!calibrateTrigger.isBlue) blueSlider.value -= 10;
        if (!calibrateTrigger.isMint) mintSlider.value -= 10;

        if (isYellowCorrect) yellowRotSpeed = 0;
        if (isBlueCorrect) blueRotSpeed = 0;
        if (isMintCorrect) mintRotSpeed = 0;


        if (isYellowCorrect && isBlueCorrect && isMintCorrect)
        {
            print("done");
            //calibrateDistributorUI.SetActive(false);
            //JM_MissionStatus.instance.isMissionDone = true;
            JM_MissionStatus.instance.SetMissionDone();

            JM_CrewMapManager.instance.CalibrateDistributor();

            taskCompletedUI.SetActive(true);

            missionTrigger.GetComponent<JM_MissionTrigger>().DisableTrigger();

            currentTime += Time.deltaTime;
            if (currentTime >= 1)
            {
                calibrateDistributorUI.SetActive(false);
            }
        }

    }

    private void Reset()
    {
        yellowRotSpeed = .6f;
        blueRotSpeed = .6f;
        mintRotSpeed = .6f;

        // 첫번째 눌러야 두번째 버튼 활성화 따라서 리셋하면 첫번째만 활성화
        //yellowButton.interactable = true;
        //blueButton.interactable = false;
        //mintButton.interactable = false;

    }

    public void onClickYellow()
    {
        if (calibrateTrigger.isYellow)
        {
            isYellowCorrect = true;
        }
    }

    public void onClickBlue()
    {
        if (calibrateTrigger.isBlue)
        {
            isBlueCorrect = true;
        }
    }

    public void onClickMint()
    {
        if (calibrateTrigger.isMint)
        {
            isMintCorrect = true;
        }
    }

    public void OnClickCancel()
    {
        Reset();
        calibrateDistributorUI.SetActive(false);
    }

}
