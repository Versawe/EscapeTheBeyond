using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class JumpScareTrigger : MonoBehaviour
{
    public GameObject ScareObj;
    
    //Anim Controllers
    public List<AnimatorController> scareGifs = new List<AnimatorController>();

    Animator currAnim;
    SpriteRenderer sr;

    private float scareChance = 20f; //actual > chance ? scare!
    private float scareActual = 0f;
    private float scareTimer = 0f;
    private bool IsTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        currAnim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = null;

        ScareObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTriggered) return;
        scareTimer -= 1 * Time.deltaTime;
        if (scareTimer <= 0) 
        {
            scareActual = 0;
            scareTimer = 0;
            currAnim.runtimeAnimatorController = null;
            sr.sprite = null;
            ScareObj.SetActive(false);
            IsTriggered = false;
            NoDestroy.TriggerScarePP = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!IsTriggered) 
            {
                scareActual = Random.Range(1, scareChance+1);
                scareTimer = Random.Range(0.25f, 2f);
                print("Do Once!");
            }
            if (scareActual >= scareChance && !IsTriggered)
            {
                float chooseAnim = Random.Range(0, scareGifs.Count);
                currAnim.runtimeAnimatorController = scareGifs[(int)chooseAnim];
                ScareObj.SetActive(true);
                print("Triggered, do once!");
                IsTriggered = true;
                NoDestroy.TriggerScarePP = true;
            }
            if (!IsTriggered) 
            {
                IsTriggered = false;
                sr.sprite = null;
                print("Not triggered, do once!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            NoDestroy.TriggerScarePP = false;
            IsTriggered = false;
            sr.sprite = null;
            scareActual = 0;
            scareTimer = 0;
            currAnim.runtimeAnimatorController = null;
            ScareObj.SetActive(false);
        }
        
    }
}
