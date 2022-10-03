using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

public class JM_GameManager : MonoBehaviourPun
{
    // �̱���
    public static JM_GameManager instance;

    // ���� waitRoom ���� gameRoom ���� �Ǵ�
    public bool isGameRoom;

    // �÷��̾���� ������ ����Ʈ
    public List<PhotonView> playerList = new List<PhotonView>();

    // �÷��̾���� �ε����� ������ ����Ʈ
    public List<int> playerIndexList = new List<int>();

    // �ʱ� ������ġ ����Ʈ
    public List<Transform> spawnPosList = new List<Transform>();

    // ���Ӿ� ������ġ ����
    public Transform gameStartOrigin;

    // ���� �÷��̾� photonView
    public PhotonView localPv;

    int randomNum;

    // playerList�� ���ӿ�����Ʈ�� ������������ ���� ����Ʈ
    public List<bool> isImposterList = new List<bool>();

    // playerList�� ���ӿ�����Ʈ �÷� ����Ʈ
    public List<Color> colorList = new List<Color>();

    // ���� �÷��̾� �������� ����
    public bool isLocalImposter;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.SerializationRate = 60;

        PhotonNetwork.SendRate = 60;

        int randomNum = Random.Range(0, 3);

        // �� �ο� �� text ������Ʈ
        SH_RoomUI.instance.PlayerNumUpdate();

