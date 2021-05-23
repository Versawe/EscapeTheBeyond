using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySP : MonoBehaviour
{
    private float despawn = 20;


    private void FixedUpdate()
    {
        despawn -= 1 * Time.deltaTime;

        if (despawn <= 0)
        {
            Destroy(gameObject);
        }
    }
}
