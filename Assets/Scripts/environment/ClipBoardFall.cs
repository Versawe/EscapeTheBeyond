using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this makes an audio sound and dialogue cue happen when the clipboard hits the ground in GlitchyStart scene
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
            AudioController.PlayDialogueSound(2);
        } 
    }
}
