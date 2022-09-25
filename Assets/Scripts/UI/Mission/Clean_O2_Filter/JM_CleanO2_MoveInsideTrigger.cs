using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_CleanO2_MoveInsideTrigger : MonoBehaviour
{
    public GameObject on1;
    public GameObject on2;
    public Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        on1.SetActive(true);
        on2.SetActive(true);
        //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        collision.gameObject.GetComponent<JM_CleanO2Filter>().leafDir = Vector3.left;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.GetComponent<JM_CleanO2Filter>().leafDir);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        on1.SetActive(false);
        on2.SetActive(false);
        
    }

}
