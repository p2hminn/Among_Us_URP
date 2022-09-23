using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_PlayerPanel : MonoBehaviourPun
{
    // �÷��̾� �г���, �÷��̾� ����, �÷��̾� ����(�׾����� ����) ����

    void Start()
    {
        
    }

    public Image playerImg;
    public Text NickNametxt;
    public Image diedImg;
    public Material voteMatFactory;
    // �г� �� ���� ���� + ����ȭ
    public void SetInfo(PhotonView photonView)
    {
         // �÷��̾� ����
         Color color = photonView.gameObject.GetComponent<JM_PlayerMove>().color;
        // ���� ������ ��Ƽ���� ���� �ֱ�
        Material mat = Instantiate(voteMatFactory);
        mat.SetColor("_PlayerColor", color);
        playerImg.material = mat;

        // �÷��̾� �г���
        NickNametxt.GetComponent<Text>().text = photonView.Owner.NickName;

        // �÷��̾� �г� ���� ���� ����ȭ 
        photonView.RPC("RPC_SetPanel", RpcTarget.All);
    }
    
    // �÷��̾� ��ǥ �гΰ� ���õ� ��� ���� ����ȭ
    [PunRPC]
    void RPC_SetPanel()
    {
        // �÷��̾� �׾�����
        if (photonView.gameObject.CompareTag("Ghost"))
        {
            // Died ǥ�� Ȱ��ȭ
            diedImg.gameObject.SetActive(true);
            // ��ư interactable ��Ȱ��ȭ
            GetComponent<Button>().interactable = false;
        }
        
        // �Ű��� ũ��
        //if (photonView.gameObject.)
    }


}
