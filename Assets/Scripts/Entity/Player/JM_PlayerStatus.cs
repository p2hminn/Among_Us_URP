using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_PlayerStatus : MonoBehaviourPun
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
