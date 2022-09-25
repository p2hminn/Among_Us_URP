using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_TaskCompletedUI : MonoBehaviour
{
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 2)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
