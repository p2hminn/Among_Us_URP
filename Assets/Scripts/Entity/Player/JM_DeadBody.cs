using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JM_DeadBody : MonoBehaviourPun
{
    [SerializeField]
    public Color color;

    Material mat;


    void Start()
    {

        mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", color);

        // 플레이어가 죽으면 Report 버튼 비활성화
        if (photonView.IsMine)
        {
            GameObject.FindWithTag("Canvas").GetComponent<JM_CrewUI>().isReportAble = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color settingColor)
    {
        color = settingColor;
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = color.a;
        photonView.RPC("RPC_SetColor", RpcTarget.All, r, g, b, a);
    }

    [PunRPC]
    void RPC_SetColor(float r, float g, float b, float a)
    {
        color.r = r;
        color.g = g;
        color.b = b;
        color.a = a;
    }


}
