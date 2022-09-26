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


    // ��ǥ ���� ���� 
    public bool isVote;
    // ��ǥ UI
    public GameObject voteUI;
    // �÷��̾� �г� ������
    public GameObject playerPanelFactory;
    // Player panel ���� �θ� ��ü
    public Transform trPanel;
    // ����Ʈ�� ��� ����� ID
    public int reportViewID;
    // ��ǥ ��� (������ ����) : �г� �ε���, ��ǥ��
    public int[] voteResult;// �ε��� : �� ��° �г�����, int : �� ǥ�� �޾Ҵ���
    public int voteCompleteNum;  // ��� ��ǥ �Ϸ��ߴ��� �˱� ���� �� ����

    bool isOnce;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("��ǥ�� ��� : " + voteCompleteNum);
        }
        

        // ��� ��ǥ�Ϸ��ϸ� ��ο��� ��ǥ ��� �����ֱ�
        if (voteCompleteNum == PhotonNetwork.CurrentRoom.PlayerCount && !isOnce && JM_GameManager.instance.isGameRoom)
        {
            isOnce = true;
            VoteResult();
        }
    }

    // �÷��̾� �г� ����
    public void PlayerPanelSetting()
    {
        isVote = true;
        // �ʱ�ȭ
        for (int i=0; i < voteResult.Length; i++)
        {
            voteResult[i] = 0;
        }
        voteCompleteNum = 0;
        isOnce = false;

        // ��ǥ UI Ȱ��ȭ
        voteUI.SetActive(true);
        // �÷��̾ŭ �г� �����ϱ�
        for ( int i=0; i < JM_GameManager.instance.playerList.Count; i++ )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel �� ���� ����
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i], reportViewID);
            // ���� ũ�� ��ǥ�ߴٰ� ġ�� (RPC�� �������� ��������)
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) voteCompleteNum++;
        }
        // ���� ũ���� ��� ��� �г� ��ư ��Ȱ��ȭ�ؼ� ��ǥ ���ϰ� �ϱ�
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            if (JM_GameManager.instance.playerList[i].IsMine && JM_GameManager.instance.playerList[i].CompareTag("Ghost"))
            {
                PanelOff();
            }
        }
    }

    // ��ǥ ��� ��ǥ
    /*
     1. �� �гε��� ��ǥȮ�� ��ư�� ������ ������ VoteManager���� ��ǥ ��� ����
     2. ������ VoteManager�� ��ΰ� ��ǥ�Ϸ��� ��� RPC�� ��ο��� ��ǥ����� �˷��ش�.
     3. �߹� UI �����ְ� �ٽ� ���� ����
     */
    public Text voteTitle;
    public GameObject voteCheckCrewFactory;
    public Transform panels;
    public int maxVoteIndex;  // �ִ� ��ǥ���� �ε���
    int maxVoteNum;  // �ִ� ��ǥ�� �� 
    public Text voteResultText;  // ��ǥ ��� UI�� �Է��� Text
    public GameObject voteResultUI; // ��ǥ ��� UI
    public void VoteResult()
    {
        voteTitle.text = "��ǥ ���";
        panels = GameObject.FindGameObjectsWithTag("Panels")[0].transform;
        // Array���鼭 VotingCrew�� �ڽ����� �־��ֱ�
        for (int i=0; i < panels.childCount; i++)
        {
            GameObject votingCrew = panels.GetChild(i).GetChild(4).gameObject;
            for (int j=0; j < voteResult[i]; j++)
            {
                GameObject vote = Instantiate(voteCheckCrewFactory);
                vote.transform.SetParent(panels, false); // VotingCrew�� �ڽ����� ��ǥ ���� ��ŭ voteCheckCrew ������ ����� �־��ֱ�
            }
        }

        // ���� ��ǥ ���� ���� ��� 
        int maxVote = voteResult.Max();
        maxVoteIndex = voteResult.ToList().IndexOf(maxVote);
        // �ִ� ��ǥ�� �� ���ϱ�
        for (int i=0; i<voteResult.Length; i++)
        {
            if (voteResult[i] == maxVote)
            {
                maxVoteNum++;
            }
        }


        // �ִ� ��ǥ�ڰ� �ټ��� ���
        if (maxVoteNum > 1)
        {
            voteResultText.text = "�ƹ��� ������� �ʾҽ��ϴ�. (�ǳʶ�)";
        }
        // �Ѹ� �ִ� ��ǥ�� ���
        else
        {
            // �ִ� ��ǥ �÷��̾��� ��ũ��Ʈ
            JM_PlayerMove pm = trPanel.GetChild(maxVoteIndex).gameObject.GetComponent<SH_PlayerPanel>().photonView.GetComponent<JM_PlayerMove>();
            // ���������� ���
            if (pm.isImposter)
            {
                voteResultText.text = $"{pm.nickName}���� �������Ϳ����ϴ�.";
            }
            // �������� �ƴ� ���
            else
            {
                voteResultText.text = $"{pm.nickName}���� �������Ͱ� �ƴϾ����ϴ�.";
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

    // ��ü �г� ��ǥ ��ư ��Ȱ��ȭ
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
        // ��ŵ ��ư ��Ȱ��ȭ
        //btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        //btnSKipVote.gameObject.SetActive(false);
        print("�� ��Ȱ��ȭ��");
    }
}
