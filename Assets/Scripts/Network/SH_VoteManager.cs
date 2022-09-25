using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_VoteManager : MonoBehaviourPun
{
    // singleton
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
    // 리포트한 사람 포톤뷰 ID
    public int reportViewID;
    // 투표 결과 (방장이 수집)
    public Dictionary<GameObject, int> voteResultDic = new Dictionary<GameObject, int>();
    public int voteCompleteNum;

    bool isOnce;
    private void Update()
    {
        print("투표한 사람 : " + voteCompleteNum);

        // 모두 투표완료하면 모두에게 투표 결과 보여주기
        if (voteCompleteNum == JM_GameManager.instance.playerList.Count && !isOnce)
        {
            isOnce = true;
            photonView.RPC("VoteResult", RpcTarget.All);
        }
    }

    // 플레이어 패널 세팅
    public void PlayerPanelSetting()
    {
        isVote = true;
        // 초기화
        voteResultDic.Clear();
        voteCompleteNum = 0;
        isOnce = false;

        // 투표 UI 활성화
        voteUI.SetActive(true);
        // 플레이어만큼 패널 세팅하기
        for ( int i=0; i < JM_GameManager.instance.playerList.Count; i++ )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel 상세 정보 세팅
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i], reportViewID);
            // 죽은 크루 투표했다고 치기 (RPC로 방장한테 보내야함)
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) 
            // 신고한 사람 표시하기
            //if (JM_GameManager.instance.playerList[i].ViewID == reportViewID) reportImg.gameObject.SetActive(true);
            if (reportViewID != 0) reportViewID = 0;
        }
        // 죽은 크루의 경우 모든 패널 버튼 비활성화해서 투표 못하게 하기
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            if (JM_GameManager.instance.playerList[i].IsMine && JM_GameManager.instance.playerList[i].CompareTag("Ghost"))
            {
                PanelOff();
            }
        }
    }

    [PunRPC]
    public void SendGhostVote()
    {
        voteCompleteNum++;
    }

    // 투표 결과 발표
    /*
     1. 각 패널들이 투표확인 버튼을 누르면 방장의 VoteManager에게 투표 결과 보냄
     2. 방장의 VoteManager는 모두가 투표완료할 경우 RPC로 모두에게 투표결과를 알려준다.
     3. 추방 UI 보여주고 다시 게임 시작
     */
    [PunRPC]
    public void VoteResult()
    {

        print("투표 결과 보여주자~");
        // 투표 끝
        //voteUI.SetActive(false);
        //isVote = false;
    }

    // 전체 패널 투표 버튼 비활성화
    Button btnSKipVote;
    public void PanelOff()
    {
        trPanel = GameObject.FindGameObjectsWithTag("Panels")[0].transform;
        foreach (Transform panel in trPanel)
        {
            panel.GetComponent<Button>().interactable = false;
            panel.GetChild(7).gameObject.SetActive(false);  // Btn_Vote
            panel.GetChild(8).gameObject.SetActive(false);  // Btn_VoteCancel
        }
        // 스킵 버튼 비활성화
        btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        btnSKipVote.gameObject.SetActive(false);
        print("다 비활성화함");
    }
}
