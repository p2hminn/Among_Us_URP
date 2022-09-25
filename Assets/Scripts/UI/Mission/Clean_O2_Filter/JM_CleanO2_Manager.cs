using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_CleanO2_Manager : MonoBehaviour
{
    public GameObject cleanO2FilterUI;

    public GameObject leaf1;
    public GameObject leaf2;
    public GameObject leaf3;
    public GameObject leaf4;
    public GameObject leaf5;

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
        if (!leaf1.activeSelf && !leaf2.activeSelf && !leaf3.activeSelf && !leaf4.activeSelf && !leaf5.activeSelf)
        {
            currentTime += Time.deltaTime;
            isMissionComplete = true;
            JM_MissionStatus.instance.isMissionDone = true;
            taskCompletedUI.SetActive(true);
            if (currentTime >= 1)
            {
                cleanO2FilterUI.SetActive(false);
            }
        }
    }
}
