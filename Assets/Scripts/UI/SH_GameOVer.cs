using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_GameOVer : MonoBehaviourPun
{
    public bool crewWin;
    float alpha;
    public Text txtResult;
    public Image gradImg;
    public Color winColor;
    public Color looseColor;
    public List<Image> crews;
    Color localColor;

    private void Start()
    {
        alpha = GetComponent<CanvasGroup>().alpha;
        winColor = new Color32(45, 118, 214, 255);
        looseColor = new Color32(255, 0, 42, 199);

        // 크루 리스트 초기화
        for (int i = 0; i < crews.Count; i++)
        {
            crews[i].gameObject.SetActive(false);
        }

        // 로컬 플레이어 색상 찾기
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            if (JM_GameManager.instance.playerList[i].IsMine)
            {
                localColor = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
            }
        }
    }

    bool isOnce;
    private void Update()
    {
        if (Input.GetButtonDown("Jump'") && !isOnce)
        {
            isOnce = true;
            Crew(true);   // 크루가 이긴 경우 & 로컬 플레이어가 크루인 경우
            Crew(false);  // 크루가 진 경우 & 로컬 플레이어가 크루인 경우
        }
    }

    // 크루원 UI
    public void Crew(bool crewWin)
    {
        if (crewWin)
        {
            // 게임 결과 Text
            txtResult.text = "Win";
            txtResult.color = winColor;
            // Gradient Color
            gradImg.color = winColor;

        }
        else
        {
            // 게임 결과 Text
            txtResult.text = "Loose";
            txtResult.color = looseColor;
            // Gradient Color
            gradImg.color = looseColor;
        }

        // Crew 수만큼 컬러 지정 & 배치
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // 임포스터가 아닌 아이만 배치
            if (JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter == false)
            {
                // 맨 앞은 로컬 플레이어
                if (i == 0)
                {
                    crews[i].gameObject.GetComponent<Image>().material.SetColor("_Playercolor", localColor);
                    crews[i].gameObject.SetActive(true);
                }
                else
                {
                    // 색상 가져오기
                    Color c = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
                    // 색상 지정
                    JM_GameManager.instance.playerList[i].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", c);
                    crews[i].gameObject.SetActive(true);
                }
            }
        }
        StartCoroutine("GameOverFadeIn");
    }

    // 임포스터 UI
    public void Impostor(bool crewWin)
    {
        if (!crewWin)
        {
            // 게임 결과 Text
            txtResult.text = "Win";
            txtResult.color = winColor;
            // Gradient Color
            gradImg.color = winColor;

        }
        else
        {
            // 게임 결과 Text
            txtResult.text = "Loose";
            txtResult.color = looseColor;
            // Gradient Color
            gradImg.color = looseColor;
        }

        // Impostor 수만큼 컬러 지정 & 배치
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // 임포스터인 아이만 배치
            if (JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter == true)
            {
                // 맨 앞은 로컬 플레이어
                if (i == 0)
                {
                    crews[i].gameObject.GetComponent<Image>().material.SetColor("_Playercolor", localColor);
                    crews[i].gameObject.SetActive(true);
                }
                else
                {
                    // 색상 가져오기
                    Color c = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
                    // 색상 지정
                    JM_GameManager.instance.playerList[i].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", c);
                    crews[i].gameObject.SetActive(true);
                }
            }
        }
        StartCoroutine("GameOverFadeIn");
    }

    public float fadeSpeed;
    IEnumerator GameOverFadeIn()
    {
        while (alpha < 1)
        {
            alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        alpha = 1;
    }

    // Quit 버튼 (로비로 이동)
    public void OnQuit()
    {
        PhotonNetwork.LeaveRoom();
    }

    // PlayAgain 버튼 (대기실로 재입장)
    public void OnPlayAgain()
    {
        PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    }
}
