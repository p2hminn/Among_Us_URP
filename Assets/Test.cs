using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h1 = Input.GetAxisRaw("Horizontal");
        float v1 = Input.GetAxisRaw("Vertical");
        // 방향 지정
        Vector2 playerDir1 = h1 * Vector2.right + v1 * Vector2.up;
        //playerDir1.Normalize();

        if (playerDir1.magnitude > 0)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = playerDir1 * 5;
        }
    }
}
