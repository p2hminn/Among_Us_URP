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
    // ChatItem 공장
    public GameObject chatItemFactory;
    // ScrollView의 Content
    public Transform trContent;
    // 아이디색
    Color idColor;

    void Start()
    {
        // InputField에 Text 입력 후 Enter를 치거나 화살표 버튼을 누를 경우 채팅란 생성
        inputChat.onSubmit.AddListener(OnSubmit);
    }

    // 버튼 누르면 채팅 생성
    public void OnSendChat()
    {
        // <color=#FFFFFF> 닉네임 </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + "  :  " + inputChat.text;
        // 채팅 내용 동기화
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        // InputChat 내용 초기화
        inputChat.text = "";
        // InputChat에 Focusing
        inputChat.ActivateInputField();
    }


    // InputField에서 Enter쳤을 때 호출
    public void OnSubmit(string s)
    {
        // <color=#FFFFFF> 닉네임 </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + "  :  " + s;
        // 채팅 내용 동기화
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        // InputChat 내용 초기화
        inputChat.text = "";
        // InputChat에 Focusing
        inputChat.ActivateInputField();
    }
    [PunRPC]
    void RpcAddChat(string chatText)  // 받은 Text로 보내줘야 한다.
    {
        // content 자식으로 ChatItem 만들기
        GameObject item = Instantiate(chatItemFactory, trContent);
        SH_ChatItem chat = item.GetComponent<SH_ChatItem>();
        chat.SetText(chatText);
    }
}
