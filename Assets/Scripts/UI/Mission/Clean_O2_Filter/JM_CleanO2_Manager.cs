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
    Vector3 leaf1Pos;
    Vector3 leaf2Pos;
    Vector3 leaf3Pos;
    Vector3 leaf4Pos;
    Vector3 leaf5Pos;

    public GameObject missionTrigger;

    // Start is called before the first frame update
    void Start()
    {
        leaf1Pos = leaf1.transform.position;
        leaf2Pos = leaf2.transform.position;
        leaf3Pos = leaf3.transform.position;
        leaf4Pos = leaf4.transform.position;
        leaf5Pos = leaf5.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!leaf1.activeSelf && !leaf2.activeSelf && !leaf3.activeSelf && !leaf4.activeSelf && !leaf5.activeSelf)
        {
            currentTime += Time.deltaTime;
            isMissionComplete = true;
            //JM_MissionStatus.instance.isMissionDone = true;
            JM_MissionStatus.instance.SetMissionDone();

            JM_CrewMapManager.instance.CleanO2Filter();

            taskCompletedUI.SetActive(true);

            missionTrigger.GetComponent<JM_MissionTrigger>().DisableTrigger();


            if (currentTime >= 1)
            {
                cleanO2FilterUI.SetActive(false);
            }
        }
    }

    public void OnClickCancel()
    {
        leaf1.SetActive(true);
        leaf2.SetActive(true);
        leaf3.SetActive(true);
        leaf4.SetActive(true);
        leaf5.SetActive(true);
        leaf1.transform.position = leaf1Pos;
        leaf2.transform.position = leaf2Pos;
        leaf3.transform.position = leaf3Pos;
        leaf4.transform.position = leaf4Pos;
        leaf5.transform.position = leaf5Pos;

        cleanO2FilterUI.SetActive(false);

    }
}
