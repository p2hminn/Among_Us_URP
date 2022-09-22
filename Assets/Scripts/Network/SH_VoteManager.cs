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
    GameObject voteUI;
    // �÷��̾� �г� ������
    public GameObject playerPanelFactory;
    // Player panel ���� �θ� ��ü
    public Transform trPanel;


    void Start()
    {
        
    }

    
    void Update()
    {
        // ��ǥ UI Ȱ��ȭ
        if (isVote) voteUI.SetActive(true);
    }


    // �÷��̾� �г� ����
    void PlayerPanelSetting()
    {
        // �÷��̾ŭ �г� �����ϱ�
        foreach ( PhotonView photonView in JM_GameManager.instance.playerList )
        {
            GameObject panel = Instantiate(playerPanelFactory, trPanel);
            // panel �� ���� ����
            SH_PlayerPanel playerPanel = panel.GetComponent<SH_PlayerPanel>();
            playerPanel.SetInfo(photonView);
            // 10�� ������������ ��ư ��Ȱ��ȭ
            playerPanel.GetComponent<Button>().interactable = false;
        }
    }
}
