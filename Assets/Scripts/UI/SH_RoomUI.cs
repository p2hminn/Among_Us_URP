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

    bool isSelectionUI;
    public bool isGameScene;
    public GameObject gameMap;

    // ���� �÷��̾ ������������ �ƴ��� ����
    public bool isLocalImposter;

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

        if (isStart)
        {
            JM_GameIntro();
        }
        if (isSelectionUI)
        {
            JM_ShowPlayerRole();
        }
        if (isGameScene)
        {
            JM_GameEnable();
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

        /*
        if (isStart && JM_PlayerMove.instance.introStart)
        {
            // Start ��ư ���� ��� ���� ��Ʈ�� ����
            StartCoroutine("GameIntro");
            JM_PlayerMove.instance.introStart = false;
        }
        */
    }

    float currentTime = 0;

    void JM_GameIntro()
    {
        shhh.SetActive(true);
        float delayTime = 3;
        currentTime += Time.deltaTime;
        if (currentTime > delayTime)
        {
            shhh.SetActive(false);
            isSelectionUI = true;
            isStart = false;
            currentTime = 0;
        }
    }

    void JM_ShowPlayerRole()
    {
        currentTime += Time.deltaTime;
        float delayTime = 3;
        // �������Ͷ��
        if (isLocalImposter)
        {
            // �������� ui ����
            imposters.SetActive(true);
        }
        // ũ����
        else
        {
            // ũ�� ui ����
            crews.SetActive(true);
        }

        if (currentTime > delayTime)
        {
            isGameScene = true;
            isSelectionUI = false;
            // �������Ͷ�� �������� ���ְ� ũ���� ũ�� ����
            if (isLocalImposter)
                imposters.SetActive(false);
            else if (!isLocalImposter)
                crews.SetActive(false);
            cam.gameObject.SetActive(true);

            currentTime = 0;
        }
    }
    void JM_GameEnable()
    {
        gameMap.SetActive(true);
    }


    // GameIntro �ڷ�ƾ
    //IEnumerator GameIntro()
    //{
    //    //yield return new WaitForSeconds(2);
    //    float currTime = 0;
    //    float delayTime = 5000;
    //    shhh.SetActive(true);
    //    while (currTime < delayTime)
    //    {
    //        currTime += introSpeed * Time.deltaTime;
    //        print($"currTime : {currTime}");
    //    }
    //    shhh.SetActive(false);

    //    // ũ���� ���
    //    if (!JM_PlayerMove.instance.isImposter)
    //    {
    //        float a = 0;
    //        crews.SetActive(true);
    //        CanvasGroup crewsAlpha = crews.GetComponent<CanvasGroup>();
    //        // ���࿡ �����̶�� player ������Ʈ Ȱ��ȭ�ϱ�
    //        if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
    //        while (a < 1)
    //        {
    //            print($"crewsAlpha : {a}");
    //            a += introSpeed * Time.deltaTime;
    //            crewsAlpha.alpha = a;
    //            yield return null;
    //        }
    //        crewsAlpha.alpha = 1;
    //        yield return new WaitForSeconds(2);
    //        crews.SetActive(false);
    //    }

    //    // ���������� ���
    //    else
    //    {
    //        float a = 0;
    //        imposters.SetActive(true);
    //        CanvasGroup impostersAlpha = imposters.GetComponent<CanvasGroup>();
    //        if (JM_PlayerMove.instance.photonView.IsMine) JM_PlayerMove.instance.gameObject.SetActive(true);
    //        while (a < 1)
    //        {
    //            print($"impostersAlpha : {a}");
    //            a += introSpeed * Time.deltaTime;
    //            impostersAlpha.alpha = a;
    //            yield return null;
    //        }
    //        impostersAlpha.alpha = 1;
    //        yield return new WaitForSeconds(2);
    //        imposters.SetActive(false);
    //    }

    //    yield return new WaitForSeconds(2);


    //}
}