using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    public Material mat;
    public GameObject crew2;


    void Start()
    {
        // 머티리얼 새로 생성해 색깔 바꾸기
        Material mat = (Material) Material.Instantiate(Resources.Load("Materials/Voting_Crew_Mat"));
        crew2.GetComponent<SpriteRenderer>().material = mat;
    }

    void Update()
    {
        
    }
}
