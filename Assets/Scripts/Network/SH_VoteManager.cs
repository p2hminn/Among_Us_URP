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
    GameObject voteUI;
    // 플레이어 패널 프리팹
    public GameObject playerPanelFactory;
    // Player panel 넣을 부모 객체
    public Transform trPanel;


    void Start()
    {
        
    }

    
    void Update()
    {
        // 투표 UI 활성화
        if (isVote) voteUI.SetActive(true);
    }


    // 플레이어 패널 세팅
    void PlayerPanelSetting()
    {
        // 플레이어만큼 패널 세팅하기
        foreach ( PhotonView photonView in JM_GameManager.instance.playerList )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            // panel 상세 정보 세팅
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            playerPanel.SetInfo(photonView);
            // 10초 지날때까지는 버튼 비활성화
            playerPanel.GetComponent<Button>().interactable = false;
        }
    }
}
