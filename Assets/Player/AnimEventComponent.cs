using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventComponent : MonoBehaviour
{
    Animator animator;

    bool running;
    MovmentComponent movmentComp;
    GroundChecker groundChecker;
    bool onGround = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movmentComp= GetComponent<MovmentComponent>();
        groundChecker = GetComponent<GroundChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateData();
        UpdateAnimation();
    }
    private void GenerateData()
    {
        Vector3 vel = movmentComp.Velocity;
        vel.y = 0;
        running = vel.magnitude > 0;
        bool prevOnGround = onGround;
        onGround = groundChecker.IsOnGround();
        if(onGround && !prevOnGround)
        {
            animator.SetTrigger("landed");
        }
    }
    private void UpdateAnimation()
    {
        animator.SetBool("inAir", !onGround);
        animator.SetBool("running", running);
    }


}
