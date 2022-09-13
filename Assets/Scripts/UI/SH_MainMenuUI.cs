using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SH_MainMenuUI : MonoBehaviour
{
    // 온라인 버튼
    // Inspector창에서 온라인 버튼 누르면 MainMenu UI 끄고 Online UI 활성화하도록 설정
    public void OnClickOnlineButton()
    {
        // 기본 로비 진입
        PhotonNetwork.JoinLobby();
    }


    // 게임 종료 버튼
    public void OnClickQuitButton()
    {
// 유니티 에디터일경우 플레이 중단
# if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
// 빌드된 상태일 경우 어플리케이션 종료
# else 
            Application.Quit();
# endif 
    }
}
