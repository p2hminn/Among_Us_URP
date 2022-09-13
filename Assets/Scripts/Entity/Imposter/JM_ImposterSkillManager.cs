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

    // ���� �ӵ��� ������ ���� ����
    float curPlayerSpeed;

    // �������Ͱ� �÷��̾ ���� �� �ִ� ����
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
        // �÷��̾���� �Ÿ��� ���� �� �ִ� �Ÿ���� ���̴� ���·� ��ȯ�Ѵ�. 
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

    // �÷��̾�� ����ִ� ����
    private void OnTriggerStay(Collider other)
    {
        print("trigger working");
        
    }
}
