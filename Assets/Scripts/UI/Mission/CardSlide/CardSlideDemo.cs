using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class CardSlideDemo : MonoBehaviourPun, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cardSlideUI;

    Vector3 originPos;
    bool isMissionStart;
    bool isGoBack;
    public bool isSlideStart;
    bool isSliding;
    public Transform startPos;
    public Transform endPos;

    public float currentTime;

    public GameObject greenDark;
    public GameObject redDark;

    public JM_MissionStatus missionStatus;

    public bool isMissionDone;

    public GameObject taskCompletedUI;

    public GameObject missionTrigger;

    void Start()
    {
        originPos = transform.position;
        isMissionStart = true;
    }

    void Update()
    {
        if (isGoBack)
        {
            transform.position = Vector3.Lerp(transform.position, originPos, Time.deltaTime * 3);         
        }
        if (Vector3.Distance(transform.position, originPos) < 0.5f)
        {
            transform.position = originPos;
            isGoBack = false;
            isMissionStart = true;
            redDark.SetActive(true);
            greenDark.SetActive(true);
            if (isMissionDone)
            {
                // ������ ����
                cardSlideUI.SetActive(false);
            }
        }

        if (isSlideStart)
        {
            transform.position = Vector3.Lerp(transform.position, startPos.position, Time.deltaTime * 3);
            isMissionStart = false;
        }
        if (Vector3.Distance(transform.position, startPos.position) < 0.5f)
        {
            transform.position = startPos.position;
        }

        if (transform.position == startPos.position)
        {
            isSlideStart = false;
            isSliding = true;
        }
        if (isSliding)
        {
            if (transform.position.x > startPos.position.x)
            {
                CountTime();
            }

            if (transform.position.x > endPos.position.x)
            {
                print("ī��ܱ� ��");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("2");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ī�� ��� �̵��Ҷ��� ���� ��ġ�� ���ư��� �ʴ´�
        isGoBack = false;

        // ó������ ������� �̵� ����
        if (isMissionStart)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToViewportPoint(mousePosition);
            transform.position = mousePosition;
        }

        // �����̴��� ���� �����̴� ���� ��ġ�� �̵�
        if (isSlideStart)
        {
            
        }

        // �ű⼭���ʹ� ���μ��ηθ� �̵� ����
        if (isSliding)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, startPos.position.y, 10);
            transform.position = mousePosition;
        }
     
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Reset();
    }

    void CountTime()
    {
        currentTime += Time.deltaTime;
        if (transform.position.x >= endPos.position.x)
        {
            if (currentTime > 2.0f && currentTime < 4.0f)
            {
                isMissionDone = true;
                Done();
                Reset();
                currentTime = 0;
            }
            else
            {
                Fail();
                Reset();
                currentTime = 0;
            }
        }
    }

    private void Reset()
    {
        isGoBack = true;
        isMissionStart = false;
        isSlideStart = false;
        isSliding = false;
    }

    void Done()
    {
        print("�� �ܾ���");
        greenDark.SetActive(false);
        //JM_MissionStatus.instance.isMissionDone = true;

        JM_MissionStatus.instance.SetMissionDone();
        JM_CrewMapManager.instance.SwipeCard();

        isMissionDone = true;
        taskCompletedUI.SetActive(true);

        missionTrigger.GetComponent<JM_MissionTrigger>().DisableTrigger();
    }

    void Fail()
    {
        print("����");
        redDark.SetActive(false);
    }

    public void OnClickCancel()
    {
        cardSlideUI.SetActive(false);
    }
}
