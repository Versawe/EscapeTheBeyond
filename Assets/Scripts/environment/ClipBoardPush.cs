using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClipBoardPush : MonoBehaviour
{
    GameObject noDestroy;
    public GameObject ClipBoardPrefab;
    public GameObject ClipBoardRef;
    public Rigidbody ClipBoardBody;

    private bool IsClipboardSpawned = false;
    private float clipboardFallTimer = 60;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("NoDestroyOBJ")) noDestroy = GameObject.Find("NoDestroyOBJ");
        else noDestroy = null;

        if (NoDestroy.gameProgression == 1 && NoDestroy.puzzleOneLoginAttempts == 2) IsClipboardSpawned = true;
        if (IsClipboardSpawned) 
        {
            ClipBoardRef = Instantiate(ClipBoardPrefab, transform.position, transform.rotation);
            ClipBoardBody = ClipBoardRef.GetComponent<Rigidbody>();
        } 
        else 
        {
            ClipBoardRef = null;
            ClipBoardBody = null;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClipboardSpawned) clipboardFallTimer -= 1 * Time.deltaTime;
        else return;

        if (clipboardFallTimer <= 0) Push();
    }

    public void Push() //Push the Clipboard onto the ground
    {
        
    }
}
