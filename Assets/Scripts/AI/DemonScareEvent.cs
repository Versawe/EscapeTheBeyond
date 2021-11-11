using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonScareEvent : MonoBehaviour
{
    NavMeshAgent nm;
    GameObject Player;
    public GameObject ScareScene;

    public bool IsScaring = false;
    private float scareDis = 1.75f;
    private float scareTimer = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("FPSController");
        nm = GetComponent<NavMeshAgent>();

        nm.SetDestination(Player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        TriggerScare();
        nm.SetDestination(Player.transform.position);
    }

    private void TriggerScare()
    {
        float dist = Vector3.Distance(Player.transform.position, transform.position);
        if (dist <= scareDis)
        {
            IsScaring = true;
            ScareScene.SetActive(true);
        }

        if (IsScaring)
        {
            scareTimer -= 1 * Time.deltaTime;
            Player.GetComponentInChildren<CameraRotationFirstPerson>().enabled = false;
            Player.GetComponent<CharacterMovementFirstPerson>().enabled = false;
            Player.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (IsScaring && scareTimer <= 0)
        {
            NoDestroy.atGameOver = true;
        }
    }
}
