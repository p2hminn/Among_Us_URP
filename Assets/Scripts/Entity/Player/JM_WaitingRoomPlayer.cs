using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_WaitingRoomPlayer : MonoBehaviourPun
{
    // �÷��̾� �̵��ӵ�
    public float playerSpeed = 3;
    float originSpeed;

    // Rigidbody2D
    // Rigidbody2D rb;

    // �����̴��� ����
    bool isMoving;

    // �������� ����
    bool isImposter;

    // �ִϸ�����
    Animator anim;

    JM_PlayerStatus playerCode;

    // ī�޶�
    public Camera cam;


    // ���� ��ġ
    Vector3 receivePos;

    // ȸ���Ǿ� �ϴ� ��
    Quaternion receiveRot;

    // ���� �ӵ�
    public float lerpSpeed = 100;

    void Start()
    {
        if (photonView.IsMine) cam.gameObject.SetActive(true);
        // ���� �ӵ� ����
        originSpeed = playerSpeed;

        // rb = GetComponent<Rigidbody2D>();
        
        playerCode = GetComponent<JM_PlayerStatus>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        // ���� �� ���� �ƴ϶�� �Լ��� �����ڴ�
        if (!photonView.IsMine) return;

        // �̵� ��ǲ �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ��ǲ�� ���� ��� 
        if (h != 0f || v != 0f)
        {
            // �̵��Լ� ����
            Move(h, v);
            // �̵� �� 
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        photonView.RPC("RPC_SetBool", RpcTarget.All, isMoving);
    }

    // �̵� �Լ�
    private void Move(float h, float v)
    {
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();

        // ���� ����
        Vector3 playerDir = h * Vector3.right + v * Vector3.up;
        playerDir.Normalize();


        // �̵��������� ���ϵ��� �������� ����
        // �ϴ� �Լ��� �� �� ���濡 ����ȭ

        ChangeDir(playerDir);

        // �̵�
        transform.position += playerDir * playerSpeed * Time.deltaTime;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ������
        if (stream.IsWriting) // isMine == true
        {
            // position, rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        // ������ �ޱ�
        if (stream.IsReading) // isMine == false
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();

        }
    }

    void ChangeDir(Vector3 dir)
    {
        photonView.RPC("RPC_ChangeDir", RpcTarget.All, dir);
    }

    [PunRPC]
    public void RPC_ChangeDir(Vector3 dir)
    {
        if (dir.x > 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (dir.x < 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    [PunRPC]
    public void RPC_SetBool(bool bl)
    {
        anim.SetBool("IsMoving", bl);
    }
}
