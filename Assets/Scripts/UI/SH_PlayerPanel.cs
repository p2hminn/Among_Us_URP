using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviour
{
    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    public Image blackImg;
    public Image reportImg;
    public Transform trPanel;
    public int playerViewId;
    public PhotonView photonView;
    public int localPanelIdx;


    // �г� �� ���� ���� 
    public void SetInfo(PhotonView pv, int reportViewID)
    {
        playerViewId = pv.ViewID;
        photonView = pv;
        // ���� ũ���� ���
        if (pv.gameObject.CompareTag("Ghost"))
        {
            // ��ư ��Ȱ��ȭ
            GetComponent<Button>().interactable = false;
            // ���� ǥ�� 
            diedImg.gameObject.SetActive(true);
            // ���̹��� Ȱ��ȭ
            blackImg.gameObject.SetActive(true);
        }

        // �÷��̾� ����
        Color color = pv.gameObject.GetComponent<JM_PlayerMove>().color;
        // ���� ������ ��Ƽ���� ���� �ֱ�
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // �÷��̾� �г���
        NickNametxt.GetComponent<Text>().text = pv.Owner.NickName;

        // �Ű��� ��� 
        if (reportViewID == pv.ViewID) //&& pv.IsMine)
        {
            transform.GetChild(9).gameObject.SetActive(true);  // Img_Report
            //pv.RPC("RPC_SetPanel", RpcTarget.All);
        }
        //print("reportViewID : " + reportViewID +"  /  pv.ViewID : " + pv.ViewID);

    }
    // �÷��̾� ��ǥ �гΰ� ���õ� ��� ���� ����ȭ
    //[PunRPC]
    //public void RPC_SetPanel()
    //{
    //    // ����Ʈ�� ��� ǥ��
    //    transform.GetChild(9).gameObject.SetActive(true);  // Img_Report
    //    print("SetPanel");
    //}



    // �÷��̾� �г� Ŭ���� �� ��ǥ ��ư ������ �ϱ�
    public void OnClickPanel()
    {
        // ��ǥ �Ϸ����� ��쿡�� ��ǥ ��ư �ȳ����� �ϱ�
        if (voteComplete) return;

        // ���� ������ �г��� �ƴ� ��� ������ �гε��� ��ǥ ��ư ��Ȱ��ȭ
        trPanel = SH_RoomUI.instance.trPanels;
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

    
    
    bool voteComplete;
    // ��ǥ Ȯ�� ��ư ���� ��� 
    public void OnClickVote()
    {
        //print(gameObject.transform.GetChild(1).GetComponent<Text>().text);  => ���� Ȯ�� ��ư�� �޷��ִ� �г�
        voteComplete = true;
        // ��ǥ�Ϸ��ϸ�  ��� �гε� ��ǥ ��ư ��Ȱ��ȭ
        SH_VoteManager.instance.PanelOff();

        // ���� �÷��̾ ǥ���� �г��� ��ǥ �Ϸ� �̹��� Ȱ��ȭ + ����ȭ
        photonView.RPC("RPC_SendVoted", RpcTarget.All, localPanelIdx);

        // �ڽ��� ��ǥ ����� VoteManager���� ������
        photonView.RPC("RPC_SendVoteResult", RpcTarget.All, transform.GetSiblingIndex());

        // VoteFor �̹��� Ȱ��ȭ
        transform.GetChild(5).gameObject.SetActive(true);

    }
    

    // ��ǥ ��� ��ư ���� ���
    public void OnClickVoteCancel()
    {
        transform.GetChild(7).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
    }
}
