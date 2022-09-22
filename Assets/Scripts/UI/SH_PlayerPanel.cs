using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviourPun
{
    // 플레이어 닉네임, 플레이어 색깔, 플레이어 상태(죽었는지 여부) 세팅

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // 패널 상세 정보 세팅
    public void SetInfo(PhotonView photonView)
    {
        // 플레이어 색상
        Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
        Material mat = transform.GetChild(0).GetComponent<Material>();
        mat.SetColor("_PlayerColor", color);

        // 플레이어 닉네임
        transform.GetChild(1).GetComponent<Text>().text = photonView.Owner.NickName;
    }
}
