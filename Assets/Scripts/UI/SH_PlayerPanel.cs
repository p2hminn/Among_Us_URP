using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviourPun
{
    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    public Image blackImg;
    public Image reportImg;

    // �г� �� ���� ���� 
    public void SetInfo(PhotonView photonView, int reportViewID = 0)
    {
        // ���� ũ���� ���
        if (photonView.gameObject.CompareTag("Ghost"))
        {
            // ��ư ��Ȱ��ȭ
            GetComponent<Button>().interactable = false;
            // ���� ǥ�� 
            diedImg.gameObject.SetActive(true);
            // ���̹��� Ȱ��ȭ
            blackImg.gameObject.SetActive(true);
        }

        // �÷��̾� ����
        Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
        // ���� ������ ��Ƽ���� ���� �ֱ�
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // �÷��̾� �г���
        NickNametxt.GetComponent<Text>().text = photonView.Owner.NickName;

        // �Ű��� ��� 
        if (reportViewID == photonView.ViewID) photonView.RPC("RPC_SetPanel", RpcTarget.All, reportViewID);
    }
    

    // �÷��̾� ��ǥ �гΰ� ���õ� ��� ���� ����ȭ
    [PunRPC]
    void RPC_SetPanel()
    {
        // ����Ʈ�� ��� ǥ��
        reportImg.gameObject.SetActive(true);
    }


    public Button btnVote;
    public Button btnVoteCancel;

    // �÷��̾� �г� Ŭ���� �� ��ǥ ��ư ������ �ϱ�
    public int n;
    public void OnClickPanel()
    {
        // ��ǥ �Ϸ����� ��쿡�� ��ǥ ��ư �ȳ����� �ϱ�
        if (voteComplete) return;

        Transform trPanel = GameObject.FindGameObjectsWithTag("Panels")[0].transform;

        
        // ���� ������ �г��� �ƴ� ��� ������ �гε��� ��ǥ ��ư ��Ȱ��ȭ
        foreach (Transform panel in trPanel)
        {
            if (panel.gameObject != gameObject)
            {
                panel.GetChild(7).gameObject.SetActive(false);
                panel.GetChild(8).gameObject.SetActive(false);
            }
            else
            {
                panel.GetChild(7).gameObject.SetActive(true);
                panel.GetChild(8).gameObject.SetActive(true);
            }
        }
    }

    
    public Image voteForImg;
    public Image votedImg;
    bool voteComplete;
    // ��ǥ Ȯ�� ��ư ���� ��� 
    public void OnClickVote()
    {
        voteComplete = true;
        // ��ǥ�Ϸ��ϸ�  ��� �гε� �ڱ� �ڽ��� ��ư interactable ��Ȱ��ȭ
        //GetComponent<Button>().interactable = false;

        // ��ǥ �Ϸ� �̹��� Ȱ��ȭ + ����ȭ
        photonView.RPC("SendVoted", RpcTarget.All);

        // �ڽ��� ��ǥ ����� MasterClient VoteManager���� ������
        photonView.RPC("SendVoteResult", RpcTarget.MasterClient, gameObject);

        // VoteFor �̹��� Ȱ��ȭ
        voteForImg.gameObject.SetActive(true);

    }
    // ��ο��� ��ǥ�Ϸ����� ǥ��
    [PunRPC]
    public void SendVoted()
    {
        // ��ǥ ��ư ��Ȱ��ȭ + ����ȭ
        btnVote.gameObject.SetActive(false);
        btnVoteCancel.gameObject.SetActive(false);
        votedImg.gameObject.SetActive(true);
    }
    // MasterClient���Ը� ��ǥ ��� ������
    [PunRPC]
    public void SendVoteResult(GameObject g)
    {
        SH_VoteManager.instance.voteResultDic[g] += 1;
        SH_VoteManager.instance.voteCompleteNum++;
    }


    // ��ǥ ��� ��ư ���� ���
    public void OnClickVoteCancel()
    {
        btnVote.gameObject.SetActive(false);
        btnVoteCancel.gameObject.SetActive(false);
    }

}
