using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviourPun
{
    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    public Image blackImg;
    public Image reportImg;

    // 패널 상세 정보 세팅 
    public void SetInfo(PhotonView photonView, int reportViewID = 0)
    {
        // 죽은 크루의 경우
        if (photonView.gameObject.CompareTag("Ghost"))
        {
            // 버튼 비활성화
            GetComponent<Button>().interactable = false;
            // 죽은 표시 
            diedImg.gameObject.SetActive(true);
            // 블랙이미지 활성화
            blackImg.gameObject.SetActive(true);
        }

        // 플레이어 색상
        Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
        // 새로 생성한 머티리얼 파일 넣기
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // 플레이어 닉네임
        NickNametxt.GetComponent<Text>().text = photonView.Owner.NickName;

        // 신고한 사람 
        if (reportViewID == photonView.ViewID) photonView.RPC("RPC_SetPanel", RpcTarget.All, reportViewID);
    }
    

    // 플레이어 투표 패널과 관련된 모든 사항 동기화
    [PunRPC]
    void RPC_SetPanel()
    {
        // 리포트한 사람 표시
        reportImg.gameObject.SetActive(true);
    }


    public Button btnVote;
    public Button btnVoteCancel;

    // 플레이어 패널 클릭할 때 투표 버튼 나오게 하기
    public int n;
    public void OnClickPanel()
    {
        // 투표 완료했을 경우에는 투표 버튼 안나오게 하기
        if (voteComplete) return;

        Transform trPanel = GameObject.FindGameObjectsWithTag("Panels")[0].transform;

        
        // 내가 선택한 패널이 아닐 경우 나머지 패널들의 투표 버튼 비활성화
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

    
    public Image voteForImg;
    public Image votedImg;
    bool voteComplete;
    // 투표 확인 버튼 누를 경우 
    public void OnClickVote()
    {
        voteComplete = true;
        // 투표완료하면  모든 패널들 자기 자신의 버튼 interactable 비활성화
        //GetComponent<Button>().interactable = false;

        // 투표 완료 이미지 활성화 + 동기화
        photonView.RPC("SendVoted", RpcTarget.All);

        // 자신의 투표 결과를 MasterClient VoteManager에게 보내기
        photonView.RPC("SendVoteResult", RpcTarget.MasterClient, gameObject);

        // VoteFor 이미지 활성화
        voteForImg.gameObject.SetActive(true);

    }
    // 모두에게 투표완료했음 표시
    [PunRPC]
    public void SendVoted()
    {
        // 투표 버튼 비활성화 + 동기화
        btnVote.gameObject.SetActive(false);
        btnVoteCancel.gameObject.SetActive(false);
        votedImg.gameObject.SetActive(true);
    }
    // MasterClient에게만 투표 결과 보내기
    [PunRPC]
    public void SendVoteResult(GameObject g)
    {
        SH_VoteManager.instance.voteResultDic[g] += 1;
        SH_VoteManager.instance.voteCompleteNum++;
    }


    // 투표 취소 버튼 누를 경우
    public void OnClickVoteCancel()
    {
        btnVote.gameObject.SetActive(false);
        btnVoteCancel.gameObject.SetActive(false);
    }

}
