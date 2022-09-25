using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_CalibrateTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject yellowWheel;
    public GameObject blueWheel;
    public GameObject mintWheel;

    public bool isYellow;
    public bool isBlue;
    public bool isMint;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(isYellow);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == yellowWheel)
        {
            isYellow = true;
        }
        if (collision.gameObject == blueWheel)
        {
            isBlue = true;
        }
        if (collision.gameObject == mintWheel)
        {
            isMint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == yellowWheel)
        {
            isYellow = false;
        }
        if (other.gameObject == blueWheel)
        {
            isBlue = false;
        }
        if (other.gameObject == mintWheel)
        {
            isMint = false;
        }
    }

}
