using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ImposterVentManager : MonoBehaviour
{
    bool isUp;
    bool isDown;
    bool isNormal;
    public Vector3 originPos;

    public JM_ImposterStatus imposterCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void GoUp()
    {
        imposterCode.isUp = true;
    }

    public void GoDown()
    {
        imposterCode.isUp = false;
        imposterCode.isDown = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        imposterCode.isOne = false;
        imposterCode.isSecond = false;
        imposterCode.isThird = false;
    }

}
