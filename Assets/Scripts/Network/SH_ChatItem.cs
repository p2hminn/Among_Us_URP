using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_ChatItem : MonoBehaviour
{
    // Text
    public Text chatText;
    // RectTransform
    RectTransform rt;
    // preferredHeight
    float preferredH;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    
    void Update()
    {
        // preferredH 업데이트될 때만
        if (preferredH != chatText.preferredHeight)
        {
            // chatText.text의 크기에 맞게 ContentSize 변경
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
            preferredH = chatText.preferredHeight;
        }
    }

    // Text 세팅, Text 내용의 크기에 맞게 자신의 ContentSize 변경
    public void SetText(string s)
    {
        chatText.text = s;
    }
}
