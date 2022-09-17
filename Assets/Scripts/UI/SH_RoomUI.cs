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
    // ���� ���� Start ��ư ���� ����
    public bool isStart = false;
    // ���� Start ��ư ���� �� ��Ȱ��ȭ�ؾ� �ϴ� ������Ʈ��
    [Header("GameStart �� ��Ȱ��ȭ�ؾ� �ϴ� ������Ʈ")]
    public List<GameObject> toOff = new List<GameObject>();
    // GameIntro UI ī�޶� 
    public Camera cam;
    // GameIntro UI
    public GameObject shhh;
    public GameObject crews;
    public GameObject imposters;
    public float introSpeed;

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
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && PhotonNetwork.IsMasterClient)
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



    // ������ Start��ư ���� ���
    public void OnClickStart()
    {
        // Start��ư ���ȴٰ� RPC �����ֱ�
        photonView.RPC("GameIntroStart", RpcTarget.All);
    }

    [PunRPC]
    void GameIntroStart()
    {
        isStart = true;
        // UI ���̰��� ī�޶� Ȱ��ȭ
        cam.gameObject.SetActive(true);
        // �ش� ����Ʈ ���� ��� ������Ʈ�� ��Ȱ��ȭ��Ű��
        for (int i = 0; i < toOff.Count; i++)
        {
            toOff[i].SetActive(false);
        }

        if (isStart)
        {
            // Start ��ư ���� ��� ���� ��Ʈ�� ����
            if (isStart)
            {
                StartCoroutine("GameIntro");
            }
        }
    }
    

    // GameIntro �ڷ�ƾ
    IEnumerator GameIntro()
    {
        //yield return new WaitForSeconds(2);

        float currTime = 0;
        float delayTime = 2;
        while (currTime < delayTime)
        {
            shhh.SetActive(true);
            currTime += introSpeed * Time.deltaTime;
            print($"currTime : {currTime}");
        }
        shhh.SetActive(false);

        // ũ���� ���
        if (!JM_PlayerMove.instance.isImposter)
        {
            float a = 0;
            CanvasGroup crewsAlpha = crews.GetComponent<CanvasGroup>();
            // ���࿡ �����̶�� player ������Ʈ Ȱ��ȭ�ϱ�
            if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
            while (a < 1)
            {
                print($"crewsAlpha : {a}");
                a += introSpeed * Time.deltaTime;
                crewsAlpha.alpha = a;
            }
            crewsAlpha.alpha = 1;
        }
        // ���������� ���
        else
        {
            float a = 0;
            CanvasGroup impostersAlpha = imposters.GetComponent<CanvasGroup>();
            if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
            while (a < 1)
            {
                print($"impostersAlpha : {a}");
                a += introSpeed * Time.deltaTime;
                impostersAlpha.alpha = a;
            }
            impostersAlpha.alpha = 1;
        }
        yield return new WaitForSeconds(2);


    }
}
