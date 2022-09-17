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

        GameObject crew = PhotonNetwork.Instantiate("Crew", spawnPosList[randomNum].position, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetGameScene(1);
        }
    }

    public void AddPlayer(PhotonView pv)
    {
        // �÷��̾� ����Ʈ�� �÷��̾� ���� �� ����
        playerList.Add(pv);
    }

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
                // �÷��̾� �ִ� ���ڿ� 0 ���̿��� ���� ���ڸ� ����
                int randomNum = 0;
                    //Random.Range(0, playerList.Count);
                print("index number : " + randomNum);
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
            ChooseImposter(imposterIndexList);
        }
    }

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
                    print("��� ����������" + i);
                }
                else
                {
                    playerList[i].RPC("RPC_SetCrew", RpcTarget.All);
                    print("��� ũ����" + i);
                }
            }
        }       
    }

   
}
