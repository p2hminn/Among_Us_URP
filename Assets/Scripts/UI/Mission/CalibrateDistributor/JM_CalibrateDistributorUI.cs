using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_CalibrateDistributorUI : MonoBehaviour
{
    public GameObject calibrateDistributorUI;

    // ���κ���
    public GameObject mainBoard;

    // ����
    public GameObject yellowWheel;
    public GameObject blueWheel;
    public GameObject mintWheel;

    // �����̴���
    public Slider yellowSlider;
    public Slider blueSlider;
    public Slider mintSlider;

    // ��ư��
    public Button yellowButton;
    public Button blueButton;
    public Button mintButton;

    // �� ���� �� ������� --> �� ���߸� �̼Ǽ���
    bool isYellowCorrect;
    bool isBlueCorrect;
    bool isMintCorrect;

    JM_CalibrateTrigger calibrateTrigger;

    // ������ ��� ����. ���ٰ� CalibrateTrigger���� ����ο� ���� Ʈ���϶� ��ư�� ������ �׶� �ش� ���� �̵��� �����. 

    // ������ �̵��ӵ�
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
        // �����Ҷ� ����� ��ư�� Ȱ��ȭ
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // �⺻�����δ� �����ӵ��� ���ư���
        yellowWheel.transform.Rotate(0, 0, yellowRotSpeed);
        blueWheel.transform.Rotate(0, 0, blueRotSpeed);
        mintWheel.transform.Rotate(0, 0, mintRotSpeed);

        // ����� ���߸� �Ķ��� ��ư Ȱ��ȭ, �Ķ��� ���߸� ��Ʈ�� ��ư Ȱ��ȭ
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

        // ù��° ������ �ι�° ��ư Ȱ��ȭ ���� �����ϸ� ù��°�� Ȱ��ȭ
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
