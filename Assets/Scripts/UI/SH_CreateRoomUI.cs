using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Linq;

public class SH_CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("[�� �̸� Images]")]
    [SerializeField]
    private List<Image> mapImgs;
    [Header("[ũ�� Images]")]
    [SerializeField]
    private List<Image> crewImgs;
    [Header("[�������� �� ��ư]")]
    [SerializeField]
    private List<Button> imposterCountButtons;
    [Header("[�ִ� �÷��̾� �� ��ư]")]
    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    // ���� �����ϴ� ���� ������
    private CreateRoomData roomData;
    // �г��� ��������
    public SH_LobbyManager lobbyManager;
    

    void Start()
    {
        // �� �̸� 6�ڸ� ���� ����
        string roomName = RandomString(6);
        print(roomName);
        
        // �� ������ �ʱ�ȭ
        roomData = new CreateRoomData() { name = roomName, imposterCount = 1, maxPlayerCount = 10 };

        // ũ��, �������� �̹��� ������Ʈ
        UpdateImposterImgs();
        UpdateCrewImgs();
    }

    // ���� string ��� �Լ�
    private System.Random random = new System.Random();
    public string RandomString(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }


    // �� ���� �ɼ� ���� ��ư
    public void OnClickMapImgs(int count)
    {
        roomData.mapName = mapImgs[count].name;

        for (int i=0; i < mapImgs.Count; i++)
        {
            if (i == count)
            {
                mapImgs[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                mapImgs[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
    public void OnClickImposterNum(int count)
    {
        roomData.imposterCount = count;

        for (int i=0; i < imposterCountButtons.Count; i++)
        {
            if (i == count-1)
            {
                imposterCountButtons[i].image.color = new Color(1, 1, 1, 1);
                
            }
            else
            {
                imposterCountButtons[i].image.color = new Color(1, 1, 1, 0);
            }
        }
        UpdateImposterImgs();
    }
    // �Ͼ�� : MainMenu_Crew_Mat 
    // ������ : MainMenu_Imposter_Mat 
    private void UpdateImposterImgs()
    {
        int imposterCount = roomData.imposterCount;
        int maxCount = roomData.maxPlayerCount;

        // �ʱ�ȭ
        for (int i=0; i < crewImgs.Count; i++)
        {
            // crewImgs[i].color = new Color(1, 1, 1, 1);
            crewImgs[i].material = Resources.Load<Material>("Materials/MainMenu_Crew_Mat");
        }

        // �������� ����ŭ ���� ���� ����
        while (imposterCount > 0)
        {
            // ���° �̹����� �������� ��������� ������ ���� �̱�
            int n = Random.Range(0, maxCount);

            for (int i=0; i < maxCount; i++)
            {
                if (i == n && crewImgs[i].material != Resources.Load<Material>("Materials/MainMenu_Imposter_Mat") && crewImgs[i].gameObject.activeSelf)
                {
                    // ��� �߰��ڵ�
                    crewImgs[i].material = Resources.Load<Material>("Materials/MainMenu_Imposter_Mat");
                    //crewImgs[i].color = new Color(0, 0, 0, 1);
                    imposterCount--;
                }
            }
        }
    }
    private void UpdateCrewImgs()
    {
        int max = roomData.maxPlayerCount;

        for (int i = 0; i < crewImgs.Count; i++)
        {
            if (i < max)
            {
                crewImgs[i].gameObject.SetActive(true);
            }
            else
            {
                crewImgs[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            // �ִ� �÷��̾� ���� ������ ��ư �̹����� alpha�� = 1
            if (i == count - 4)
            {
                maxPlayerCountButtons[i].image.color = new Color(1, 1, 1, 1);
                
            }
            else
            {
                maxPlayerCountButtons[i].image.color = new Color(1, 1, 1, 0);
            }
        }

        // ũ�� �̹���, �������� �̹��� ������Ʈ
        UpdateCrewImgs();
        UpdateImposterImgs();
    }


    // Ȯ�� ��ư -> �� ����
    public void OnClickToCreateRoom()
    {
        // �� �ɼ� ����
        RoomOptions roomOptions = new RoomOptions();

        // �ִ� �ο�
        roomOptions.MaxPlayers = (byte) roomData.maxPlayerCount;
        // �� ��Ͽ� ���̴��� ����
        roomOptions.IsVisible = true;
        // custom ���� ���� 
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        // custom ���� �����ϴ� ����
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { };
           
        // �� �����
        PhotonNetwork.CreateRoom(roomData.name, roomOptions);
    }
    // �� ���� ������ ���(������ �ڵ� ����)
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    // �� ���� ������ ���
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }
}


// ���� �����Ǵ� ���� ������ ����
public class CreateRoomData
{
    // �� �̸�
    public string name;
    // �� �̸�
    public string mapName;
    // �������� ��
    public int imposterCount;
    // �ִ� �÷��̾� ��
    public int maxPlayerCount;
}


