using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardFall : MonoBehaviour
{
    AudioSource source;

    void Awake() 
    {
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "soundOnCollide") 
        {
            source.Play();
            print("thud!");
            AudioController.PlayDialogueSound(2);
        } 
    }
}
