using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIFTrigger : MonoBehaviour
{
    private float randomTimer = 1;
    private bool IsZone = false;

    SpriteRenderer sr;
    public Sprite blackOut;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = blackOut;

        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsZone) return;

        if (IsZone) randomTimer -= 1 * Time.deltaTime;

        if(IsZone && randomTimer <= 0)
        {
            anim.enabled = true;
            //play chosen audio too
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            IsZone = true;
            randomTimer = Random.Range(0.25f,12);
            //print(randomTimer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsZone = false;
            randomTimer = 1;
            anim.enabled = false;
            sr.sprite = blackOut;
        }
    }
}
