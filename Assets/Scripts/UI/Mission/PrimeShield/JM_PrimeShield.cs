using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JM_PrimeShield : MonoBehaviour
{

    public GameObject primeShieldUI;
    public GameObject shieldStatus;

    public List<Button> shieldList;
    public List<int> shieldNumList;
    int shield1;
    int shield2;
    int shield3;

    public bool isMissionComplete;

    float currentTime;

    public GameObject taskCompletedUI;

    public GameObject missionTrigger;

    // Start is called before the first frame update
    void Start()
    {
        SetShield();
    }

    // 랜덤 세개만 버튼 인터랙터블하게

    // Update is called once per frame
    void Update()
    {
        if (shieldList[shield1].interactable == false && shieldList[shield2].interactable == false && shieldList[shield3].interactable == false)
        {
            shieldStatus.GetComponent<Image>().color = Color.white;
            shieldStatus.transform.Rotate(0, 0, 0.5f);
            isMissionComplete = true;
            // JM_MissionStatus.instance.isMissionDone = true;
            JM_MissionStatus.instance.SetMissionDone();

            JM_CrewMapManager.instance.PrimeShield();

            taskCompletedUI.SetActive(true);

            missionTrigger.GetComponent<JM_MissionTrigger>().DisableTrigger();


            currentTime += Time.deltaTime;

            if (currentTime >= 1)
            {
                primeShieldUI.SetActive(false);
            }           
        }
    }

    void SetShield()
    {
        shield1 = Random.Range(0, shieldNumList.Count);
        shieldNumList.RemoveAt(shield1);
        shield2 = Random.Range(0, shieldNumList.Count);
        shieldNumList.RemoveAt(shield2);
        shield3 = Random.Range(0, shieldNumList.Count);
        shieldNumList.RemoveAt(shield3);

        shieldList[shield1].interactable = true;
        shieldList[shield2].interactable = true;
        shieldList[shield3].interactable = true;

    }
    public void OnClickShield()
    {
        print("good");
        
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button.GetComponent<Button>().interactable = false;
        
    }

    public void OnClickCancel()
    {
        for (int i = 0; i < shieldList.Count; i++)
        {
            shieldList[i].interactable = false;
        }
        SetShield();
        primeShieldUI.SetActive(false);
    }
}
