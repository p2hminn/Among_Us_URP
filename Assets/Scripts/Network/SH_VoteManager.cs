using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

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
    // 투표 결과 (방장이 수집) : 패널 인덱스, 투표수
    public int[] voteResult;// 인덱스 : 몇 번째 패널인지, int : 몇 표를 받았는지
    public int voteCompleteNum;  // 모두 투표 완료했는지 알기 위해 수 세기

    bool isOnce;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("투표한 사람 : " + voteCompleteNum);
        }
        

        // 모두 투표완료하면 모두에게 투표 결과 보여주기
        if (voteCompleteNum == PhotonNetwork.CurrentRoom.PlayerCount && !isOnce && JM_GameManager.instance.isGameRoom)
        {
            isOnce = true;
            VoteResult();
        }
    }

    // 플레이어 패널 세팅
    public void PlayerPanelSetting()
    {
        isVote = true;
        // 초기화
        for (int i=0; i < voteResult.Length; i++)
        {
            voteResult[i] = 0;
        }
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
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) voteCompleteNum++;
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

    // 투표 결과 발표
    /*
     1. 각 패널들이 투표확인 버튼을 누르면 방장의 VoteManager에게 투표 결과 보냄
     2. 방장의 VoteManager는 모두가 투표완료할 경우 RPC로 모두에게 투표결과를 알려준다.
     3. 추방 UI 보여주고 다시 게임 시작
     */
    public Text voteTitle;
    public GameObject voteCheckCrewFactory;
    public Transform panels;
    public int maxVoteIndex;  // 최다 득표자의 인덱스
    int maxVoteNum;  // 최다 득표자 수 
    public Text voteResultText;  // 투표 결과 UI에 입력할 Text
    public GameObject voteResultUI; // 투표 결과 UI
    public void VoteResult()
    {
        voteTitle.text = "투표 결과";
        panels = GameObject.FindGameObjectsWithTag("Panels")[0].transform;
        // Array돌면서 VotingCrew의 자식으로 넣어주기
        for (int i=0; i < panels.childCount; i++)
        {
            GameObject votingCrew = panels.GetChild(i).GetChild(4).gameObject;
            for (int j=0; j < voteResult[i]; j++)
            {
                GameObject vote = Instantiate(voteCheckCrewFactory);
                vote.transform.SetParent(panels, false); // VotingCrew의 자식으로 투표 받은 만큼 voteCheckCrew 프리팹 만들어 넣어주기
            }
        }

        // 가장 투표 많이 받은 결과 
        int maxVote = voteResult.Max();
        maxVoteIndex = voteResult.ToList().IndexOf(maxVote);
        // 최다 득표자 수 구하기
        for (int i=0; i<voteResult.Length; i++)
        {
            if (voteResult[i] == maxVote)
            {
                maxVoteNum++;
            }
        }


        // 최다 득표자가 다수인 경우
        if (maxVoteNum > 1)
        {
            voteResultText.text = "아무도 방출되지 않았습니다. (건너뜀)";
        }
        // 한명만 최다 득표인 경우
        else
        {
            // 최다 득표 플레이어의 스크립트
            JM_PlayerMove pm = trPanel.GetChild(maxVoteIndex).gameObject.GetComponent<SH_PlayerPanel>().photonView.GetComponent<JM_PlayerMove>();
            // 임포스터인 경우
            if (pm.isImposter)
            {
                voteResultText.text = $"{pm.nickName}님은 임포스터였습니다.";
            }
            // 임포스터 아닌 경우
            else
            {
                voteResultText.text = $"{pm.nickName}님은 임포스터가 아니었습니다.";
            }
        }
        StartCoroutine("ActivateVoteResultUI");
    }
    IEnumerator ActivateVoteResultUI()
    {
        voteResultUI.SetActive(true);
        yield return new WaitForSeconds(4);
        voteResultUI.SetActive(false);
        voteUI.SetActive(false);
        isVote = false;
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
        //btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        //btnSKipVote.gameObject.SetActive(false);
        print("다 비활성화함");
    }
}
