using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_MainMenuUI : MonoBehaviour
{
    // �¶��� ��ư
    // Inspectorâ���� �¶��� ��ư ������ MainMenu UI ���� Online UI Ȱ��ȭ�ϵ��� ����
    public void OnClickOnlineButton()
    {
        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();
    }


    // ���� ���� ��ư
    public void OnClickQuitButton()
    {
// ����Ƽ �������ϰ�� �÷��� �ߴ�
# if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
// ����� ������ ��� ���ø����̼� ����
# else 
            Application.Quit();
# endif 
    }
}
