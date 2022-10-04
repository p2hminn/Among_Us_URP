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

    int idx;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        winColor = new Color32(45, 118, 214, 255);
        looseColor = new Color32(255, 0, 42, 199);


        // ũ�� �̹��� ����Ʈ �ʱ�ȭ
        for (int i = 0; i < crews.Count; i++)
        {
            crews[i].gameObject.SetActive(false);
        }

        // ũ�� 

        // ��������

        // ���� �÷��̾� ���� ã��
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
    // ũ��� UI
    public void Crew(bool crewWin)
    {
        #region UI ��������
        if (crewWin == true)
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

        Material mat = crews[0].gameObject.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.localColor);
        resultColor.Add(JM_ColorManager.instance.localColor);
        crews[0].gameObject.SetActive(true);
        crews.RemoveAt(0);


        // ����Ʈ���� ���� �÷��̾��� ������ ���� �����
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            if (JM_ColorManager.instance.localColor == JM_GameManager.instance.colorList[i])
            {
                JM_GameManager.instance.playerList.RemoveAt(i);
                JM_GameManager.instance.isImposterList.RemoveAt(i);
                JM_GameManager.instance.colorList.RemoveAt(i);
            }
        }

        //if (JM_GameManager.instance.playerList[0].gameObject.CompareTag("Ghost")) crews[0].sprite = ghostSprite;



        // ������ ��������
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {

            if (!JM_GameManager.instance.isImposterList[i])
            {
                crews[idx].gameObject.SetActive(true);

                Color color = JM_GameManager.instance.colorList[i];
                crews[idx].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", color);

                idx++;

                //if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[i].sprite = ghostSprite;
            }
        }

        /*

        // ���� �÷��̾� �̸� ����
        crews[0].gameObject.SetActive(true);
        Material mat = crews[0].gameObject.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.localColor);
        resultColor.Add(JM_ColorManager.instance.localColor);
       

        int idx = 1;
        // Crew ����ŭ �÷� ���� & ��ġ
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            // ���������̰ų� ���� �÷��̾��� ��� �ǳʶٱ�
            //if (JM_GameManager.instance.isImposterList[i] == true | JM_GameManager.instance.colorList[i] == JM_ColorManager.instance.localColor) continue;
            if (JM_GameManager.instance.isImposterList[i] == false && JM_GameManager.instance.colorList[i] != JM_ColorManager.instance.localColor)
            {
                crews[i].gameObject.SetActive(true);
                // �ش� ������ Crew�� idx��° �̹����� ��ĥ + Ȱ��ȭ
                Color c = JM_GameManager.instance.colorList[i];
                resultColor.Add(c);
                print("c :" + c);
                crews[idx].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", c);
                // ��Ʈ�� ��� sprite �������ֱ�
                if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[idx].sprite = ghostSprite;
                idx++;
            }
        }
        */
        #endregion
        print("GameOverUI �������� �Ϸ�");
        gameOverUI.SetActive(true);
        //cg.alpha = 1;
        //StartCoroutine("GameOverFadeIn");
    }

    // �������� UI
    public void Impostor(bool crewWin)
    {
        #region UI ��������
        if (crewWin == false)
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

        // ���� �÷��̾� �̸� ����         
        Material mat = crews[0].gameObject.GetComponent<Image>().material;
        mat.SetColor("_PlayerColor", JM_ColorManager.instance.localColor);
        resultColor.Add(JM_ColorManager.instance.localColor);
        crews[0].gameObject.SetActive(true);
        crews.RemoveAt(0);

        // ����Ʈ���� ���� �÷��̾��� ������ ���� �����
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {
            if (JM_ColorManager.instance.localColor == JM_GameManager.instance.colorList[i])
            {
                print("���� ��...");
                JM_GameManager.instance.playerList.RemoveAt(i);
                JM_GameManager.instance.isImposterList.RemoveAt(i);
                JM_GameManager.instance.colorList.RemoveAt(i);

            }
        }

        //if (JM_GameManager.instance.playerList[0].gameObject.CompareTag("Ghost")) crews[0].sprite = ghostSprite;



        // ������ ��������
        for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        {

            if (JM_GameManager.instance.isImposterList[i])
            {
                crews[idx].gameObject.SetActive(true);

                Color color = JM_GameManager.instance.colorList[i];
                crews[idx].gameObject.GetComponent<Image>().material.SetColor("_PlayerColor", color);
                idx++;

                //if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[i].sprite = ghostSprite;
            }
        }


        //// ũ�� �̹��� �ε���
        //int idx = 1;
        //// Crew ����ŭ �÷� ���� & ��ġ
        //for (int i = 0; i < JM_GameManager.instance.playerList.Count; i++)
        //{
        //    // ũ���̰ų� ���� �÷��̾��� ��� �ǳʶٱ�
        //    if (JM_GameManager.instance.isImposterList[i] == false | JM_GameManager.instance.colorList[i] == JM_ColorManager.instance.localColor) continue;
        //    else
        //    {
        //        // �÷�����Ʈ i��° ������ Crew�� idx��° �̹����� ��ĥ + Ȱ��ȭ
        //        crews[i].gameObject.SetActive(true);
        //        Color c = JM_GameManager.instance.colorList[i];
        //        Material mat2 = crews[idx].gameObject.GetComponent<Image>().material;
        //        mat2.SetColor("_PlayerColor", c);
        //        resultColor.Add(c);
        //        // ��Ʈ�� ��� sprite �������ֱ�
        //        if (JM_GameManager.instance.playerList[i].gameObject.CompareTag("Ghost")) crews[idx].sprite = ghostSprite;
        //        idx++;
        //    }
        //}
        #endregion

        print("GameOverUI �������� �Ϸ�");
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

    // Quit ��ư (�κ�� �̵�)
    public void OnQuit()
    {
        //cg.alpha = 0;
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LoadLevel("SH_MainMenu"); 
        Application.Quit();
    }

    // PlayAgain ��ư (���Ƿ� ������)
    //public void OnPlayAgain()
    //{
    //    cg.alpha = 0;
    //    PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    //}
}