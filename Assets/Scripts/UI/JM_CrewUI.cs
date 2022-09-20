using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_CrewUI : MonoBehaviour
{
    // singleton
    public  static JM_CrewUI instance;

    // UI 사용될 크루 및 임포스터 오브젝트
    public GameObject imposter;
    public GameObject crew;

    // 크루 죽는 UI
    public GameObject crewDieUI;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (crewDieUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            crewDieUI.SetActive(false);
        }
    }

    public void Die()
    {
        crewDieUI.SetActive(true);
    }

}
