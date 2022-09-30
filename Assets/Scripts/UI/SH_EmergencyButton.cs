using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_EmergencyButton : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        print("1111111");
        if (collision.gameObject.CompareTag("Crew") && transform.parent.GetComponent<JM_PlayerStatus>().enabled == true) 
        {
            print(transform.parent);
            print("2222222");
            GetComponent<SpriteRenderer>().enabled = true;
            SH_RoomUI.instance.btnCrewUse.GetComponent<Button>().interactable = true;
        }
        else
        {
            print("33333333");
            GetComponent<SpriteRenderer>().enabled = false;
            SH_RoomUI.instance.btnCrewUse.GetComponent<Button>().interactable = false;
        }
    }
}
