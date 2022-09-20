using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_PlayerStatus : MonoBehaviourPun
{
    Animator anim;
    // 죽을때 시체를 생성한다.
    // 게임매니저에서 생성?
    // 시체 색은 플레이어의 색
    // 일단 시체 생성하는거부터

    // 시체 생성공장
    public GameObject deadBodyGenerator;

    


    public enum State
    {
        crew,
        ghost,
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void Dead()
    {
        // 여기까지 문제 ㄴ
        photonView.RPC("RPC_Dead", RpcTarget.AllBuffered);
        JM_CrewUI.instance.Die();
    } 
}
