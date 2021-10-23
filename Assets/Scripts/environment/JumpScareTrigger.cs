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

    private float scareChance = 8f; //actual > chance ? scare!
    private float scareActual = 0f;
    private float scareTimer = 0f;
    private bool IsTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        currAnim = GetComponentInChildren<Animator>();
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
            ScareObj.SetActive(false);
            IsTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            IsTriggered = true;
            scareActual = Random.Range(1, 10);
            scareTimer = Random.Range(0.25f, 2f);
            if (scareActual >= scareChance)
            {
                ScareObj.SetActive(true);
                float chooseAnim = Random.Range(0, scareGifs.Count - 1);
                currAnim.runtimeAnimatorController = scareGifs[(int)chooseAnim];
            }
            print(scareActual);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsTriggered = false;
            scareActual = 0;
            scareTimer = 0;
            currAnim.runtimeAnimatorController = null;
            ScareObj.SetActive(false);
        }
        
    }
}
