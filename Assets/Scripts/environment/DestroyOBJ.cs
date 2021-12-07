using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this destroys game objects after 20 seconds
public class DestroyOBJ : MonoBehaviour
{
    public float despawn = 20;


    private void FixedUpdate()
    {
        despawn -= 1 * Time.deltaTime;

        if (despawn <= 0)
        {
            Destroy(gameObject);
        }
    }
}
