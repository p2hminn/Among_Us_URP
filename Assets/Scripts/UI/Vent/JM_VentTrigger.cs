using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_VentTrigger : MonoBehaviour
{

    Color defaultColor;
    Color triggerColor;
    Color insideColor;

    public GameObject imposter;

    public GameObject dir1;
    public GameObject dir2;
    public Transform pos1;
    public Transform pos2;

    public bool isInVent;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(1, 1, 1, 1);
        triggerColor = new Color(255f / 255f, 62f / 255f, 62f / 255f, 1);
        insideColor = new Color(255f / 255f, 173 / 255f, 64 / 255f, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInVent)
        {
            imposter.GetComponentInChildren<SpriteRenderer>().enabled = false;
            imposter.transform.Find("Canvas").gameObject.SetActive(false);

            GetComponent<SpriteRenderer>().color = insideColor;
            if (Input.GetKeyDown(KeyCode.A))
            {
                // 위치를 1번 위치로
                imposter.transform.position = pos1.position;
                // 
                imposter.GetComponentInChildren<SpriteRenderer>().enabled = true;
                imposter.transform.Find("Canvas").gameObject.SetActive(false);

                isInVent = false;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                // 위치를 2번 위치로
                imposter.transform.position = pos2.position;
                imposter.GetComponentInChildren<SpriteRenderer>().enabled = true;
                imposter.transform.Find("Canvas").gameObject.SetActive(false);
                isInVent = false;
            }
        }
        if (!isInVent)
        {
            GetComponent<SpriteRenderer>().color = defaultColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew") && collision.gameObject.GetComponent<JM_ImposterStatus>().enabled)
        {
            GetComponent<SpriteRenderer>().color = triggerColor;

            imposter = collision.gameObject;

            JM_ImposterUI.instance.isVent = true;
            JM_ImposterUI.instance.imposterCode = collision.gameObject.GetComponent<JM_ImposterStatus>();

            collision.gameObject.GetComponent<JM_ImposterStatus>().ventCode = this;
            dir1.SetActive(true);
            dir2.SetActive(true);


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
        JM_ImposterUI.instance.isVent = false;
        dir1.SetActive(false);
        dir2.SetActive(false);
    }

}
