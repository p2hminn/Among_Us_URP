using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SH_RoomUI : MonoBehaviourPunCallbacks
{
    // singleton
    public static SH_RoomUI instance;
    private void Awake()
    {
        instance = this;
    }
    // Start ��ư
    public Button btn_Start;
    // �� �̸� Text
    public Text txt_RoomName;
    // ���� ���� �ο� / �ִ� ���� ���� �ο� Text
    public Text txt_PlayerNum;

    void Start()
    {
        // ���̸� UI text
        txt_RoomName.text = PhotonNetwork.CurrentRoom.Name;

        // ������ ��쿡�� Start ��ư Ȱ��ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            btn_Start.gameObject.SetActive(true);
        }
    }

    
    void Update()
    {
        // ���� ���� �ο��� 4���̰� ������ ��쿡  Start ��ư  interactable Ȱ��ȭ
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4 && PhotonNetwork.IsMasterClient)
        {
            btn_Start.interactable = true;
        }
    }

    // �÷��̾ �濡 ���� �� & ���� �� �� �ο� �� ������Ʈ
    public override void OnPlayerEnteredRoom(Player newPlayer) => PlayerNumUpdate();
    public override void OnPlayerLeftRoom(Player otherPlayer) => PlayerNumUpdate();
    // ���� ���� �ο� �� Text ������Ʈ
    public void PlayerNumUpdate()
    {
        // ���� �ο� / ���� �ִ� �ο� UI text ������Ʈ
        txt_PlayerNum.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
    
}
