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
    // ��ǥ ��� (������ ����)
    public Dictionary<GameObject, int> voteResultDic = new Dictionary<GameObject, int>();
    public int voteCompleteNum;

    bool isOnce;
    private void Update()
    {
        print("��ǥ�� ��� : " + voteCompleteNum);

        // ��� ��ǥ�Ϸ��ϸ� ��ο��� ��ǥ ��� �����ֱ�
        if (voteCompleteNum == JM_GameManager.instance.playerList.Count && !isOnce)
        {
            isOnce = true;
            photonView.RPC("VoteResult", RpcTarget.All);
        }
    }

    // �÷��̾� �г� ����
    public void PlayerPanelSetting()
    {
        isVote = true;
        // �ʱ�ȭ
        voteResultDic.Clear();
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
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) 
            // �Ű��� ��� ǥ���ϱ�
            //if (JM_GameManager.instance.playerList[i].ViewID == reportViewID) reportImg.gameObject.SetActive(true);
            if (reportViewID != 0) reportViewID = 0;
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

    [PunRPC]
    public void SendGhostVote()
    {
        voteCompleteNum++;
    }

    // ��ǥ ��� ��ǥ
    /*
     1. �� �гε��� ��ǥȮ�� ��ư�� ������ ������ VoteManager���� ��ǥ ��� ����
     2. ������ VoteManager�� ��ΰ� ��ǥ�Ϸ��� ��� RPC�� ��ο��� ��ǥ����� �˷��ش�.
     3. �߹� UI �����ְ� �ٽ� ���� ����
     */
    [PunRPC]
    public void VoteResult()
    {

        print("��ǥ ��� ��������~");
        // ��ǥ ��
        //voteUI.SetActive(false);
        //isVote = false;
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
        btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        btnSKipVote.gameObject.SetActive(false);
        print("�� ��Ȱ��ȭ��");
    }
}
