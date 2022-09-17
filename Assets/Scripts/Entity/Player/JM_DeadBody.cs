using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_DeadBody : MonoBehaviourPun
{
    [SerializeField]
    Color color;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", color);

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
