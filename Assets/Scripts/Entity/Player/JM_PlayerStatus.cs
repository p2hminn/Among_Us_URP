using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_PlayerStatus : MonoBehaviourPun
{
    Animator anim;
    // ������ ��ü�� �����Ѵ�.
    // ���ӸŴ������� ����?
    // ��ü ���� �÷��̾��� ��
    // �ϴ� ��ü �����ϴ°ź���

    // ��ü ��������
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
        // ������� ���� ��
        photonView.RPC("RPC_Dead", RpcTarget.AllBuffered);
        JM_CrewUI.instance.Die();
    } 
}
