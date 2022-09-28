using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_VentTrigger : MonoBehaviour
{
    Color defaultColor;
    Color triggerColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(1, 1, 1, 1);
        triggerColor = new Color(255f / 255f, 62f / 255f, 62f / 255f, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Crew") && collision.gameObject.GetComponent<JM_ImposterStatus>().enabled)
        {
            GetComponent<SpriteRenderer>().color = triggerColor;
            JM_ImposterUI.instance.isVent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
        JM_ImposterUI.instance.isVent = false;
    }

}
