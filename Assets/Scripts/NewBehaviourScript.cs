using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    Material mat;
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor("_PlayerColor", Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
