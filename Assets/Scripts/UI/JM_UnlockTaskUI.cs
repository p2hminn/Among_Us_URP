using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class JM_UnlockTaskUI : MonoBehaviour
{
    public List<Text> buttonList;
    public List<int> textList;
    public List<GameObject> ansList;

    Color successColor;
    Color defaultColor;

    public GameObject taskUI;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, textList.Count);
            buttonList[i].text = textList[randomNum].ToString();
            textList.RemoveAt(randomNum);
        }
        successColor = new Color(48f / 255f, 161f / 255f, 77f / 255f, 1f);
        defaultColor = new Color(189f / 255f, 201f / 255f, 241f / 255f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ansList.Count >= 10)
        {
            taskUI.SetActive(false);
        }
    }
    public void OnClick()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text) == ansList.Count + 1)
        {
            ansList.Add(button);
            button.GetComponent<Image>().color = successColor;
        }
        else
        {
            for (int i = 0; i < ansList.Count; i++)
            {
                ansList[i].GetComponent<Image>().color = defaultColor;
            }
            ansList.Clear();
            return;
        }
    }

    public void OnClickCancel()
    {
        for (int i = 0; i < ansList.Count; i++)
        {
            ansList[i].GetComponent<Image>().color = defaultColor;
        }
        ansList.Clear();
        taskUI.SetActive(false);
    }
}
