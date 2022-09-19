
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_UnfoldTrigger : MonoBehaviour
{
    Color defaultColor;
    Color triggerColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(1, 1, 1, 1);
        triggerColor = new Color(243f / 255f, 134f / 255f, 2f / 255f / 1f);
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAble)
        {
            print("able");
            if (Input.GetButtonDown("Jump"))
            {
                print("worked");
                gameUI.SetActive(true);
            }
        }
    }

    public GameObject gameUI;
    bool isAble;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew") && collision.gameObject.GetComponent<JM_PlayerStatus>().enabled)
        {
            GetComponent<SpriteRenderer>().color = triggerColor;
            isAble = true;

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}