using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_ChatManager : MonoBehaviourPun
{
    // InputChat
    public InputField inputChat;
    // ChatItem ����
    public GameObject chatItemFactory;
    // ScrollView�� Content
    public Transform trContent;
    // ���̵��
    Color idColor;

    void Start()
    {
        // InputField�� Text �Է� �� Enter�� ġ�ų� ȭ��ǥ ��ư�� ���� ��� ä�ö� ����
        inputChat.onSubmit.AddListener(OnSubmit);
    }

    // ��ư ������ ä�� ����
    public void OnSendChat()
    {

    }


    // InputField���� Enter���� �� ȣ��
    public void OnSubmit(string s)
    {
        // <color=#FFFFFF> �г��� </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.white) + ">" + PhotonNetwork.NickName + "</color>" + ":" + s;
        // ä�� ���� ����ȭ
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        // InputChat ���� �ʱ�ȭ
        inputChat.text = "";
        // InputChat�� Focusing
        inputChat.ActivateInputField();
    }
    [PunRPC]
    void RpcAddChat(string chatText)  // ���� Text�� ������� �Ѵ�.
    {
        // content �ڽ����� ChatItem �����
        GameObject item = Instantiate(chatItemFactory, trContent);
        SH_ChatItem chat = item.GetComponent<SH_ChatItem>();
        chat.SetText(chatText);
    }
}
