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
    // ȸ�� �� ��ǥ �ð� Text
    public Text txtVoteTime;
    // ȸ�� �ð�
    public float discussTime = 10;
    // ��ǥ �ð�
    public float voteTime = 120;
    // ���� �÷��̾��� �г� idx
    public int localPanelIdx;
    public PhotonView p;


    bool isOnce;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("��ǥ�� ��� : " + voteCompleteNum);
        }

        if (p)
        {
            print("�׾���? 2 : " + p.gameObject.activeSelf);
            print("���� ���� �Լ�2 : " + MethodBase.GetCurrentMethod().Name);
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
        }  // �г�


        // ��ǥ ����
        voteUI.SetActive(true);
        // �÷��̾ŭ �г� �����ϱ�
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel ������Ʈ �̸��� �÷��̾� �̸����� ����
            playerPanel.gameObject.name = JM_GameManager.instance.playerList[i].Owner.NickName;
            // panel �� ���� ����
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i], reportViewID);
            // ���� �÷��̾��� �г��� �� ��°���� ǥ��
            if (JM_GameManager.instance.playerList[i].IsMine)
            {
                localPanelIdx = i;
                print("myPanelIndex : " + i);
            }
            // ���� ũ�� ��ǥ�ߴٰ� ġ�� (RPC�� �������� ��������)
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost")) voteCompleteNum++;
        }


        // ȸ�� �ð�
        StartCoroutine("StartDiscuss");


    }
    IEnumerator StartDiscuss()
    {
        PanelOff();



        float currTime = 0;

        while (currTime < discussTime)
        {
            currTime += Time.deltaTime;
            txtVoteTime.text = $"ȸ�� �ð� : {(int)discussTime - (int)currTime}��";
            yield return null;
        }
        txtVoteTime.text = "";
        // ��ǥ ����
        PanelOn();
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
    public string saveVoteResult;  // ��ǥ ��� ������ ����
    public Text voteResultText;  // ��ǥ ��� UI�� �Է��� Text
    public GameObject voteResultUI; // ��ǥ ��� UI
    public void VoteResult()
    {
        // ��ǥ ��� ���ϱ�
        // ���� ��ǥ ���� ���� ����� ��ǥ��
        int maxVote = voteResult.Max();
        maxVoteIndex = voteResult.ToList().IndexOf(maxVote);
        // �ִ� ��ǥ�� �� ���ϱ�
        for (int i = 0; i < voteResult.Length; i++)
        {
            if (voteResult[i] == maxVote)
            {
                maxVoteNum++;
            }
        }
        // �ִ� ��ǥ�ڰ� �ټ��� ��� ( ������ �߻��� ��� )
        if (maxVoteNum > 1)
        {
            saveVoteResult = "�ƹ��� ������� �ʾҽ��ϴ�. (���� �߻�)";
        }
        // �Ѹ� �ִ� ��ǥ�� ���
        else
        {
            // �ִ� ��ǥ �÷��̾��� ��ũ��Ʈ
            JM_PlayerMove pm = trPanel.GetChild(maxVoteIndex).gameObject.GetComponent<SH_PlayerPanel>().photonView.GetComponent<JM_PlayerMove>();
            // ���������� ���
            if (pm.isImposter)
            {
                saveVoteResult = $"{pm.nickName.text}���� �������Ϳ����ϴ�.";
                print("���� ��� ������~~~~");
                // ���� ���
                if (pm.photonView.IsMine)
                {
                    pm.GetComponent<JM_PlayerMove>().ToGhost();
                }
                
                else
                {
                    p = pm.photonView;
                    pm.gameObject.SetActive(false);
                    print("������");
                    print("�׾���? : " + pm.gameObject.activeSelf);
                    print("���� ���� �Լ� : " + MethodBase.GetCurrentMethod().Name);
                }
                
            }
            // �������� �ƴ� ���
            else
            {
                saveVoteResult = $"{pm.nickName.text}���� �������Ͱ� �ƴϾ����ϴ�.";
                // ���� ���
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


        // ��ǥ ��� ��ǥ
        voteTitle.text = "��ǥ ���";
        StartCoroutine("ShowVoteResult");
    }
    IEnumerator ShowVoteResult()
    {
        // ��ǥ ��� Text �ִϸ��̼� ȿ��
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


        // Array���鼭 VotingCrew�� �ڽ����� �־��ֱ�
        panels = SH_RoomUI.instance.trPanels;
        for (int i = 0; i < panels.childCount; i++)
        {
            Transform tr = panels.GetChild(i);
            tr = tr.GetChild(4);
            GameObject votingCrew = panels.GetChild(i).GetChild(4).gameObject;
            for (int j = 0; j < voteResult[i]; j++)
            {
                GameObject vote = Instantiate(voteCheckCrewFactory);
                vote.transform.SetParent(tr, false); // VotingCrew�� �ڽ����� ��ǥ ���� ��ŭ voteCheckCrew ������ ����� �־��ֱ�
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2);

        // ��ǥ ��� Ÿ���� ȿ��
        voteResultText.text = "";
        voteResultUI.SetActive(true);
        for (int i = 0; i < saveVoteResult.Length; i++)
        {
            voteResultText.text += saveVoteResult[i];
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(2);

        // ����
        voteUI.SetActive(false);
        voteResultUI.SetActive(false);

        // ���� �ʱ�ȭ
        isVote = false;
        isOnce = false;
        voteCompleteNum = 0;
    }


    // ��ü �г� ��ǥ ��ư ��Ȱ��ȭ / Ȱ��ȭ �Լ�
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
        // ��ŵ ��ư ��Ȱ��ȭ
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
        // ��ŵ ��ư ��Ȱ��ȭ
        //btnSKipVote = GameObject.Find("Btn_SkipVote").GetComponent<Button>();
        //btnSKipVote.gameObject.SetActive(false);
    }
}