using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterSkillManager : MonoBehaviour
{

    Animator anim;
    JM_PlayerMove pm;

    public enum State
    {
        idle,
        move,
        mission,
        vent,
        kill,
    }
    State state;

    // 기존 속도를 저장할 변수 생성
    float curPlayerSpeed;

    // 임포스터가 플레이어를 죽일 수 있는 범위
    float killDist;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<JM_PlayerMove>();
        curPlayerSpeed = pm.playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어와의 거리가 죽일 수 있는 거리라면 죽이는 상태로 전환한다. 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            state = State.kill;
            anim.SetTrigger("Imposter_Kill");
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Imposter_Kill") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {

            pm.playerSpeed = 0;
        }
        else
        {
            pm.playerSpeed = curPlayerSpeed;
        }



    }

    // 플레이어랑 닿아있는 동안
    private void OnTriggerStay(Collider other)
    {
        print("trigger working");
        
    }
}
