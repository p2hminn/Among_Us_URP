using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_GameOverUI : MonoBehaviourPun
{

    public static SH_GameOverUI instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool crewWin;
    CanvasGroup cg;
    public Text txtResult;
    public Image gradImg;
    public Color winColor;
    public Color looseColor;
    public List<Image> crews;
    public Sprite ghostSprite;
    public List<Color> resultColor;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        winColor = new Color32(45, 118, 214, 255);
        looseColor = new Color32(255, 0, 42, 199);


        // 크루 이미지 리스트 초기화
        for (int i = 0; i < crews.Count; i++)
        {
            crews[i].gameObject.SetActive(false);
        }

        // 크루 

        // 임포스터

        // 로컬 플레이어 색상 찾기
        //for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        //{
        //    if (JM_GameManager.instance.playerList[i].IsMine)
        //    {
        //        localColor = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
        //        print(localColor);
        //    }
        //}

    }
    public GameObject gameOverUI;
    // 크루원 UI
    public void Crew(bool crewWin)
    {
        #region UI 사전세팅
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

        // 로컬 플레이어 미리 설정
        Material mat = crews[0].gameObject.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.localColor);
        resultColor.Add(JM_ColorManager.instance.localColor);
        crews[0].gameObject.SetActive(true);

        int idx = 1;
        // Crew 수만큼 컬러 지정 & 배치
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // 임포스터이거나 로컬 플레이어일 경우 건너뛰기
            if (JM_GameManager.instance.isImposterList[i] == true | JM_GameManager.instance.colorList[i] == JM_ColorManager.instance.localColor) continue;
            else
            {
                // 해당 색상을 Crew의 idx번째 이미지에 색칠 + 활성화
                Color c = JM_GameManager.instance.colorList[i];
                resultColor.Add(c);
                crews[idx].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", c);
                // 고스트일 경우 sprite 변경해주기
                if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[idx].sprite = ghostSprite;
                crews[i].gameObject.SetActive(true);
                idx++;
            }
        }
        #endregion
        print("GameOverUI 사전세팅 완료");
        gameOverUI.SetActive(true);
        //cg.alpha = 1;
        //StartCoroutine("GameOverFadeIn");
    }

    // 임포스터 UI
    public void Impostor(bool crewWin)
    {
        #region UI 사전세팅
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

        // 로컬 플레이어 미리 설정
        Material mat = crews[0].gameObject.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.localColor);
        crews[0].gameObject.SetActive(true);

        int idx = 1;
        // Crew 수만큼 컬러 지정 & 배치
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // 크루이거나 로컬 플레이어일 경우 건너뛰기
            if (JM_GameManager.instance.isImposterList[i] == false | JM_GameManager.instance.colorList[i] == JM_ColorManager.instance.localColor) continue;
            else
            {
                // 컬러리스트 i번째 색상을 Crew의 idx번째 이미지에 색칠 + 활성화
                Color c = JM_GameManager.instance.colorList[i];
                Material mat2 = crews[idx].gameObject.GetComponent<Image>().material;
                mat2.SetColor("_PlayerColor", c);
                // 고스트일 경우 sprite 변경해주기
                if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[idx].sprite = ghostSprite;
                crews[i].gameObject.SetActive(true);
                idx++;
            }
        }
        #endregion

        print("GameOverUI 사전세팅 완료");
        gameOverUI.SetActive(true);
        //cg.alpha = 1;
        //StartCoroutine("GameOverFadeIn");
    }

    public float fadeSpeed = 1;
    IEnumerator GameOverFadeIn()
    {
        while (cg.alpha < 1f)
        {
            cg.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1;
    }

    // Quit 버튼 (로비로 이동)
    public void OnQuit()
    {
        cg.alpha = 0;
        PhotonNetwork.LeaveRoom();
    }

    // PlayAgain 버튼 (대기실로 재입장)
    public void OnPlayAgain()
    {
        cg.alpha = 0;
        PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    }
}