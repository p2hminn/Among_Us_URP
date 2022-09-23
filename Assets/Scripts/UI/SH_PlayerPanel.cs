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

    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    // 패널 상세 정보 세팅 + 동기화
    public void SetInfo(PhotonView photonView)
    {
         // 플레이어 색상
         Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
        // 새로 생성한 머티리얼 파일 넣기
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // 플레이어 닉네임
        NickNametxt.GetComponent<Text>().text = photonView.Owner.NickName;

        // 플레이어 패널 사전 설정 동기화 
        photonView.RPC("RPC_SetPanel", RpcTarget.All);
    }
    
    // 플레이어 투표 패널과 관련된 모든 사항 동기화
    [PunRPC]
    void RPC_SetPanel()
    {
        // 플레이어 죽었으면
        if (photonView.gameObject.CompareTag("Ghost"))
        {
            // Died 표시 활성화
            diedImg.gameObject.SetActive(true);
            // 버튼 interactable 비활성화
            GetComponent<Button>().interactable = false;
        }
        
        // 신고한 크루
        //if (photonView.gameObject.)
    }


}
