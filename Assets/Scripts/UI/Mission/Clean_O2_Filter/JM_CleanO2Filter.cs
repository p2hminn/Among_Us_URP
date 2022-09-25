using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JM_CleanO2Filter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 originPos;
    Vector3 mousePosition;
    public Vector3 leafDir;

    public Rigidbody2D rb;
    bool movingByOwn;
    public bool triggerDetected;
    float speed;

    public void OnBeginDrag(PointerEventData eventData)
    {
        movingByOwn = false;
        originPos = transform.position;
        print("originPos : " + originPos.x + ", " + originPos.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 objPosition = Camera.main.ScreenToViewportPoint(mousePosition);
        CalculateDir();
        transform.position = mousePosition;
        rb.velocity = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        movingByOwn = true;
        speed = 150;
        print("leafDir : " + leafDir.x + ", " + leafDir.y);
        rb.AddForce(leafDir * 10000);
    }

    void CalculateDir()
    {
        leafDir = mousePosition - originPos;
        leafDir.Normalize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (movingByOwn)
        {
            // transform.position += leafDir * speed * Time.deltaTime;
            //rb.AddForceAtPosition(leafDir * 10, mousePosition);
            rb.AddRelativeForce(leafDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed *= 0.2f;
    }

    
}
