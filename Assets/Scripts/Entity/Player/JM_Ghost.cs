using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class JM_Ghost : MonoBehaviourPun
{
    Color color;
    SpriteRenderer sr;
    // �÷��̾� �̵��ӵ�
    public float playerSpeed = 3;

    Material mat;
    public GameObject body;



    void Start()
    {
        // ��������Ʈ ������                                                                                                                                                                                                                                    
        sr = body.GetComponent<SpriteRenderer>();


        //// �г��� ����
        //photonView.Owner.NickName = PhotonNetwork.NickName;

        //// ���� �Ŵ��� �÷��̾��Ʈ�� �ڱ� ����� �ø���
        //JM_GameManager.instance.playerList.Add(photonView);

    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 Ŀ�� UI ���� ���� ��� �÷��̾� �� �����̰�
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // �̵� ��ǲ�� ���� ��� 
            if (h != 0f || v != 0f)

                // �̵��Լ� ����
                Move(h, v);
        }
    }

    public void SetColor(Color settingColor)
    {
        color = settingColor;
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = color.a;
        mat = body.GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", color);
    }

    // �̵� �Լ�
    private void Move(float h, float v)
    {

        // ���� ����
        Vector3 playerDir = h * Vector3.right + v * Vector3.up;
        playerDir.Normalize();


        // �̵��������� ���ϵ��� �������� ����
        // �ϴ� �Լ��� �� �� ���濡 ����ȭ

        ChangeDir(playerDir);

        // �̵�
        transform.position += playerDir * playerSpeed * Time.deltaTime;

    }

    void ChangeDir(Vector3 dir)
    {
        if (dir.x > 0f)
        {
            sr.flipX = false;
        }
        //transform.localScale = new Vector3(-1f, 1f, 1f);

        else if (dir.x < 0f)
        {
            sr.flipX = true;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}