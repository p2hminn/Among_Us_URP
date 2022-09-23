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



    // ��ǥ ���� ���� 
    public bool isVote;
    // ��ǥ UI
    public GameObject voteUI;
    // �÷��̾� �г� ������
    public GameObject playerPanelFactory;
    // Player panel ���� �θ� ��ü
    public Transform trPanel;



    // �÷��̾� �г� ����
    public void PlayerPanelSetting()
    {
        isVote = true;
        voteUI.SetActive(true);
        // �÷��̾ŭ �г� �����ϱ�
        for ( int i=0; i <  JM_GameManager.instance.playerList.Count; i++ )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            // panel �� ���� ����
            playerPanel.SetInfo(JM_GameManager.instance.playerList[i]);
            // ���� ũ���� ��� ��ǥ ���ϵ��� ����
            if (JM_GameManager.instance.playerList[i].CompareTag("Ghost"))
            {

            }
            //playerPanel.GetComponent<Button>().interactable = false;
        }
    }
}
