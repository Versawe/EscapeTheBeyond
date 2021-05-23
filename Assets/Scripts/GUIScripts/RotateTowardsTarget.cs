using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    public bool IsCameraFixed = true;

    public Transform target;
    Quaternion targetRotation;
    Vector3 startPos;

    private float SinValue;
    public float moveSpeed = 1.5f;
    public float moveAmount = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        //SinValue = transform.position.z;
        startPos = transform.position;
    }
    private void LateUpdate()
    {
        MoveCam();
        RotateCam();
    }

    private void MoveCam()
    {
        //will use for future "locking" camera onto mirror for puzzle 3
        //Movement should be called before rotation, or else it's kinda glitchy
        if (!IsCameraFixed) return;

        //make object move back and forth slightly
        SinValue = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        float ClampedSinValue = Mathf.Clamp(SinValue, 3.5f, 5f);
        Vector3 moveVec = new Vector3(transform.position.x, transform.position.y, startPos.z + SinValue);
        transform.position = moveVec;
    }

    private void RotateCam()
    {
        //make object look at target
        Vector3 dir = (transform.position - target.position) * -1;
        targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = targetRotation;
    }
}
