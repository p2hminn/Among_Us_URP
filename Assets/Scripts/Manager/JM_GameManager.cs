using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_GameManager : MonoBehaviourPun
{
    // �̱���
    public static JM_GameManager instance;

    // ���� waitRoom ���� gameRoom ���� �Ǵ�
    public bool isGameRoom;

    // �÷��̾���� ������ ����Ʈ
    public List<PhotonView> playerList = new List<PhotonView>();

    // �ʱ� ������ġ ����Ʈ
    public List<Transform> spawnPosList = new List<Transform>();

    // ���Ӿ� ������ġ ����
    public Transform gameStartOrigin;

    // ���� �÷��̾� photonView
    public PhotonView localPv;


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

    bool isOnce = true;
    bool isOnce2 = true;

    bool isOnce3;
    void Update()
    {
        // ������ Start��ư ���� ��� playerList photonView�� gameObject ��Ȱ��ȭ (�ѹ��� ������ ��)
        if (SH_RoomUI.instance.isStart && isOnce)
        {
            // imposter �� �Ű������� �־ imposter ���� ���� ����
            SetGameScene((int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"]);
            print((int)PhotonNetwork.CurrentRoom.CustomProperties["imposter"]);
           
        }

        // ���Ӿ��� �� ��� �ٽ� �÷��̾�� Ȱ��ȭ��Ű��
        if (SH_RoomUI.instance.isGameScene)
        {
            isGameRoom = true; 
        }

        // ���� ���۵Ǹ� �÷��̾� ����� ����
        if (isGameRoom && isOnce2)
        {
            playerList.Sort((photon1, photon2) => photon1.ViewID.CompareTo(photon2.ViewID));
            for (int i=0;i<playerList.Count; i++)
            {
                print(playerList[i].ViewID);
            }
        }



        if (Input.GetKeyDown(KeyCode.Alpha3) && !isOnce3)
        {
            isOnce3 = true;
            GameObject g = GameObject.Find("GameOverUI");
            g.GetComponent<SH_GameOVer>().Crew(true);   // ũ�簡  �̱� ��� & ���� �÷��̾ ũ���� ���
                          //Crew(false);  // ũ�簡 �� ��� & ���� �÷��̾ ũ���� ���
        }
    }



    // ���Ӿ� Ȱ��ȭ
    [PunRPC]
    public void RPC_EnablePlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].gameObject.SetActive(true);
        }
    }

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
    }


    // �������� idx ����
    public void SetGameScene(int imposterAmt)
    {
        //isGameRoom = true;
        if (PhotonNetwork.IsMasterClient)
        {
            // int �� �̷���� ����Ʈ�� �����
            List<int> imposterIndexList = new List<int>();

            // �������� ����ŭ�� for ���� ������
            for (int i = 0; i < imposterAmt; i++)
            {
                // �÷��̾� �ִ� ����(���� �濡 �ִ� �ִ� �ο�)�� 0 ���̿��� ���� ���� ����
                int randomNum = Random.Range(0, playerList.Count);
                //int randomNum = 0;
                print("�������� �ε��� : " + randomNum);
                // �������� ����Ʈ�� �������ڰ� ���ٸ�
                if (!imposterIndexList.Contains(randomNum))
                {
                    // ����Ʈ�� �������� �߰�                                                        
                    imposterIndexList.Add(randomNum);
                }
                // �׷��� �ʴٸ�
                else
                {
                    // �ٽ� �������� ����
                    randomNum = Random.Range(0, playerList.Count);
                }
            }
            //print("ChooseImposter");
            ChooseImposter(imposterIndexList);
        }
    }
    // imposter �ε����� ���� ����Ʈ�� �޾� imposter ����
    void ChooseImposter(List<int> imposterIndexList)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            // ���������� �ε����� �´ٸ�
            for (int j = 0; j < imposterIndexList.Count; j++)
            {
                if (i == imposterIndexList[j])
                {  
                    // RPC �Լ��� �ش� �ε��� �÷��̾�� �������� �Ҵ�
                    playerList[i].RPC("RPC_SetImposter", RpcTarget.All);
                    //print("�������� �ε��� : " + i);
                }                                                             
                else
                {
                    playerList[i].RPC("RPC_SetCrew", RpcTarget.All);
                    //print("ũ�� �ε��� : " + i);
                }
            }
        }
    }
    
}