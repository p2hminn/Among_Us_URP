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

    // ���� ��������UI �ڵ� �� ũ��UI �ڵ�
    JM_ImposterUI imposterUICode;
    JM_CrewUI crewUICode;

    public GameObject imposterGameUI;
    public GameObject crewGameUI;
    public GameObject reportUI;
    public GameObject diedCrew;

    // ��ü ����
    public Color dieColor;

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
    }
    void Update()
    {
        // ���� ���� �ο��� 4���̰� ������ ��쿡  Start ��ư  interactable Ȱ��ȭ
        if (PhotonNetwork.CurrentRoom.PlayerCount ==  2 && PhotonNetwork.IsMasterClient)
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
        JM_GameManager.instance.SetStartPos();
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


    // ���� ��Ʈ�� 
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

            // �÷��̾� Ȱ��ȭ �Լ� ���ӸŴ������� ȣ��
            // photonView.RPC("RPC_EnablePlayers", RpcTarget.All);
            JM_GameManager.instance.RPC_EnablePlayers();

            // ��ġ�� ����
            
                
            // photonView.RPC("RPC_SetPlayerPos", RpcTarget.All);
            // photonView.RPC("RPC_Test", RpcTarget.All);

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
    public GameObject missionStatusUI;
    void JM_GameEnable()
    {
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
            print("open");
            chatView.SetActive(true);
            open = true;
        }
        else
        {
            print("close");
            chatView.SetActive(false);
            open = false;
        }
    }

}