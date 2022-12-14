using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_VentTrigger : MonoBehaviourPun
{

    Color defaultColor;
    Color triggerColor;
    Color insideColor;

    public GameObject imposter;

    public GameObject dir1;
    public GameObject dir2;
    public Transform pos1;
    public Transform pos2;

    public bool isInVent;

    public JM_ImposterStatus imposterCode;
    public JM_PlayerMove imposterMoveCode;

    float originSpeed;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(1, 1, 1, 1);
        triggerColor = new Color(255f / 255f, 62f / 255f, 62f / 255f, 1);
        insideColor = new Color(255f / 255f, 173 / 255f, 64 / 255f, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;
        dir1.SetActive(false);
        dir2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInVent)
        {
            imposterCode.SetInvisible();

            GetComponent<SpriteRenderer>().color = insideColor;

            if (gameObject.name.Contains("-1"))
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    GoToFirstPos();
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    GoToSecondPos();
                }
            }
            if (gameObject.name.Contains("-2"))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GoToFirstPos();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    GoToSecondPos();
                }
            }
            if (gameObject.name.Contains("-3"))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GoToFirstPos();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    GoToSecondPos();
                }
            }
        }        

        if (!isInVent)
        {
            GetComponent<SpriteRenderer>().color = defaultColor;
        }
    }

    void GoToFirstPos()
    {
        // ?????? 1?? ??????
        imposter.transform.position = new Vector3(pos1.position.x, pos1.position.y + 0.5f, pos1.position.z);
        // 
        //imposter.GetComponentInChildren<SpriteRenderer>().enabled = true;
        //imposter.transform.Find("Canvas").gameObject.SetActive(true);

        imposterCode.SetVisible();

        isInVent = false;
        imposterCode.isInVent = false;
        imposterCode.isOutVent = true;

        // ???????? ?????????? ????
        imposterCode.GetOutsideVent();
        imposterCode.originPos = new Vector3(pos1.position.x, pos1.position.y + 0.5f, pos1.position.z);
    }

    void GoToSecondPos()
    {
        // ?????? 2?? ??????
        imposter.transform.position = new Vector3(pos2.position.x, pos2.position.y + 0.5f, pos2.position.z);

        //imposter.GetComponentInChildren<SpriteRenderer>().enabled = true;
        //imposter.transform.Find("Canvas").gameObject.SetActive(true);

        imposterCode.SetVisible();

        isInVent = false;
        imposterCode.isInVent = false;
        imposterCode.isOutVent = true;

        imposterCode.GetOutsideVent();
        imposterCode.originPos = new Vector3(pos2.position.x, pos2.position.y + 0.5f, pos2.position.z);
    }

    


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew") && collision.gameObject.GetComponent<JM_ImposterStatus>().enabled)
        {
            GetComponent<SpriteRenderer>().color = triggerColor;

            imposter = collision.gameObject;
            imposterCode = collision.gameObject.GetComponent<JM_ImposterStatus>();
            imposterMoveCode = collision.gameObject.GetComponent<JM_PlayerMove>();

            originSpeed = imposterMoveCode.playerSpeed;

            JM_ImposterUI.instance.isVent = true;
            JM_ImposterUI.instance.imposterCode = collision.gameObject.GetComponent<JM_ImposterStatus>();
            JM_ImposterUI.instance.imposterPos = collision.gameObject;

            collision.gameObject.GetComponent<JM_ImposterStatus>().ventCode = this;
            collision.gameObject.GetComponent<JM_ImposterStatus>().isOne = true;

            dir1.SetActive(true);
            dir2.SetActive(true);


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
        JM_ImposterUI.instance.isVent = false;

        dir1.SetActive(false);
        dir2.SetActive(false);
    }

}
