using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JM_MissionStatus : MonoBehaviourPun
{
    public static JM_MissionStatus instance;

    public Slider missionSlider;

    public bool isMissionDone;
    float currentTime;
    //float sliderValue;

    public bool isCrewWin;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sliderValue = 0;
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
        if (missionSlider.value >= missionSlider.maxValue)
        {
            isCrewWin = true;
        }
    }

    public void UpdateMissionSlider()
    {
        currentTime += Time.deltaTime;
        missionSlider.value = currentTime;
        if (currentTime >= 1)
        {
            isMissionDone = false;
            //sliderValue = missionSlider.value;
        }

    }

    [PunRPC]
    void RPC_UpdateMissionSlider()
    {
        currentTime += Time.deltaTime;
        missionSlider.value = currentTime;
        if (currentTime >= 1)
        {
            print("けいしかいしかいしぉ");
            photonView.RPC("RPC_SetMissionDone", RpcTarget.All);
            //sliderValue = missionSlider.value;
        }
    }

    public void SetMissionDone()
    {
        photonView.RPC("RPC_SetMissionDone", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SetMissionDone()
    {
        isMissionDone = true;        
    }

}
