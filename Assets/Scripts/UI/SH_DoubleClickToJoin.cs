using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SH_DoubleClickToJoin : MonoBehaviour, IPointerClickHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    
    SH_LobbyManager lobbyManager;

    private void Awake()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<SH_LobbyManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �ι�° Ŭ��
        if (Time.time - doubleClickedTime < interval)
        {
            doubleClickedTime = -1.0f;

            // ���� ���� Ŭ���� ��� �� ����
            lobbyManager.OnClickJoinRoom();
        }
        // ù��° Ŭ��
        else
        {
            doubleClickedTime = Time.time;
        }
    }
}
