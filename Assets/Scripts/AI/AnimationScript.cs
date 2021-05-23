﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    //animation trigger vars
    RipperAIMain MonsterScript;
    FindPoints searchScript;

    private Animator ripAnimate;
    private bool idle = true;
    private bool walk = false;
    private bool angryWalk = false;
    private bool search = false;
    private bool attack = false;
    private bool jump = false;
    private bool run = false;

    // Start is called before the first frame update
    void Start()
    {
        MonsterScript = GetComponent<RipperAIMain>();
        searchScript = GetComponent<FindPoints>();
        ripAnimate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        setAnimations();
        
    }

    private void setAnimations()
    {
        if (!MonsterScript.destroyObj)
        {
            if (MonsterScript.aiState == "Still")
            {
                AnimationTriggers("Idle");
            }
            else if (MonsterScript.aiState == "Chase" || MonsterScript.aiState == "Track")
            {
                AnimationTriggers("Chase");
            }
            else if (MonsterScript.aiState == "Patrol")
            {
                if (MonsterScript.IsScreaming)
                {
                    AnimationTriggers("jump");
                }
                else 
                {
                    AnimationTriggers("Walk");
                }
            }
            else if (MonsterScript.aiState == "Search")
            {
                if (searchScript.targetCopy) 
                {
                    float distanceBetweenTarget = Vector3.Distance(transform.position, searchScript.targetCopy.transform.position);
                    if (distanceBetweenTarget <= 0.1f)
                    {
                        //search animation
                        AnimationTriggers("Search");
                    }
                    else
                    {
                        AnimationTriggers("Walk");
                    }
                }
            }
            else if (MonsterScript.aiState == "ChasePlus")
            {
                AnimationTriggers("run");
            }
        }
        else
        {
            AnimationTriggers("Attack");
        }
    }

    private void AnimationTriggers(string state) 
    {
        if (state == "Idle") 
        {
            idle = true;
            walk = false;
            angryWalk = false;
            search = false;
            attack = false;
            jump = false;
            run = false;
        }
        if (state == "Chase")
        {
            idle = false;
            walk = false;
            angryWalk = true;
            search = false;
            attack = false;
            jump = false;
            run = false;
        }
        if (state == "Search")
        {
            idle = false;
            walk = false;
            angryWalk = false;
            search = true;
            attack = false;
            jump = false;
            run = false;
        }
        if (state == "Attack") 
        {
            idle = false;
            walk = false;
            angryWalk = false;
            search = false;
            attack = true;
            jump = false;
            run = false;
        }
        if (state == "Walk")
        {
            idle = false;
            walk = true;
            angryWalk = false;
            search = false;
            attack = false;
            run = false;
            jump = false;
        }
        if (state == "jump")
        {
            idle = false;
            walk = false;
            angryWalk = false;
            search = false;
            attack = false;
            run = false;
            jump = true;
        }
        if (state == "run")
        {
            idle = false;
            walk = false;
            angryWalk = false;
            search = false;
            attack = false;
            jump = false;
            run = true;
        }
        ripAnimate.SetBool("isIdle", idle);
        ripAnimate.SetBool("IsAngryWalking", angryWalk);
        ripAnimate.SetBool("IsSearching", search);
        ripAnimate.SetBool("IsAttacking", attack);
        ripAnimate.SetBool("isWalking", walk);
        ripAnimate.SetBool("IsJumping", jump);
        ripAnimate.SetBool("IsRunning", run);
    }
}