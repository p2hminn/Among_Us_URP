using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_GameManager : MonoBehaviourPun
{
    // 싱글톤
    public static JM_GameManager instance;

    // 현재 waitRoom 인지 gameRoom 인지 판단
    public bool isGameRoom;

    // 플레이어들을 저장할 리스트
    public List<PhotonView> playerList = new List<PhotonView>();

    // 초기 스폰위치 리스트
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
        // 플레이어 리스트에 플레이어 포톤 뷰 저장
        playerList.Add(pv);
    }

    public void SetGameScene(int imposterAmt)
    {
        //isGameRoom = true;
        if (PhotonNetwork.IsMasterClient)
        {
            // int 로 이루어진 리스트를 만들고
            List<int> imposterGenerator = new List<int>();

            // 임포스터 수만큼의 for 문을 돌려서
            for (int i = 0; i < imposterAmt; i++)
            {
                // 플레이어 최대 숫자와 0 사이에서 랜덤 숫자를 생성
                int randomNum = 1;
                    //Random.Range(0, playerList.Count);
                print("index number : " + randomNum);
                // 임포스터 리스트에 랜덤숫자가 없다면
                if (!imposterGenerator.Contains(randomNum))
                {
                    // 리스트에 랜덤숫자 추가
                    imposterGenerator.Add(randomNum);
                }
                // 그렇지 않다면
                else
                {
                    // 다시 랜덤숫자 구함
                    randomNum = Random.Range(0, playerList.Count);
                }
            }
            ChooseImposter(imposterGenerator);
        }
    }

    void ChooseImposter(List<int> imposterGenerator)
    {
       for (int i = 0; i < playerList.Count; i++)
        {
            // 임포스터의 인덱스가 맞다면
            for (int j = 0; j < imposterGenerator.Count; j++)
            {
                if (i == imposterGenerator[j])
                {
                    // RPC 함수로 해당 인덱스 플레이어는 임포스터 할당
                    playerList[i].RPC("RPC_SetImposter", RpcTarget.All);
                    print("얘는 임포스터임" + i);
                }
                else
                {
                    playerList[i].RPC("RPC_SetCrew", RpcTarget.All);
                    print("얘는 크루임" + i);
                }
            }
        }       
    }
}
