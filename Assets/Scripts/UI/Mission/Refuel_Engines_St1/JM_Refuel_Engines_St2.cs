using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_Refuel_Engines_St2 : MonoBehaviour
{
    public GameObject refuelEnginesSt1UI;
    public Slider fuelGage;
    public Slider fuelGage2;
    public GameObject greenlightOn;
    public GameObject redlightOn;


    bool isRefuelStart;
    bool isRefuelEnd;
    public bool isMissionComplete;
    float currentTime;

    public GameObject taskCompletedUI;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRefuelStart)
        {
            currentTime += Time.deltaTime;
            fuelGage.value = currentTime * 0.5f;
            fuelGage2.value -= Time.deltaTime * 0.5f;
            redlightOn.SetActive(true);

            if (fuelGage.value >= fuelGage.maxValue)
            {
                currentTime = 0;
                isRefuelStart = false;
                isRefuelEnd = true;
                redlightOn.SetActive(false);
            }
        }

        if (isRefuelEnd)
        {
            isMissionComplete = true;
            currentTime += Time.deltaTime;
            greenlightOn.SetActive(true);

            //JM_MissionStatus.instance.isMissionDone = true;
            JM_MissionStatus.instance.SetMissionDone();

            if (gameObject.name.Contains("Upper"))
            {
                JM_CrewMapManager.instance.FuelSt2Upper();
            }
            if (gameObject.name.Contains("Lower"))
            {
                JM_CrewMapManager.instance.FuelSt2Lower();
            }

            taskCompletedUI.SetActive(true);

            if (currentTime >= 1)
            {
                refuelEnginesSt1UI.SetActive(false);
            }
        }
    }

    public void onClick()
    {
        isRefuelStart = true;
    }

    public void OnClickCancel()
    {
        isRefuelStart = false;
        fuelGage.value = fuelGage.minValue;
        refuelEnginesSt1UI.SetActive(false);
    }
}