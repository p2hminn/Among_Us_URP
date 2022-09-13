using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PlayerStatus : MonoBehaviour
{
    Animator anim;
    public enum State
    {
        idle, 
        move,
        mission,
        die,
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.die)
        {
            SpriteRenderer sr;
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
            anim.SetTrigger("Die");
        }
        
    }

    
}
