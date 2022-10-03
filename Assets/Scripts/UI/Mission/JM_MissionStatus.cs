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
    float currentMissionDone;

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
        isMissionDone = false;
        currentMissionDone = 1;
        isOnce = false;
    }

    bool isOnce;
    void Update()
    {
        if (isMissionDone)
        {
            UpdateMissionSlider();
        }
        if (missionSlider.value >= missionSlider.maxValue && !isOnce)
        {
            JM_GameManager.instance.photonView.RPC("FindYourEnd", RpcTarget.All, true);
            isOnce = true;
        }
        if (!isMissionDone)
        {
            currentTime = 0;
        }
    }

    public void UpdateMissionSlider()
    {
        currentTime += Time.deltaTime;
        missionSlider.value += Time.deltaTime;
        if (currentTime >= 1)
        {
            SetMissionNotDone();
        }
    }

    public void SetMissionDone()
    {
        photonView.RPC("RPC_SetMissionDone", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_SetMissionDone()
    {
        isMissionDone = true;        
    }

    public void SetMissionNotDone()
    {
        photonView.RPC("RPC_SetMissionNotDone", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_SetMissionNotDone()
    {
        isMissionDone = false;
    }

}
