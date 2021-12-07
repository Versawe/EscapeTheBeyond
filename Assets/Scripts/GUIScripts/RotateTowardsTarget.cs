using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to pan the camera back in forth in front of mirror. Used in main menu and story audio scene
public class RotateTowardsTarget : MonoBehaviour
{
    public bool IsCameraFixed = true;
    public GameObject MMUI;
    public SlotSave slotScript;

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
        if (MMUI != null) slotScript = MMUI.GetComponent<SlotSave>();
        else slotScript = null;
    }
    private void LateUpdate() //camera movement always seems smoother in late update for some reason
    {
        MoveCam();
        RotateCam();
    }

    private void MoveCam() //logic to move camera back and forth
    {
        //will use for future "locking" camera onto mirror for puzzle 3
        //Movement should be called before rotation, or else it's kinda glitchy
        if (!IsCameraFixed) return;
        if (slotScript != null && MMUI != null) 
        {
            if (slotScript.IsJustStarted) moveSpeed = 15f;
        }
        //make object move back and forth slightly
        SinValue = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        float ClampedSinValue = Mathf.Clamp(SinValue, 3.5f, 5f);
        Vector3 moveVec = new Vector3(transform.position.x, transform.position.y, startPos.z + SinValue);
        transform.position = moveVec;

    }

    private void RotateCam() //lock rotation onto target (mirror for most all cases in game)
    {
        //make object look at target
        Vector3 dir = (transform.position - target.position) * -1;
        targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = targetRotation;
    }
}
