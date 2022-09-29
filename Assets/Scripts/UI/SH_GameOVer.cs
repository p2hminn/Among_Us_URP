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

        // ũ�� ����Ʈ �ʱ�ȭ
        for (int i = 0; i < crews.Count; i++)
        {
            crews[i].gameObject.SetActive(false);
        }

        // ���� �÷��̾� ���� ã��
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
            Crew(true);   // ũ�簡 �̱� ��� & ���� �÷��̾ ũ���� ���
            Crew(false);  // ũ�簡 �� ��� & ���� �÷��̾ ũ���� ���
        }
    }

    // ũ��� UI
    public void Crew(bool crewWin)
    {
        if (crewWin)
        {
            // ���� ��� Text
            txtResult.text = "Win";
            txtResult.color = winColor;
            // Gradient Color
            gradImg.color = winColor;

        }
        else
        {
            // ���� ��� Text
            txtResult.text = "Loose";
            txtResult.color = looseColor;
            // Gradient Color
            gradImg.color = looseColor;
        }

        // Crew ����ŭ �÷� ���� & ��ġ
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // �������Ͱ� �ƴ� ���̸� ��ġ
            if (JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter == false)
            {
                // �� ���� ���� �÷��̾�
                if (i == 0)
                {
                    crews[i].gameObject.GetComponent<Image>().material.SetColor("_Playercolor", localColor);
                    crews[i].gameObject.SetActive(true);
                }
                else
                {
                    // ���� ��������
                    Color c = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
                    // ���� ����
                    JM_GameManager.instance.playerList[i].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", c);
                    crews[i].gameObject.SetActive(true);
                }
            }
        }
        StartCoroutine("GameOverFadeIn");
    }

    // �������� UI
    public void Impostor(bool crewWin)
    {
        if (!crewWin)
        {
            // ���� ��� Text
            txtResult.text = "Win";
            txtResult.color = winColor;
            // Gradient Color
            gradImg.color = winColor;

        }
        else
        {
            // ���� ��� Text
            txtResult.text = "Loose";
            txtResult.color = looseColor;
            // Gradient Color
            gradImg.color = looseColor;
        }

        // Impostor ����ŭ �÷� ���� & ��ġ
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // ���������� ���̸� ��ġ
            if (JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().isImposter == true)
            {
                // �� ���� ���� �÷��̾�
                if (i == 0)
                {
                    crews[i].gameObject.GetComponent<Image>().material.SetColor("_Playercolor", localColor);
                    crews[i].gameObject.SetActive(true);
                }
                else
                {
                    // ���� ��������
                    Color c = JM_GameManager.instance.playerList[i].gameObject.GetComponent<JM_PlayerMove>().color;
                    // ���� ����
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

    // Quit ��ư (�κ�� �̵�)
    public void OnQuit()
    {
        PhotonNetwork.LeaveRoom();
    }

    // PlayAgain ��ư (���Ƿ� ������)
    public void OnPlayAgain()
    {
        PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    }
}