        GameObject crew = PhotonNetwork.Instantiate("Crew2_New", spawnPosList[randomNum].position, Quaternion.identity);
        // ���� �÷��̾��� photonView ����
        localPv = crew.GetComponent<PhotonView>();

    }
    // ����Ʈ ��ư ������ ���� �÷��̾��� ViewID �����ϵ��� �Ѹ���
    public void SendReportPlayer()
    {
        localPv.RPC("RPC_SendReportPlayer", RpcTarget.All, localPv.ViewID);
    }

    bool isOnce;
    bool isOnce1;
    bool isOnce2;
    //bool isOnce3;
    void Update()
    {
        // ������ Start��ư ���� ��� playerList photonView�� gameObject ��Ȱ��ȭ (�ѹ��� ������ ��)
        if (SH_RoomUI.instance.isStart && !isOnce)
        {
            // imposter �� �Ű������� �־ imposter ���� ���� ����
            SetGameScene((int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"]);
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].gameObject.SetActive(false);
            }
            isOnce = true;
        }

        // ���Ӿ��� �� ��� �ٽ� �÷��̾�� Ȱ��ȭ��Ű��
        if (SH_RoomUI.instance.isGameScene && !isOnce1)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].gameObject.SetActive(true);
            }
            isGameRoom = true;
            isOnce1 = true;
        }

        // ���� ���۵Ǹ� �÷��̾� ����� ����
        if (isGameRoom && !isOnce2)
        {
            playerList.Sort((photon1, photon2) => photon1.ViewID.CompareTo(photon2.ViewID));
            isOnce2 = true;
        }

        //if (SH_VoteManager.instance.p)
        //{
        //    print("�׾���? 2 : " + SH_VoteManager.instance.p.gameObject.activeSelf);
        //    print("���� ���� �Լ�2_GameManager : " + MethodBase.GetCurrentMethod().Name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3) && !isOnce3)
        //{
        //    isOnce3 = true;
        //    GameObject g = GameObject.Find("GameOverUI");
        //    g.GetComponent<SH_GameOVer>().Crew(true);   // ũ�簡  �̱� ��� & ���� �÷��̾ ũ���� ���
        //                  //Crew(false);  // ũ�簡 �� ��� & ���� �÷��̾ ũ���� ���
        //}
    }
    // ���Ӿ� Ȱ��ȭ
    //[PunRPC]
    //public void RPC_EnablePlayers()
    //{
    //    for (int i = 0; i < playerList.Count; i++)
    //    {
    //        playerList[i].gameObject.SetActive(true);
    //    }
    //}

    [PunRPC]
    public void RPC_SetPlayerPos()
    {
        for (int i = 0; i < playerList.Count; i++)
        {

        }
    }

    // ���ӽ��� ��ġ��
    public Vector3[] startPos;

    // ������ Ŭ���̾�Ʈ�϶��� ��ġ���� ����
    public void SetStartPos()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        startPos = new Vector3[playerList.Count];
        float angle = 360 / playerList.Count;
        for (int i = 0; i < playerList.Count; i++)
        {
            startPos[i] = gameStartOrigin.position + transform.up * 2.5f;
            playerList[i].gameObject.transform.position = startPos[i];
            transform.Rotate(0, 0, angle);

            playerList[i].gameObject.GetComponent<JM_PlayerMove>().SetIndividualPos(startPos[i].x, startPos[i].y, startPos[i].z);
        }
    }

    // ������ ��ġ���� �ش� ������� ���� ����



    // �÷��̾� ������ �� �÷��̾� ����Ʈ�� �÷��̾� ���� �� ���� (PlayerMove.cs�� Start)
    public void AddPlayer(PhotonView pv)
    {
        playerList.Add(pv);
        playerIndexList.Add(playerIndexList.Count);
    }

    public List<int> imposterIndexList = new List<int>();

    // �������� idx ����
    public void SetGameScene(int imposterAmt)
    {
        print("imposterAmt : " + imposterAmt);
        //isGameRoom = true;
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            // �������� ����ŭ�� for ���� ������
            for (int i = 0; i < imposterAmt; i++)
            {
                // �÷��̾� �ִ� ����(���� �濡 �ִ� �ִ� �ο�)�� 0 ���̿��� ���� ���� ����
                int randomNum = Random.Range(0, playerIndexList.Count);
                //int randomNum = 0;

                // �������ڸ� �������� �ε��� ����Ʈ�� ����
                imposterIndexList.Add(playerIndexList[randomNum]);
                // �������ڸ� �÷��̾� �ε��� ����Ʈ���� ����
                playerIndexList.RemoveAt(randomNum);
            }
            ChooseImposter(imposterIndexList);
            ChooseCrew();
        }
        */

        for (int i = 0; i < imposterAmt; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int randomNum = Random.Range(0, playerIndexList.Count);
                print("master : " + randomNum);
                photonView.RPC("RPC_ShareRandomNum", RpcTarget.All, playerIndexList[randomNum]);
                playerIndexList.RemoveAt(randomNum);
            }

        }
        if (PhotonNetwork.IsMasterClient)
        {
            ChooseImposter(imposterIndexList);
            ChooseCrew();
        }  
    }

    [PunRPC]
    void RPC_ShareRandomNum(int i)
    {
        print("others : " + randomNum);
        imposterIndexList.Add(i);
    }

    // imposter �ε����� ���� ����Ʈ�� �޾� imposter ����
    void ChooseImposter(List<int> imposterIndexList)
    {
        /*
        for (int i = 0; i < imposterIndexList.Count; i++)
        {
            if (playerList)
            playerList[imposterIndexList[i]].RPC("RPC_SetImposter", playerList[i].Owner);
        }
        */

        
        for (int i = 0; i < playerList.Count; i++)
        {
            // ���������� �ε����� �´ٸ�
            for (int j = 0; j < imposterIndexList.Count; j++)
            {
                if (i == imposterIndexList[j])
                {
                    // RPC �Լ��� �ش� �ε��� �÷��̾�� �������� �Ҵ�
                    playerList[i].RPC("RPC_SetImposter", RpcTarget.All);


                    print("�������� �ε��� : " + i);
                }
            }
        }
        

    }

    void ChooseCrew()
    {
        for (int i = 0; i < playerIndexList.Count; i++)
        {
            playerList[playerIndexList[i]].RPC("RPC_SetCrew", playerList[playerIndexList[i]].Owner);
        }
    }

}