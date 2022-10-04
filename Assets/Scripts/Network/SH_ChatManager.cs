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
    public GameObject warning;
    public void OnSendChat()
    {
        // ���� ����� ��� ä�� �Ұ�
        if (JM_GameManager.instance.localPv.gameObject.CompareTag("Ghost"))
        {
            inputChat.text = "";
            StartCoroutine(warn());
            return;
        }
        // <color=#FFFFFF> �г��� </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + "  :  " + inputChat.text;
        // ä�� ���� ����ȭ
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        // InputChat ���� �ʱ�ȭ
        inputChat.text = "";
        // InputChat�� Focusing
        inputChat.ActivateInputField();
    }
    


    // InputField���� Enter���� �� ȣ��
    public void OnSubmit(string s)
    {
        // ���� ����� ��� ä�� �Ұ�
        if (JM_GameManager.instance.localPv.gameObject.CompareTag("Ghost"))
        {
            inputChat.text = "";
            StartCoroutine(warn());
            return;
        }
        // <color=#FFFFFF> �г��� </color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.blue) + ">" + PhotonNetwork.NickName + "</color>" + "  :  " + s;
        // ä�� ���� ����ȭ
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        // InputChat ���� �ʱ�ȭ
        inputChat.text = "";
        // InputChat�� Focusing
        inputChat.ActivateInputField();
    }
    IEnumerator warn()
    {
        warning.SetActive(true);
        yield return new WaitForSeconds(2);
        warning.SetActive(false);
    }
    //// ���� Content�� H
    //float prevContentH;
    //// ScrollView�� RectTransform 
    //public RectTransform trScrollView;

    [PunRPC]
    void RpcAddChat(string chatText)  // ���� Text�� ������� �Ѵ�.
    {
        //prevContentH = trContent.sizeDelta.y;

        // content �ڽ����� ChatItem �����
        GameObject item = Instantiate(chatItemFactory, trContent);
        SH_ChatItem chat = item.GetComponent<SH_ChatItem>();
        chat.SetText(chatText);

        //StartCoroutine(AutoScrollBottom();
    }

    //IEnumerator AutoScrollBottom()
    //{
    //    yield return null;

    //    // trScrollView H ���� Content H���� Ŀ���� (��ũ�� ���ɻ���)
    //    if (trContent.sizeDelta.y > trScrollView.sizeDelta.y)
    //    {
    //        // content�� �ٴڿ� ����־��ٸ�
    //        if (trContent.anchoredPosition.y >= prevContentH - trScrollView.sizeDelta.y)
    //        {
    //            // Content�� y�� �ٽ� ����
    //            trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trScrollView.sizeDelta.y);
    //        }
    //    }
    //}
}
