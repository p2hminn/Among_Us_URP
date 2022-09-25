using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_MissionStatus : MonoBehaviour
{
    public static JM_MissionStatus instance;

    public Slider missionSlider;

    public bool isMissionDone;
    float currentTime;
    float sliderValue;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderValue = 0;
        // test
        //isMissionDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMissionDone)
        {
            UpdateMissionSlider();
        }
    }

    public void UpdateMissionSlider()
    {
        currentTime += Time.deltaTime;
        missionSlider.value = currentTime;
        if (missionSlider.value - sliderValue >= 1)
        {
            isMissionDone = false;
            sliderValue = missionSlider.value;
        }
    }

}
