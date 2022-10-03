using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEngine.Rendering.Universal;

public class SH_RoomUI : MonoBehaviourPunCallbacks
{
    // singleton
    public static SH_RoomUI instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject light;

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

    // ���� ��������UI �ڵ� �� ũ��UI �ڵ�
    JM_ImposterUI imposterUICode;
    JM_CrewUI crewUICode;

    public GameObject imposterGameUI;
    public GameObject crewGameUI;
    public GameObject reportUI;
    public GameObject diedCrew;
    public GameObject voteUI;
    public GameObject btnEmergency;
    public Button btnCrewUse;
    public Transform trPanels;

    // ��ü ����
    public Color dieColor;

    public bool isEmergency;
    public bool isChat;

    void Start()
    {
        // ���̸� UI text
        txt_RoomName.text = PhotonNetwork.CurrentRoom.Name;

        // ������ ��쿡�� Start ��ư Ȱ��ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            btn_Start.gameObject.SetActive(true);
        }

        // ũ��UI ��������UI �����Ҷ��� �Ѵ� �����ִ� ���� ���Ŀ� �������� �Ѿ�� ���� �Ǵ��ؼ� Ų��
        imposterUICode = GetComponent<JM_ImposterUI>();
        crewUICode = GetComponent<JM_CrewUI>();
        imposterUICode.enabled = false;
        crewUICode.enabled = false;

        // ���ǿ� �������� ��ο�� ����
        light.GetComponent<Light2D>().intensity = 1;
    }

    bool isOnce;
    void Update()
    {
        if (isLocalImposter) print("RoomUI recognizes I am imposter");

        // ���� ���� �ο��� 4���̰� ������ ��쿡  Start ��ư  interactable Ȱ��ȭ
        if (PhotonNetwork.CurrentRoom.PlayerCount ==  1 && PhotonNetwork.IsMasterClient)
        {
            btn_Start.interactable = true;
        }

        // ���� ��Ʈ��
        if (isStart)
        {
            JM_GameIntro();
        }
        if (isSelectionUI)
        {
            JM_ShowPlayerRole();
        }
        if (isGameScene && !isOnce)
        {
            JM_GameEnable();
            isOnce = true;
        }

        //if (SH_VoteManager.instance.p)
        //{
        //    print("�׾���? 2 : " + SH_VoteManager.instance.p.gameObject.activeSelf);
        //    print("���� ���� �Լ�2_RoomUI : " + MethodBase.GetCurrentMethod().Name);
        //}

        // Vote UI Ȱ��ȭ ������ üũ
        if (voteUI.activeSelf)
        {
            isChat = true;
        }
        else
        {
            isChat = false;
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

    public float startSeconds = 5;
    public Text txtStartCount;

    // ������ Start��ư ���� ���
    public void OnClickStart()
    {
        // Start��ư ���ȴٰ� RPC �����ֱ�
        photonView.RPC("GameIntroStart", RpcTarget.All);
    }

    [PunRPC]
    void GameIntroStart()
    {
        // ���� ���� ī��Ʈ
        txtStartCount.gameObject.SetActive(true);
        StartCoroutine("StartCount");
    }
    IEnumerator StartCount()
    {
        float currTime = 0;

        while (currTime < startSeconds)
        {
            currTime += Time.deltaTime;
            txtStartCount.text = $"{(int)startSeconds - (int)currTime}�� �� ����";
            yield return null;
        }
        txtStartCount.text = "";

        JM_GameManager.instance.SetStartPos();
        // �ش� ����Ʈ ���� ��� ������Ʈ�� ��Ȱ��ȭ��Ű��
        for (int i = 0; i < toOff.Count; i++)
        {
            toOff[i].SetActive(false);
        }
        // UI ���̰��� ī�޶� Ȱ��ȭ
        cam.gameObject.SetActive(true);
        // ���� ��Ʈ�� ����
        isStart = true;
    }


    // ���� ��Ʈ�� 
    float currentTime = 0;
    // �� UI
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
    // �÷��̾� ���� ����
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
            #region ��ġ ����
            // �÷��̾� Ȱ��ȭ �Լ� ���ӸŴ������� ȣ��
            // photonView.RPC("RPC_EnablePlayers", RpcTarget.All);

            // ��ġ�� ����


            // photonView.RPC("RPC_SetPlayerPos", RpcTarget.All);
            // photonView.RPC("RPC_Test", RpcTarget.All);
            #endregion
            isSelectionUI = false;

            // �÷��� ���� �˷��ִ� UI ����
            if (isLocalImposter)  imposters.SetActive(false);
            else crews.SetActive(false);

            cam.gameObject.SetActive(true);
            currentTime = 0;

            // ���� ���� ����
            isGameScene = true;
        }
    }
    public GameObject missionStatusUI;
    // ���� �����ϰ� ����
    void JM_GameEnable()
    {
        //JM_GameManager.instance.RPC_EnablePlayers();

        // ���� �����ϸ� ��ü ��Ӱ�
        light.GetComponent<Light2D>().intensity = 0.5f;


        gameMap.SetActive(true);
        missionStatusUI.SetActive(true);
        if (isLocalImposter)
        {
            imposterGameUI.SetActive(true);
            imposterUICode.enabled = true;
        }
        else if (!isLocalImposter)
        {
            crewGameUI.SetActive(true);
            crewUICode.enabled = true;
        }
    }


    public GameObject reportedDeadBody;
    // ��ü �߰� �� ����Ʈ ��ư ������ UI Ȱ��ȭ
    public void OnReportButton()
    {
        Report(dieColor.r, dieColor.g, dieColor.b, dieColor.a);
        
    }
    // RPC�� ��ü ���� �ѱ��
    public void Report(float deadR, float deadG, float deadB, float deadA)
    {
        photonView.RPC("RPC_Report", RpcTarget.All,  deadR, deadG, deadB, deadA);
    }
    [PunRPC]
    public void RPC_Report(float deadR, float deadG, float deadB, float deadA)
    {
        Color diedCrewColor = new Color(deadR, deadG, deadB, deadA);

        // �Ű�� ��ü Destroy
        Destroy(reportedDeadBody);

        StartReportUI(diedCrewColor);
    }
    // ��ü �� ��ȯ �� ����Ʈ UI Ȱ��ȭ + ��ǥ ����
    void StartReportUI(Color diedCrewColor)
    {
        Material mat = diedCrew.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", diedCrewColor);
        // ����Ʈ UI 2�ʰ� Ȱ��ȭ
        StartCoroutine("ActivateReportUI");
    }
    IEnumerator ActivateReportUI()
    {
        reportUI.SetActive(true);
        yield return new WaitForSeconds(2);
        reportUI.SetActive(false);
        // ��ǥ ����
        SH_VoteManager.instance.PlayerPanelSetting();
    }



    // ä�� ����, �ݱ�
    public GameObject chatView;
    public bool open = false;
    public void OnClickChat() 
    {
        if (!open)
        {
            chatView.SetActive(true);
            open = true;
        }
        else
        {
            chatView.SetActive(false);
            open = false;
        }
    }

    // ���ȸ�� 
    public GameObject emergencyImg;
    [PunRPC]
    public void EmergencyMeeting()
    {
        StartCoroutine("ActivateEmergencyUI");
    }
    IEnumerator ActivateEmergencyUI()
    {
        emergencyImg.SetActive(true);
        yield return new WaitForSeconds(2);
        emergencyImg.SetActive(false);
        // ��ǥ ����
        SH_VoteManager.instance.PlayerPanelSetting();
    }
}