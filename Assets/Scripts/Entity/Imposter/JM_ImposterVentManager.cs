using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ImposterVentManager : MonoBehaviour
{
    bool isUp;
    bool isDown;
    bool isNormal;
    Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            transform.position += Vector3.up * 5 * Time.deltaTime;
        }
        if (isDown)
        {
            transform.position -= Vector3.up * 5 * Time.deltaTime;
            if (Vector3.Distance(originPos, transform.position) <= 0.1f)
            {
                transform.position = originPos;
                isDown = false;
                isNormal = true;
            }
        }
    }

    public void GoUp()
    {
        isUp = true;
    }

    public void GoDown()
    {
        isUp = false;
        isDown = false;
    }

}
