using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_MissionDoneUI : MonoBehaviour
{
    public GameObject missionCompleteUI;
    float currentTime;
    bool isUIComplete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

        if (missionCompleteUI.activeSelf)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 4)
            { 
                missionCompleteUI.SetActive(false);
                print("setActive false");
                currentTime = 0;
            }
        }

    }

        
}
