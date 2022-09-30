using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviour
{
    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    public Image blackImg;
    public Image reportImg;
    public Transform trPanel;
    public int playerViewId;
    public PhotonView photonView;
    public int localPanelIdx;


    // 패널 상세 정보 세팅 
    public void SetInfo(PhotonView pv, int reportViewID)
    {
        playerViewId = pv.ViewID;
        photonView = pv;
        // 죽은 크루의 경우
        if (pv.gameObject.CompareTag("Ghost"))
        {
            // 버튼 비활성화
            GetComponent<Button>().interactable = false;
            // 죽은 표시 
            diedImg.gameObject.SetActive(true);
            // 블랙이미지 활성화
            blackImg.gameObject.SetActive(true);
        }

        // 플레이어 색상
        Color color = pv.gameObject.GetComponent<JM_PlayerMove>().color;
        // 새로 생성한 머티리얼 파일 넣기
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // 플레이어 닉네임
        NickNametxt.GetComponent<Text>().text = pv.Owner.NickName;

        // 신고한 사람 
        if (reportViewID == pv.ViewID) //&& pv.IsMine)
        {
            transform.GetChild(9).gameObject.SetActive(true);  // Img_Report
            //pv.RPC("RPC_SetPanel", RpcTarget.All);
        }
        //print("reportViewID : " + reportViewID +"  /  pv.ViewID : " + pv.ViewID);

    }
    // 플레이어 투표 패널과 관련된 모든 사항 동기화
    //[PunRPC]
    //public void RPC_SetPanel()
    //{
    //    // 리포트한 사람 표시
    //    transform.GetChild(9).gameObject.SetActive(true);  // Img_Report
    //    print("SetPanel");
    //}



    // 플레이어 패널 클릭할 때 투표 버튼 나오게 하기
    public void OnClickPanel()
    {
        // 투표 완료했을 경우에는 투표 버튼 안나오게 하기
        if (voteComplete) return;

        // 내가 선택한 패널이 아닐 경우 나머지 패널들의 투표 버튼 비활성화
        trPanel = SH_RoomUI.instance.trPanels;
        foreach (Transform panel in trPanel)
        {
            if (panel.gameObject != gameObject)
            {
                panel.GetChild(7).gameObject.SetActive(false);
                panel.GetChild(8).gameObject.SetActive(false);
            }
            else
            {
                panel.GetChild(7).gameObject.SetActive(true);
                panel.GetChild(8).gameObject.SetActive(true);
            }
        }
    }

    
    
    bool voteComplete;
    // 투표 확인 버튼 누를 경우 
    public void OnClickVote()
    {
        //print(gameObject.transform.GetChild(1).GetComponent<Text>().text);  => 누른 확인 버튼이 달려있는 패널
        voteComplete = true;
        // 투표완료하면  모든 패널들 투표 버튼 비활성화
        SH_VoteManager.instance.PanelOff();

        // 로컬 플레이어를 표시한 패널의 투표 완료 이미지 활성화 + 동기화
        photonView.RPC("RPC_SendVoted", RpcTarget.All, localPanelIdx);

        // 자신의 투표 결과를 VoteManager에게 보내기
        photonView.RPC("RPC_SendVoteResult", RpcTarget.All, transform.GetSiblingIndex());

        // VoteFor 이미지 활성화
        transform.GetChild(5).gameObject.SetActive(true);

    }
    

    // 투표 취소 버튼 누를 경우
    public void OnClickVoteCancel()
    {
        transform.GetChild(7).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
    }
}
