using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_VoteManager : MonoBehaviourPun
{
    public static SH_VoteManager instance;
    private void Awake()
    {
        instance = this;
    }



    // 투표 진행 여부 
    public bool isVote;
    // 투표 UI
    public GameObject voteUI;
    // 플레이어 패널 프리팹
    public GameObject playerPanelFactory;
    // Player panel 넣을 부모 객체
    public Transform trPanel;



    // 플레이어 패널 세팅
    public void PlayerPanelSetting()
    {
        isVote = true;
        voteUI.SetActive(true);
        // 플레이어만큼 패널 세팅하기
        for ( int i=0; i <  JM_GameManager.instance.playerList.Count; i++ )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel 상세 정보 세팅
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i]);
            // 죽은 크루의 경우 투표 못하도록 막기
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost"))
            {

            }
            //playerPanel.GetComponent<Button>().interactable = false;
        }
    }
}
