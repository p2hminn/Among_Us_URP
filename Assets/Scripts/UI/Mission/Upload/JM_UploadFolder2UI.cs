using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_UploadFolder2UI : MonoBehaviour
{
    public GameObject missionUI;

    public Slider slider;
    public Button uploadButton;
    public Button percentage;
    string percentText;
    public Button cancelButton;

    bool isStart;
    bool isDone;
    float test;

    float currentTime;

    public GameObject missionCompleteUI;
    public GameObject taskCompletedUI;

    public GameObject missionTrigger;

    // Start is called before the first frame update
    void Start()
    {
        percentText = percentage.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text;
    }

    // Update is called once per frame
    void Update()
    { 

        if (isStart)
        {
            slider.value += Time.deltaTime;
            test += Time.deltaTime / 8 * 100;
            
            percentage.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = 
                ((int)test).ToString() + " %";
            if (slider.value >= slider.maxValue)
            {
                print("done");
                isStart = false;
                isDone = true;

                //JM_MissionStatus.instance.isMissionDone = true;
                JM_MissionStatus.instance.SetMissionDone();

                JM_CrewMapManager.instance.UploadSt2Admin();

                taskCompletedUI.SetActive(true);

                missionTrigger.GetComponent<JM_MissionTrigger>().DisableTrigger();

            }
        }
        if (isDone)
        {      
            currentTime += Time.deltaTime;
            if (currentTime >= 1)
            {
                missionUI.SetActive(false);
            }
        }
        
    }

    private void Reset()
    {
        slider.value = slider.minValue;
        missionUI.SetActive(false);
        isStart = false;
        isDone = false;
    }

    public void onClickUpload()
    {
        isStart = true;
    }

    public void onClickCancel()
    {
        Reset();
    }
}
