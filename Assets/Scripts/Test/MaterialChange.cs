using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    public Material mat;
    public GameObject crew2;


    void Start()
    {
        // ��Ƽ���� ���� ������ ���� �ٲٱ�
        Material mat = (Material) Material.Instantiate(Resources.Load("Materials/Voting_Crew_Mat"));
        crew2.GetComponent<SpriteRenderer>().material = mat;
    }

    void Update()
    {
        
    }
}
