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
        // 두번째 클릭
        if (Time.time - doubleClickedTime < interval)
        {
            doubleClickedTime = -1.0f;

            // 방목록 더블 클릭할 경우 방 참가
            lobbyManager.OnClickJoinRoom();
        }
        // 첫번째 클릭
        else
        {
            doubleClickedTime = Time.time;
        }
    }
}
