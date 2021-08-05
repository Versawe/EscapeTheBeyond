using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardFall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "soundOnCollide") print("sound");
    }
}
