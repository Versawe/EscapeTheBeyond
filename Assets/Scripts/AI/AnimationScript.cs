using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    //animation trigger vars
    AIMain MonsterScript;
    FindPoints searchScript;

    private Animator AIAnimator;
    private bool idle = true;
    private bool walk = false;
    private bool angryWalk = false;
    private bool search = false;
    private bool attack = false;
    private bool jump = false;
    private bool run = false;

    //NOTE: This is for the RIPPER, might need seperate aniation scripts for AI!!

    // Start is called before the first frame update
    void Start()
    {
        MonsterScript = GetComponent<AIMain>();
        searchScript = GetComponent<FindPoints>();
        AIAnimator = GetComponent<Animator>();
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
                AnimationTriggers("Idle"); //idle
            }
            else if (MonsterScript.aiState == "Chase" || MonsterScript.aiState == "Track")
            {
                AnimationTriggers("Chase"); //angrywalk
            }
            else if (MonsterScript.aiState == "Patrol")
            {
                if (MonsterScript.IsScreaming)
                {
                    if (MonsterScript.IsRipper) AnimationTriggers("jump");
                    else AnimationTriggers("Chase");
                }
                else 
                {
                    AnimationTriggers("Walk"); //walking
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
                        if (MonsterScript.IsRipper) AnimationTriggers("Search");
                        else AnimationTriggers("Idle");
                    }
                    else
                    {
                        AnimationTriggers("Walk");
                    }
                }
            }
            else if (MonsterScript.aiState == "ChasePlus")
            {
                if(MonsterScript.IsRipper) AnimationTriggers("run");
                else AnimationTriggers("Chase");
            }
        }
        else
        {
            AnimationTriggers("Attack"); //attacking
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

        AIAnimator.SetBool("isIdle", idle);
        AIAnimator.SetBool("IsAngryWalking", angryWalk);
        AIAnimator.SetBool("IsSearching", search);
        AIAnimator.SetBool("IsAttacking", attack);
        AIAnimator.SetBool("isWalking", walk);
        AIAnimator.SetBool("IsJumping", jump);
        AIAnimator.SetBool("IsRunning", run);
    }
}
