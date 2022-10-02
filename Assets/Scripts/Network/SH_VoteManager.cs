using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Reflection;

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
    // 회의 및 투표 시간 Text
    public Text txtVoteTime;
    // 회의 시간
    public float discussTime = 10;
    // 투표 시간
    public float voteTime = 120;
    // 로컬 플레이어의 패널 idx
    public int localPanelIdx;
    public PhotonView p;


    bool isOnce;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("투표한 사람 : " + voteCompleteNum);
        }

        if (p)
        {
            print("죽었니? 2 : " + p.gameObject.activeSelf);
            print("현재 실행 함수2 : " + MethodBase.GetCurrentMethod().Name);
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
        for (int i = 0; i < voteResult.Length; i++)
        {
            voteResult[i] = 0;
        }
        isOnce = false;
        if (voteResult == null || voteResult.Length == 0)
        {
            voteResult = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        }
        foreach (Transform t in trPanel)
        {
            Destroy(t.gameObject);
        }  // 패널


        // 투표 세팅
        voteUI.SetActive(true);
        // 플레이어만큼 패널 세팅하기
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel 오브젝트 이름을 플레이어 이름으로 설정
            playerPanel.gameObject.name = JM_GameManager.instance.playerList[i].Owner.NickName;
            // panel 상세 정보 세팅
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i], reportViewID);
            // 로컬 플레이어의 패널이 몇 번째인지 표기
            if (JM_GameManager.instance.playerList[i].IsMine)
            {
                localPanelIdx = i;
                print("myPanelIndex : " + i);
            }
            // 죽은 크루 투표했다고 치기 (RPC로 방장한테 보내야함)
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) voteCompleteNum++;
        }


        // 회의 시간
        StartCoroutine("StartDiscuss");


    }
    IEnumerator StartDiscuss()
    {
        PanelOff();



        float currTime = 0;

        while (currTime < discussTime)
        {
            currTime += Time.deltaTime;
            txtVoteTime.text = $"회의 시간 : {(int)discussTime - (int)currTime}초";
            yield return null;
        }
        txtVoteTime.text = "";
        // 투표 시작
        PanelOn();
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
    public string saveVoteResult;  // 투표 결과 저장할 변수
    public Text voteResultText;  // 투표 결과 UI에 입력할 Text
    public GameObject voteResultUI; // 투표 결과 UI
    public void VoteResult()
    {
        // 투표 결과 구하기
        // 가장 투표 많이 받은 사람의 득표수
        int maxVote = voteResult.Max();
        maxVoteIndex = voteResult.ToList().IndexOf(maxVote);
        // 최다 득표자 수 구하기
        for (int i = 0; i < voteResult.Length; i++)
        {
            if (voteResult[i] == maxVote)
            {
                maxVoteNum++;
            }
        }
        // 최다 득표자가 다수인 경우 ( 동점이 발생한 경우 )
        if (maxVoteNum > 1)
        {
            saveVoteResult = "아무도 방출되지 않았습니다. (동점 발생)";
        }
        // 한명만 최다 득표인 경우
        else
        {
            // 최다 득표 플레이어의 스크립트
            JM_PlayerMove pm = trPanel.GetChild(maxVoteIndex).gameObject.GetComponent<SH_PlayerPanel>().photonView.GetComponent<JM_PlayerMove>();
            // 임포스터인 경우
            if (pm.isImposter)
            {
                saveVoteResult = $"{pm.nickName.text}님은 임포스터였습니다.";
                print("뽑힌 사람 죽이자~~~~");
                // 뽑힌 사람
                if (pm.photonView.IsMine)
                {
                    pm.GetComponent<JM_PlayerMove>().ToGhost();
                }
                
                else
                {
                    p = pm.photonView;
                    pm.gameObject.SetActive(false);
                    print("없앴음");
                    print("죽었니? : " + pm.gameObject.activeSelf);
                    print("현재 실행 함수 : " + MethodBase.GetCurrentMethod().Name);
                }
                
            }
            // 임포스터 아닌 경우
            else
            {
                saveVoteResult = $"{pm.nickName.text}님은 임포스터가 아니었습니다.";
                // 뽑힌 사람
                if (pm.photonView.IsMine)
                {
                    pm.GetComponent<JM_PlayerMove>().ToGhost();
                }
                else
                {
                    pm.gameObject.SetActive(false);
                }
            }
        }


        // 투표 결과 발표
        voteTitle.text = "투표 결과";
        StartCoroutine("ShowVoteResult");
    }
    IEnumerator ShowVoteResult()
    {
        // 투표 결과 Text 애니메이션 효과
        float t = 0;
        float size = 5;
        float upSizeTime = 0.3f;
        while (t <= upSizeTime)
        {
            t += Time.deltaTime;
            voteTitle.transform.localScale = Vector3.one * (1 + size * t);
            yield return null;
        }
        t = 0;
        while (t <= upSizeTime * 2)
        {
            t += Time.deltaTime;
            voteTitle.transform.localScale = Vector3.one * (2 * size * upSizeTime + 1 - size * t);
            yield return null;
        }
        voteTitle.transform.localScale = Vector3.one;
        //t += Time.deltaTime;


        // Array돌면서 VotingCrew의 자식으로 넣어주기
        panels = SH_RoomUI.instance.trPanels;
        for (int i = 0; i < panels.childCount; i++)
        {
            Transform tr = panels.GetChild(i);
            tr = tr.GetChild(4);
            GameObject votingCrew = panels.GetChild(i).GetChild(4).gameObject;
            for (int j = 0; j < voteResult[i]; j++)
            {
                GameObject vote = Instantiate(voteCheckCrewFactory);
                vote.transform.SetParent(tr, false); // VotingCrew의 자식으로 투표 받은 만큼 voteCheckCrew 프리팹 만들어 넣어주기
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2);

        // 투표 결과 타이핑 효과
        voteResultText.text = "";
        voteResultUI.SetActive(true);
        for (int i = 0; i < saveVoteResult.Length; i++)
        {
            voteResultText.text += saveVoteResult[i];
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(2);

        // 종료
        voteUI.SetActive(false);
        voteResultUI.SetActive(false);

        // 변수 초기화
        isVote = false;
        isOnce = false;
        voteCompleteNum = 0;
    }


    // 전체 패널 투표 버튼 비활성화 / 활성화 함수
    Button btnSKipVote;
    public void PanelOff()
    {
        trPanel = SH_RoomUI.instance.trPanels;
        foreach (Transform panel in trPanel)
        {
            panel.GetComponent<Button>().interactable = false;
            panel.GetChild(7).gameObject.SetActive(false);  // Btn_Vote
            panel.GetChild(8).gameObject.SetActive(false);  // Btn_VoteCancel
        }
        // 스킵 버튼 비활성화
        //btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        //btnSKipVote.gameObject.SetActive(false);
    }
    public void PanelOn()
    {
        trPanel = SH_RoomUI.instance.trPanels;
        foreach (Transform panel in trPanel)
        {
            panel.GetComponent<Button>().interactable = true;
        }
        // 스킵 버튼 비활성화
        //btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        //btnSKipVote.gameObject.SetActive(false);
    }
}