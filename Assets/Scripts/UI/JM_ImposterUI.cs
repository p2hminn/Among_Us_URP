using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JM_ImposterUI : MonoBehaviourPun
{
    // singleton
    public static JM_ImposterUI instance;
    private void Awake()
    {
        instance = this;
    }

    // 임포스터 상태 코드
    public JM_ImposterStatus imposterCode;

    // 현재시간
    float currentTime;

    // ** 임포스터 공격 관련 **

    // 공격버튼 활성화 가능 여부
    public bool isButtonActivate;

    // 임포스터 공격가능여부
    public bool isAttackOK;
    // 공격버튼 
    public Button attackButton;
    // 공격 쿨타임
    public float attackCoolTime;

    // 공격당하는 크루
    public GameObject victimCrew;

    // 임포스터 및 크루 색상
    [SerializeField]
    public Color imposterColor;
    [SerializeField]
    public Color crewColor;

    // 리포트 버튼
    public Button reportButton;

    // Use 버튼
    public Button useButton;
    public bool isUseable;

    // 임포스터 벤트 여부
    public bool isVent;

    public Button ventButton;
    public Button sabotageButton;

    // Start is called before the first frame update
    void Start()
    {
        reportButton.interactable = false;
        // 원래 커스텀에서 조정이 가능하지만 일단 어택쿨타임 지정 10초로
        attackCoolTime = 3;
        attackButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 공격버튼 사용 불가능 상태일때 쿨타임 함수 실행
        if (!isButtonActivate)
            AttackTimeCount();

        if (isAttackOK && isButtonActivate)
        {
            attackButton.interactable = true;
        }
        else if (!isAttackOK)
        {
            attackButton.interactable = false;
        }

        // 벤트
        if (isVent)
        {
            print("isVent status");
            sabotageButton.gameObject.SetActive(false);
            ventButton.gameObject.SetActive(true);
            ventButton.interactable = true;
        }
        if (!isVent)
        {
            sabotageButton.gameObject.SetActive(true);
            ventButton.gameObject.SetActive(false);
        }
        print(isUseable);


        // Use 버튼
        if (isUseable)
        {
            useButton.interactable = true;
            print("true");
        }
        else if (!isUseable)
        {
            useButton.interactable = false;
            print("false");
        }
    }

    // 공격버튼 누르면 임포스터코드에서 공격함수 실행
    public void ClickAttack()
    {
        // imposterCode.Attack();
        // 플레이어 죽는 함수 호출
        victimCrew.GetComponent<JM_PlayerMove>().Dead(crewColor.r, crewColor.g, crewColor.b, crewColor.a, 
            imposterColor.r, imposterColor.g, imposterColor.b, imposterColor.a);

        isButtonActivate = false;
    }

    public void ClickVent()
    {
        imposterCode.anim.SetTrigger("Vent");
    }

    // 임포스터 공격을 할 수 있는 상태가 되려면 쿨타임이 있어야된다
    // 버튼 쿨타임 숫자 카운트다운은 나중에 추가 예정
    void AttackTimeCount()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= attackCoolTime)
        {
            isButtonActivate = true;
        } 
    }

    public GameObject map;

    public void OnClickMap()
    {
        map.SetActive(true);
    }

    public void onClickUse()
    {
        if (SH_RoomUI.instance.isEmergency)
        {
            SH_RoomUI.instance.photonView.RPC("EmergencyMeeting", RpcTarget.All);
        }
    }

}
