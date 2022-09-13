using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;  // ����Ƽ�� ���� ������Ʈ
using Photon.Realtime;  // ���� ���� ���� ���̺귯��

// ������ ���� ���� ���� �ÿ��� �¶��� ��ư Ȱ��ȭ

public class SH_ConnectionManager : MonoBehaviourPunCallbacks  // ���� ���񽺰� �߻���Ű�� �ݹ� ���� 
{
    // ���� ����
    private string gameVersion = "1";
    // ��Ʈ��ũ ���� ǥ�� �ؽ�Ʈ
    public Text connectionInfoText;
    // �¶��� ��ư
    public Button onlineButton;
    // �г���
    public Text nickName;


    private void Awake()
    {
        connectionInfoText = GameObject.Find("Text_ConnectionInfo").GetComponent<Text>();
    }
    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    void Start()
    {
        Connect();
    }

    void Update()
    {
        // �г��� ����
        PhotonNetwork.NickName = nickName.text;
    }

    void Connect()
    {
        PhotonNetwork.GameVersion = gameVersion;
        //PhotonNetwork.ConnectToRegion("kr");

        // ������ ���� ���� 
        PhotonNetwork.ConnectUsingSettings();

        // �¶��� ��ư ��� ��Ȱ��ȭ
        onlineButton.interactable = false;
        // ���� �õ� ���� �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "������ ������ ���� ��...";
    }


    // ������ ���� ���� ������ ���
    public override void OnConnectedToMaster()
    {
        // �¶��� ��ư Ȱ��ȭ
        onlineButton.interactable = true;
        // ���� ���� ǥ��
        connectionInfoText.text = "������ ������ �����";
    }
    // ������ ���� ���� ������ ���
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �¶��� ��ư ��Ȱ��ȭ
        onlineButton.interactable = false;
        // ���� ���� ǥ��
        connectionInfoText.text = "������ ������ ������� ����\n���� ��õ� ��...";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        //SH_ConnectionManager.connectionInfoText.text = "�κ� ���� �Ϸ�";
    }
}
