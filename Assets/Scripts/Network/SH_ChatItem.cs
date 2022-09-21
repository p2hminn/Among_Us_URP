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
        // preferredH ������Ʈ�� ����
        if (preferredH != chatText.preferredHeight)
        {
            // chatText.text�� ũ�⿡ �°� ContentSize ����
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
            preferredH = chatText.preferredHeight;
        }
    }

    // Text ����, Text ������ ũ�⿡ �°� �ڽ��� ContentSize ����
    public void SetText(string s)
    {
        chatText.text = s;
    }
}
